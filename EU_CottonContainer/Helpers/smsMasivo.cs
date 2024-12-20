using System.Net;
using System.Text;
using Twilio;

namespace EU_CottonContainer.Helpers
{
    public class smsMasivo
    {
        public static string SendMessage(string ToMobilePhone, string MessageToSend)
        {
            var request = (HttpWebRequest)WebRequest.Create("https://api.smsmasivos.com.mx/sms/send");
            var postData = "message=" + MessageToSend + "&";
            postData += "numbers=" + ToMobilePhone + "&";
            postData += "country_code=+52";
            var data = Encoding.ASCII.GetBytes(postData);
            //Console.WriteLine(postData.ToString());
            request.Method = "POST";
            request.Headers["apikey"] = "afeaa868c70fb5799e4dd11c1797d4450718cc40";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            return responseString;
        }
    }
}
