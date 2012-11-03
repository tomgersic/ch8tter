using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Security.Authentication.Web;

namespace Ch8tter
{
    public sealed class SFDCSession
    {
        private static volatile SFDCSession instance;
        private static object syncRoot = new Object();

        public String AccessToken = "00Dd0000000dbdj!AR4AQJ9aJNEW03JcyylJ3g.tPhB_T9Df2oxgaWlGdOi0lwuMnUfRDUW1R9Krx2sJY_aST1lD2ecuD_FMfQV3AgKhHEhohvW0";
        public String InstanceUrl = "https://na14.salesforce.com";
        public String ApiVersion = "v26.0";
        public String BasePath = "services/data";
        public String ConsumerKey = "3MVG9rFJvQRVOvk45r5r6Nef.ZS37y44dr3lvAmkws6ZGGKK1oWgAQFITAkAcpdI3LNd22utrzvV.ObDuSwdB";
        public String RedirectUri = "sfdc://success";

        private SFDCSession() { }

        /**
         * Singleton instance constructor
         **/
        public static SFDCSession Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new SFDCSession();
                        }
                    }
                }
                return instance;
            }
        }


        public String RequestUrl
        {
            get
            {
                return InstanceUrl + "/" + BasePath + "/" + ApiVersion + "/";
            }

        }

        public async void oAuthUserAgentFlow()
        {
            Uri requestUri = new Uri("https://login.salesforce.com/services/oauth2/authorize?response_type=token&display=touch&client_id="+ConsumerKey+"&redirect_uri="+WebUtility.UrlEncode(RedirectUri));
            Uri callbackUri = new Uri(RedirectUri);

            WebAuthenticationResult webAuthenticationResult = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, requestUri, callbackUri);
            if (webAuthenticationResult.ResponseStatus == WebAuthenticationStatus.Success)
            {
                string token = webAuthenticationResult.ResponseData;
            }             
        }
    }
}
