using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WoodTransactions
{
    public class PostRequest
    {
        HttpWebRequest _request;

        string _address;

        public Dictionary<string, string> Headers { get; set; }

        public string Response { get; set; }

        public string Accept { get; set; }

        public string Host { get; set; }

        public string Data { get; set; }

        public string ContentType { get; set; }

        public string Referer { get; set; }

        public string UserAgent { get; set; }

        public WebProxy Proxy { get; set; }

        public PostRequest(string address)
        {
            _address = address;
            Headers = new Dictionary<string, string>();
        }

        public void Run(CookieContainer cookieContainer)
        {
            _request = (HttpWebRequest)WebRequest.Create(_address);
            _request.Method = "POST";
            _request.CookieContainer = cookieContainer;
            _request.Proxy = Proxy;
            _request.Accept = Accept;
            _request.Host = Host;
            _request.ContentType = ContentType;
            _request.Referer = Referer;
            _request.UserAgent = UserAgent;

            byte[] sentData = Encoding.UTF8.GetBytes(Data);
            _request.ContentLength = sentData.Length;

            Stream sendStream = _request.GetRequestStream();
            sendStream.Write(sentData, 0, sentData.Length);
            sendStream.Close();

            foreach (var pair in Headers)
            {
                _request.Headers.Add(pair.Key, pair.Value);
            }

            try
            {
                var respons = (HttpWebResponse)_request.GetResponse();
                var stream = respons.GetResponseStream();

                if (stream != null) Response = new StreamReader(stream).ReadToEnd();

                respons.Close();

                Console.WriteLine("Post response read!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error post request! { ex.Message }");
            }
        }
    }
}