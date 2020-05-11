using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace TestShoppingMall.Dac
{
    public class BaseDac
    {
        
        private const string LOCAL_API_HOST = "devapi.sm.com";
        private const string AZURE_API_HOST = "devsmapi.azurewebsites.net";

        private HttpContext _webContext = null;
        public HttpContext WebContext
        {
            get
            {
                return _webContext;
            }
            set
            {
                _webContext = value;
            }
        }

        private string ApiHost
        {
            get
            {
                string host = _webContext.Request.Host.Host;
                string apiHost = AZURE_API_HOST;
                if (host.Contains("localhost")) apiHost = LOCAL_API_HOST;
                return apiHost;
            }
        }


        public string LinkageToApi(string uri, string method, Dictionary<string, string> param)
        {
            string jsonStr = string.Empty;
            string strParams = MakeSerialParam(param);
            if (!uri.StartsWith("/")) uri = "/" + uri;
            uri = string.Format("http://{0}{1}", ApiHost, uri);
            HttpWebRequest req = null;
            if (method.ToLower().Equals("get"))
            {
                if (param.Count > 0)
                    uri = uri + "?" + strParams;
                req = (HttpWebRequest)HttpWebRequest.Create(uri);
            }
            else if (method.ToLower().Equals("post"))
            {
                req = (HttpWebRequest)HttpWebRequest.Create(uri);
                byte[] sendData = UTF8Encoding.UTF8.GetBytes(strParams);
                req.ContentLength = sendData.Length;
                using (Stream strm = req.GetRequestStream())
                {
                    strm.Write(sendData, 0, sendData.Length);
                }
            }
            req.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            req.Method = method;

            using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
            {
                using (StreamReader reader = new StreamReader(resp.GetResponseStream(), Encoding.GetEncoding("UTF-8")))
                {
                    jsonStr = reader.ReadToEnd();
                }
            }

            return jsonStr;
        }

        public string MakeSerialParam(Dictionary<string, string> dict)
        {
            string strRtn = string.Join("&", dict.Cast<KeyValuePair<string, string>>()
                            .Select(kv => string.Format("{0}={1}", kv.Key, kv.Value))
                            .ToArray());
            return strRtn;
        }
    }
}
