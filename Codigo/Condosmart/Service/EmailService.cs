using Core.Service;
using Core.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Service
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _settings;

        public EmailService(IOptions<SmtpSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendAsync(string toEmail, string toName, string subject, string htmlBody)
        {
            if (!_settings.Enabled)
                throw new InvalidOperationException("O envio de e-mail nao esta habilitado nas configuracoes SMTP.");

            if (string.IsNullOrWhiteSpace(_settings.Host) ||
                string.IsNullOrWhiteSpace(_settings.SenderEmail) ||
                string.IsNullOrWhiteSpace(_settings.Username) ||
                string.IsNullOrWhiteSpace(_settings.Password))
            {
                throw new InvalidOperationException("As configuracoes SMTP estao incompletas.");
            }

            var mensagem = new MimeMessage();
            mensagem.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
            mensagem.To.Add(new MailboxAddress(toName, toEmail));
            mensagem.Subject = subject;
            mensagem.Body = new BodyBuilder
            {
                HtmlBody = htmlBody
            }.ToMessageBody();

            using var client = new SmtpClient();
            var socketOptions = _settings.UseSsl ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.StartTlsWhenAvailable;

            await client.ConnectAsync(_settings.Host, _settings.Port, socketOptions);
            await client.AuthenticateAsync(_settings.Username, _settings.Password);
            await client.SendAsync(mensagem);
            await client.DisconnectAsync(true);
        }
    }
}
