namespace Core.Settings
{
    public class SmtpSettings
    {
        public const string SectionName = "Smtp";

        public bool Enabled { get; set; }

        public string Host { get; set; } = string.Empty;

        public int Port { get; set; } = 587;

        public bool UseSsl { get; set; }

        public string SenderName { get; set; } = "CondoSmart";

        public string SenderEmail { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}
