using System.Net.Mail;
using System.Net;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace EU_CottonContainer.Helpers
{
    public class smsService
    {
        private static smsSetting _smsSettings = new smsSetting();


        public static string SendMessage(string ToMobilePhone, string MessageToSend)
        {
            TwilioClient.Init(_smsSettings.Twilio_Account_SID, _smsSettings.Twilio_Auth_Token);

            var Message = MessageResource.Create(from: new Twilio.Types.PhoneNumber(_smsSettings.Twilio_Phone_Number),
                to: new Twilio.Types.PhoneNumber("+52" + ToMobilePhone),
                body: MessageToSend);

            return Message.Sid;
        }


        public static void SendEmail(string To, string Message)
        {
            //Mail Settings
            MailMessage mailMsg = new MailMessage("mypalmbook@gmail.com", To);//the verified sender email that you have registered in my account

            //Message Settings
            mailMsg.Subject = "Cotton Authentication Code";    //any default text or data from our textbox
            mailMsg.Body = Message;      //any default text or data from our textbox

            //SMTP Settings
            SmtpClient smtpClient = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));
            //the mail id and password that we have register to log in the sendgrid 
            NetworkCredential credentials = new NetworkCredential("registeremail@gmail.com", "yourpassword");
            smtpClient.Credentials = credentials;
            if (mailMsg != null)
            {
                smtpClient.Send(mailMsg);
            }
        }
    }
}
