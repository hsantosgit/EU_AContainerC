using EU_CottonSecurity;
using System.Net.Mail;
using System.Net;

namespace EU_CottonContainer.Helpers
{
    public class mailService
    {
        public static void SendMail(string Correo, string Token)
        {
            string to = Correo;
            string from = "noreply2000ac@gmail.com";
            string Password = "xcak rqdy xrrm faan";
            MailMessage message = new MailMessage(from, to);
            message.Subject = "No Reply - Código de Acceso";
            message.Body = "<html><body> Código de Acceso: " + Token + " </body></html>";
            message.IsBodyHtml = true;
            SmtpClient client = new SmtpClient("smtp.gmail.com");
            client.Port = 587;
            client.Credentials = new NetworkCredential(from, Password);
            client.EnableSsl = true;

            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                logClass.WriteLog("Exception caught in SendMail(): {0}" + ex.ToString());
            }
        }
    }
}
