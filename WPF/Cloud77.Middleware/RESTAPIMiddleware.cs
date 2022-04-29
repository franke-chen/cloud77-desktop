using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Cloud77.Middleware
{
    public class RESTAPIMiddleware
    {
        public string GetAPIKey(string host)
        {
            string url = $"{host}/login-api/info/apikey";
            HttpWebRequest req = HttpWebRequest.Create(url) as HttpWebRequest;
            req.Timeout = 5000;
            HttpWebResponse response = null;
            string key = "";
            try
            {
                response = req.GetResponse() as HttpWebResponse;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    StreamReader sr = new StreamReader(response.GetResponseStream());
                    string json = sr.ReadToEnd().ToString();
                    sr.Close();
                    var jo = JObject.Parse(json);
                    key = jo.GetValue("apikey").ToString();
                }
            }
            catch
            {
                key = "not get api key from service";
            }
            finally
            {
                if (response != null) response.Close();
            }
            return key;
        }
    }
}
