namespace ChatAppClone.Utilities
{
    using MimeKit;
    using MimeKit.Text;
    using MailKit.Net.Smtp;
    using MailKit.Security;
    
    using ChatAppClone.Utilities.Contracts;

    public class EmailService : IEmailService
    {
        private readonly IConfiguration configuration;

        public EmailService(IConfiguration _configuration)
        {
            this.configuration = _configuration;
        }

        public async Task SendAsync(string to, string subject, string html, string? from = null)
        {
            MimeMessage email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(from ?? this.configuration["MailKit:FromEmail"]));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = html };

            using SmtpClient smtp = new SmtpClient();
            await smtp.ConnectAsync(this.configuration["MailKit:Host"], int.Parse(this.configuration["MailKit:Port"]), SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(this.configuration["MailKit:Username"], this.configuration["MailKit:Password"]);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
            smtp.Dispose();  
        }
    }
}
