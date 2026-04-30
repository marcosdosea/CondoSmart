using System.Text;
using Core;
using Core.Data;
using Core.DTO;
using Core.Identity;
using Core.Models;
using Core.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class MoradorProvisionamentoService : IMoradorProvisionamentoService
    {
        private readonly CondosmartContext _context;
        private readonly UserManager<UsuarioSistema> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;

        public MoradorProvisionamentoService(
            CondosmartContext context,
            UserManager<UsuarioSistema> userManager,
            RoleManager<IdentityRole> roleManager,
            IEmailService emailService)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailService = emailService;
        }

        public async Task<ProvisionamentoMoradorResultDTO> CadastrarComAcessoAsync(Morador morador, int unidadeId, string urlAcesso)
        {
            ValidarMoradorParaAcesso(morador);

            var unidade = await _context.UnidadesResidenciais
                .Include(u => u.Condominio)
                .FirstOrDefaultAsync(u => u.Id == unidadeId);

            if (unidade is null)
                throw new ArgumentException("A unidade selecionada nao foi encontrada.");

            if (morador.CondominioId != unidade.CondominioId)
                throw new ArgumentException("A unidade precisa pertencer ao mesmo condominio do morador.");

            if (unidade.MoradorId.HasValue)
                throw new ArgumentException("A unidade selecionada ja possui um morador vinculado.");

            if (await _context.Moradores.AnyAsync(m => m.Email == morador.Email))
                throw new ArgumentException("Ja existe um morador cadastrado com este e-mail.");

            if (await _userManager.FindByEmailAsync(morador.Email!) is not null)
                throw new ArgumentException("Ja existe um usuario de acesso com este e-mail.");

            await GarantirPerfilMoradorAsync();

            await using var transaction = await _context.Database.BeginTransactionAsync();
            UsuarioSistema? usuarioCriado = null;

            try
            {
                _context.Moradores.Add(morador);
                await _context.SaveChangesAsync();

                unidade.MoradorId = morador.Id;
                await _context.SaveChangesAsync();

                var senhaTemporaria = GerarSenhaTemporaria();
                usuarioCriado = new UsuarioSistema
                {
                    NomeCompleto = morador.Nome,
                    UserName = morador.Email,
                    Email = morador.Email,
                    PhoneNumber = morador.Telefone,
                    SenhaTemporaria = true
                };

                var createResult = await _userManager.CreateAsync(usuarioCriado, senhaTemporaria);
                if (!createResult.Succeeded)
                    throw new ArgumentException(string.Join(" ", createResult.Errors.Select(e => e.Description)));

                var roleResult = await _userManager.AddToRoleAsync(usuarioCriado, Perfis.Morador);
                if (!roleResult.Succeeded)
                    throw new ArgumentException(string.Join(" ", roleResult.Errors.Select(e => e.Description)));

                await transaction.CommitAsync();

                var resultado = new ProvisionamentoMoradorResultDTO
                {
                    MoradorId = morador.Id,
                    NomeMorador = morador.Nome,
                    Email = morador.Email!,
                    SenhaTemporaria = senhaTemporaria,
                    Condominio = unidade.Condominio.Nome,
                    Unidade = unidade.Identificador,
                    UrlAcesso = urlAcesso
                };

                try
                {
                    await _emailService.SendAsync(
                        resultado.Email,
                        resultado.NomeMorador,
                        "Bem-vindo ao CondoSmart",
                        MontarHtmlBoasVindas(resultado));

                    resultado.EmailEnviado = true;
                }
                catch (Exception ex)
                {
                    resultado.EmailEnviado = false;
                    resultado.ObservacaoEmail = ex.Message;
                }

                return resultado;
            }
            catch
            {
                await transaction.RollbackAsync();

                if (usuarioCriado is not null)
                {
                    var usuarioPersistido = await _userManager.FindByEmailAsync(usuarioCriado.Email!);
                    if (usuarioPersistido is not null)
                        await _userManager.DeleteAsync(usuarioPersistido);
                }

                throw;
            }
        }

        public async Task AtualizarVinculoUnidadeAsync(int moradorId, int unidadeId, int condominioId)
        {
            var unidadeDestino = await _context.UnidadesResidenciais.FirstOrDefaultAsync(u => u.Id == unidadeId);
            if (unidadeDestino is null)
                throw new ArgumentException("A unidade selecionada nao foi encontrada.");

            if (unidadeDestino.CondominioId != condominioId)
                throw new ArgumentException("A unidade precisa pertencer ao mesmo condominio do morador.");

            if (unidadeDestino.MoradorId.HasValue && unidadeDestino.MoradorId != moradorId)
                throw new ArgumentException("A unidade selecionada ja possui outro morador vinculado.");

            var unidadesAtuais = await _context.UnidadesResidenciais
                .Where(u => u.MoradorId == moradorId && u.Id != unidadeId)
                .ToListAsync();

            foreach (var unidade in unidadesAtuais)
                unidade.MoradorId = null;

            unidadeDestino.MoradorId = moradorId;
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarContaMoradorAsync(string? emailAnterior, Morador morador)
        {
            if (string.IsNullOrWhiteSpace(morador.Email))
                return;

            UsuarioSistema? usuario = null;
            if (!string.IsNullOrWhiteSpace(emailAnterior))
                usuario = await _userManager.FindByEmailAsync(emailAnterior);

            usuario ??= await _userManager.FindByEmailAsync(morador.Email);
            if (usuario is null || !await _userManager.IsInRoleAsync(usuario, Perfis.Morador))
                return;

            if (!string.Equals(usuario.Email, morador.Email, StringComparison.OrdinalIgnoreCase))
            {
                var usuarioComNovoEmail = await _userManager.FindByEmailAsync(morador.Email);
                if (usuarioComNovoEmail is not null && usuarioComNovoEmail.Id != usuario.Id)
                    throw new ArgumentException("Ja existe outro usuario com o e-mail informado.");
            }

            usuario.NomeCompleto = morador.Nome;
            usuario.Email = morador.Email;
            usuario.UserName = morador.Email;
            usuario.PhoneNumber = morador.Telefone;

            var resultado = await _userManager.UpdateAsync(usuario);
            if (!resultado.Succeeded)
                throw new ArgumentException(string.Join(" ", resultado.Errors.Select(e => e.Description)));
        }

        public async Task RemoverAcessoAsync(int moradorId, string? email)
        {
            var unidades = await _context.UnidadesResidenciais
                .Where(u => u.MoradorId == moradorId)
                .ToListAsync();

            foreach (var unidade in unidades)
                unidade.MoradorId = null;

            await _context.SaveChangesAsync();

            if (string.IsNullOrWhiteSpace(email))
                return;

            var usuario = await _userManager.FindByEmailAsync(email);
            if (usuario is not null && await _userManager.IsInRoleAsync(usuario, Perfis.Morador))
                await _userManager.DeleteAsync(usuario);
        }

        private async Task GarantirPerfilMoradorAsync()
        {
            if (!await _roleManager.RoleExistsAsync(Perfis.Morador))
                await _roleManager.CreateAsync(new IdentityRole(Perfis.Morador));
        }

        private static void ValidarMoradorParaAcesso(Morador morador)
        {
            if (string.IsNullOrWhiteSpace(morador.Nome))
                throw new ArgumentException("O nome do morador e obrigatorio.");

            if (string.IsNullOrWhiteSpace(morador.Email))
                throw new ArgumentException("O e-mail do morador e obrigatorio para criar o acesso.");

            if (!morador.CondominioId.HasValue || morador.CondominioId.Value <= 0)
                throw new ArgumentException("Selecione um condominio valido.");
        }

        private static string GerarSenhaTemporaria()
        {
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz23456789";
            var random = Random.Shared;
            var builder = new StringBuilder("Condo@");

            for (var i = 0; i < 6; i++)
                builder.Append(chars[random.Next(chars.Length)]);

            return builder.ToString();
        }

        private static string MontarHtmlBoasVindas(ProvisionamentoMoradorResultDTO resultado)
        {
            return $@"
                <div style='font-family: Arial, sans-serif; color: #1f2937; line-height: 1.6;'>
                    <h2>Bem-vindo ao CondoSmart</h2>
                    <p>Seu acesso ao sistema do condominio foi criado pela administracao.</p>
                    <p><strong>Login:</strong> {resultado.Email}</p>
                    <p><strong>Senha temporaria:</strong> {resultado.SenhaTemporaria}</p>
                    <p><strong>Condominio:</strong> {resultado.Condominio}</p>
                    <p><strong>Unidade:</strong> {resultado.Unidade}</p>
                    <p>Ao entrar pela primeira vez, voce precisara trocar a senha antes de continuar.</p>
                    <p><a href='{resultado.UrlAcesso}'>Clique aqui para acessar o sistema</a></p>
                </div>";
        }
    }
}
