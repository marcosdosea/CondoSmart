using Core;
using Core.Data;
using Core.Models;
using Core.Service;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Service
{
    /// <summary>
    /// Implementa serviços para manter dados do condomínio
    /// </summary>
    public class CondominioService : ICondominioService
    {
        private readonly CondosmartContext context;

        public CondominioService(CondosmartContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Criar um novo condomínio na base de dados
        /// </summary>
        /// <param name="condominio">dados do condomínio</param>
        /// <returns>id do condomínio</returns>
        /// <exception cref="ArgumentException"></exception>
        public int Create(Condominio condominio)
        {
            ValidarCondominio(condominio);

            context.Add(condominio);
            context.SaveChanges();
            return condominio.Id;
        }

        /// <summary>
        /// Editar dados do condomínio na base de dados
        /// </summary>
        /// <param name="condominio">dados do condomínio</param>
        /// <exception cref="ArgumentException"></exception>
        public void Edit(Condominio condominio)
        {
            ValidarCondominio(condominio);

            context.Update(condominio);
            context.SaveChanges();
        }

        /// <summary>
        /// Remover o condomínio da base de dados
        /// </summary>
        /// <param name="id">id do condomínio</param>
        public void Delete(int id)
        {
            var condominio = context.Condominios.Find(id);
            if (condominio != null)
            {
                context.Remove(condominio);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Buscar um condomínio na base de dados
        /// </summary>
        /// <param name="id">id do condomínio</param>
        /// <returns>dados do condomínio</returns>
        public Condominio? GetById(int id)
        {
            return context.Condominios.Find(id);
        }

        /// <summary>
        /// Buscar todos os condomínios cadastrados
        /// </summary>
        /// <returns>lista de condomínios</returns>
        public List<Condominio> GetAll()
        {
            return context.Condominios.AsNoTracking().ToList();
        }

        /// <summary>
        /// Valida regras básicas do condomínio (modelo parecido com o do professor)
        /// </summary>
        /// <param name="condominio"></param>
        /// <exception cref="ArgumentException"></exception>
        private void ValidarCondominio(Condominio condominio)
        {
            if (condominio == null)
                throw new ArgumentException("Condomínio inválido.");

            ValidarNome(condominio.Nome);
            ValidarCnpj(condominio.Cnpj, condominio.Id);
            ValidarRua(condominio.Rua);
            ValidarNumero(condominio.Numero);
            ValidarBairro(condominio.Bairro);
            ValidarCidade(condominio.Cidade);
            ValidarUf(condominio.Uf);
            ValidarCep(condominio.Cep);
            ValidarEmail(condominio.Email);
            ValidarTelefone(condominio.Telefone);
            ValidarUnidades(condominio.Unidades);
        }

        /// <summary>
        /// Valida o nome do condomínio
        /// </summary>
        private static void ValidarNome(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("O nome do condomínio é obrigatório.");

            if (nome.Length > 150)
                throw new ArgumentException("O nome do condomínio não pode exceder 150 caracteres.");
        }

        /// <summary>
        /// Valida o CNPJ do condomínio
        /// </summary>
        private void ValidarCnpj(string? cnpj, int condominioId)
        {
            if (string.IsNullOrWhiteSpace(cnpj))
                throw new ArgumentException("O CNPJ do condomínio é obrigatório.");

            string cnpjLimpo = Regex.Replace(cnpj, @"\D", "");

            if (cnpjLimpo.Length != 14)
                throw new ArgumentException("O CNPJ deve conter 14 dígitos numéricos.");

            if (CnpjJaExiste(cnpjLimpo, condominioId))
                throw new ArgumentException("Já existe um condomínio cadastrado com este CNPJ.");
        }

        /// <summary>
        /// Valida a rua do condomínio
        /// </summary>
        private static void ValidarRua(string? rua)
        {
            if (string.IsNullOrWhiteSpace(rua))
                throw new ArgumentException("A rua é obrigatória.");

            if (rua.Length > 150)
                throw new ArgumentException("A rua não pode exceder 150 caracteres.");
        }

        /// <summary>
        /// Valida o número do condomínio
        /// </summary>
        private static void ValidarNumero(string? numero)
        {
            if (string.IsNullOrWhiteSpace(numero))
                throw new ArgumentException("O número é obrigatório.");

            if (numero.Length > 20)
                throw new ArgumentException("O número não pode exceder 20 caracteres.");
        }

        /// <summary>
        /// Valida o bairro do condomínio
        /// </summary>
        private static void ValidarBairro(string? bairro)
        {
            if (string.IsNullOrWhiteSpace(bairro))
                throw new ArgumentException("O bairro é obrigatório.");

            if (bairro.Length > 100)
                throw new ArgumentException("O bairro não pode exceder 100 caracteres.");
        }

        /// <summary>
        /// Valida a cidade do condomínio
        /// </summary>
        private static void ValidarCidade(string? cidade)
        {
            if (string.IsNullOrWhiteSpace(cidade))
                throw new ArgumentException("A cidade é obrigatória.");

            if (cidade.Length > 100)
                throw new ArgumentException("A cidade não pode exceder 100 caracteres.");
        }

        /// <summary>
        /// Valida o UF (Estado) do condomínio
        /// </summary>
        private static void ValidarUf(string? uf)
        {
            if (string.IsNullOrWhiteSpace(uf))
                throw new ArgumentException("O UF (Estado) é obrigatório.");

            if (uf.Length != 2)
                throw new ArgumentException("O UF deve ter 2 caracteres.");
        }

        /// <summary>
        /// Valida o CEP do condomínio
        /// </summary>
        private static void ValidarCep(string? cep)
        {
            if (string.IsNullOrWhiteSpace(cep))
                throw new ArgumentException("O CEP é obrigatório.");

            string cepLimpo = Regex.Replace(cep, @"\D", "");

            if (cepLimpo.Length != 8)
                throw new ArgumentException("O CEP deve conter 8 dígitos numéricos.");
        }

        /// <summary>
        /// Valida o email do condomínio
        /// </summary>
        private static void ValidarEmail(string? email)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                string padrao = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                if (!Regex.IsMatch(email, padrao))
                    throw new ArgumentException("O email do condomínio deve ter um formato válido.");
            }
        }

        /// <summary>
        /// Valida o telefone do condomínio
        /// </summary>
        private static void ValidarTelefone(string? telefone)
        {
            if (!string.IsNullOrWhiteSpace(telefone))
            {
                string telefoneLimpo = Regex.Replace(telefone, @"\D", "");

                if (telefoneLimpo.Length < 10 || telefoneLimpo.Length > 11)
                    throw new ArgumentException("O telefone deve conter entre 10 e 11 dígitos numéricos.");
            }
        }

        /// <summary>
        /// Valida a quantidade de unidades do condomínio
        /// </summary>
        private static void ValidarUnidades(int? unidades)
        {
            if (!unidades.HasValue || unidades <= 0)
                throw new ArgumentException("A quantidade de unidades deve ser maior que zero.");
        }

        /// <summary>
        /// Valida o CNPJ utilizando o algoritmo oficial de verificação
        /// </summary>
        /// <param name="cnpj">CNPJ contendo apenas dígitos</param>
        /// <returns>true se o CNPJ é válido; false caso contrário</returns>
        private static bool CnpjEhValido(string cnpj)
        {
            if (string.IsNullOrEmpty(cnpj) || cnpj.Length != 14)
                return false;

            if (!Regex.IsMatch(cnpj, @"^\d{14}$"))
                return false;

            if (SeqRepetida(cnpj))
                return false;

            int[] mult1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] mult2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            string temp = cnpj.Substring(0, 12);
            int soma = 0;

            for (int i = 0; i < 12; i++)
                soma += int.Parse(temp[i].ToString()) * mult1[i];

            int resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;

            if (resto != int.Parse(cnpj[12].ToString()))
                return false;

            temp = cnpj.Substring(0, 13);
            soma = 0;

            for (int i = 0; i < 13; i++)
                soma += int.Parse(temp[i].ToString()) * mult2[i];

            resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;

            if (resto != int.Parse(cnpj[13].ToString()))
                return false;

            return true;
        }

        /// <summary>
        /// Verifica se o CNPJ é uma sequência repetida
        /// </summary>
        private static bool SeqRepetida(string cnpj)
        {
            return cnpj == new string(cnpj[0], cnpj.Length);
        }

        /// <summary>
        /// Verifica se já existe um condomínio com o mesmo CNPJ no banco
        /// </summary>
        private bool CnpjJaExiste(string cnpj, int condominioId)
        {
            return context.Condominios
                .Where(c => c.Id != condominioId)
                .AsEnumerable()
                .Any(c => Regex.Replace(c.Cnpj ?? "", @"\D", "") == cnpj);
        }
    }
}
