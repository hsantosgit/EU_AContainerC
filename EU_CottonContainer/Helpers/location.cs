using System.Net.NetworkInformation;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Web;

namespace EU_CottonContainer.Helpers
{
    internal static class location
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static geoResponse GetGeoCodedResults(string address)
        {
            string url = string.Format(
                    "http://maps.google.com/maps/api/geocode/json?address={0}&region=dk&sensor=false",
                    HttpUtility.UrlEncode(address)
                    );
            var request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(geoResponse));
            var res = (geoResponse)serializer.ReadObject(request.GetResponse().GetResponseStream());
            return res;
        }

        public static string GetMACAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                if (sMacAddress == String.Empty)// only return MAC Address from first card
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                }
            }
            return sMacAddress;
        }
    }
}
