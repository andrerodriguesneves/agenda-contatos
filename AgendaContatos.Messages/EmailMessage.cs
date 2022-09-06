using System.Net;
using System.Net.Mail;

namespace AgendaContatos.Messages
{
    /// <summary>
    /// Classe para operações de envio de email
    /// </summary>
    public class EmailMessage
    {
        #region Atributos

        private string _conta = "cotinaoresponda@outlook.com";
        private string _senha = "@Admin123456";
        private string _smtp = "smtp-mail.outlook.com";
        private int _porta = 587;

        #endregion

        #region Métodos

        public void SendMail(string mailTo, string subject, string body)
        {
            var mailMessage = new MailMessage(_conta, mailTo);
            mailMessage.Subject = subject;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = true;

            var smtpClient = new SmtpClient(_smtp, _porta);
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential(_conta, _senha);
            smtpClient.Send(mailMessage);
        }

        #endregion
    }
}