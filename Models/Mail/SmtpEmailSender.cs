using System.Net.Mail;
using System.Net;

namespace IdentityApp.Models.Mail
{
    public class SmtpEmailSender : IEmailSender
    {
        private string _host;
        private int _port;
        private bool _enableSsl;
        private string _username;
        private string _password;

        public SmtpEmailSender(string host, int port, bool enableSsl, string username, string password)
        {
            _host = host;
            _port = port;
            _enableSsl = enableSsl;
            _username = username;
            _password = password;
        }

        public Task SendEmailAsync(string email, string subject, string callbackUrl)
        {
            var client = new SmtpClient(_host, _port)
            {
                Credentials = new NetworkCredential(_username, _password),
                EnableSsl = _enableSsl
            };

            var htmlMessage = $@"
    <!DOCTYPE html>
    <html lang='tr'>
    <head>
        <meta charset='UTF-8'>
        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
        <title>Kayıt Onayı</title>
        <style>
            body {{
                font-family: Arial, sans-serif;
                background-color: #f4f4f4;
                color: #333;
                margin: 0;
                padding: 0;
            }}
            .container {{
                max-width: 600px;
                margin: 0 auto;
                background-color: #ffffff;
                padding: 20px;
                border-radius: 10px;
                box-shadow: 0 4px 8px rgba(0,0,0,0.1);
            }}
            .header {{
                text-align: center;
                padding: 20px 0;
                background-color: #333;
                color: #fff;
                border-radius: 10px 10px 0 0;
            }}
            .header h1 {{
                margin: 0;
                font-size: 24px;
            }}
            .content {{
                padding: 20px;
                text-align: center;
            }}
            .content p {{
                font-size: 16px;
                line-height: 1.5;
                color: #555;
            }}
            .btn {{
                display: inline-block;
                background-color: #007bff;
                color: #fff;
                padding: 12px 20px;
                border-radius: 5px;
                text-decoration: none;
                font-size: 16px;
            }}
            .btn:hover {{
                background-color: #0056b3;
            }}
            .footer {{
                text-align: center;
                padding: 10px;
                font-size: 12px;
                color: #777;
            }}
        </style>
    </head>
    <body>
        <div class='container'>
            <div class='header'>
                <h1>Kayıt Başarılı!</h1>
            </div>
            <div class='content'>
                <p>Merhaba,</p>
                <p>Kayıt işleminiz başarıyla tamamlanmıştır. Kayıt olduğunuz için teşekkür ederiz!</p>
                <p>Lütfen hesabınızı doğrulamak için aşağıdaki butona tıklayın:</p>
                <a href='{callbackUrl}' class='btn'>Hesabını Doğrula</a>
            </div>
            <div class='footer'>
                <p>Bu e-postayı almanızın nedeni sistemimize kayıt olmanızdır.</p>
                <p>Herhangi bir sorunuz varsa, lütfen bizimle iletişime geçin.</p>
            </div>
        </div>
    </body>
    </html>";

            var mailMessage = new MailMessage(_username ?? "", email, subject, htmlMessage)
            {
                IsBodyHtml = true
            };

            return client.SendMailAsync(mailMessage);
        }

    }
}
