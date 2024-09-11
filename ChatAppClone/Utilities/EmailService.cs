namespace ChatAppClone.Utilities
{
    using ChatAppClone.Utilities.Contracts;
    using MailKit.Net.Smtp;
    using MailKit.Security;
    using MimeKit;
    using MimeKit.Text;

    public class EmailService : IEmailService
    {
        private readonly IConfiguration configuration;

        public EmailService(IConfiguration _configuration)
        {
            this.configuration = _configuration;
        }

        public void Send(string to, string subject, string html, string? from = null)
        {
            MimeMessage email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(from ?? this.configuration["MailKit:FromEmail"]));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = html };

            using SmtpClient smtp = new SmtpClient();
            smtp.Connect(this.configuration["MailKit:Host"], int.Parse(this.configuration["MailKit:Port"]), SecureSocketOptions.StartTls);
            smtp.Authenticate(this.configuration["MailKit:Username"], this.configuration["MailKit:Password"]);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
