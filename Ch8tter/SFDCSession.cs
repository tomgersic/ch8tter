using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        //public String AccessToken = "00D000000000062!AQsAQLn.YF3jqk8nMKOliG5YG4188_ag1p1V1yAWTXpLsQvCX2pHeWbTTPdsGX.lVzDK6GMBko6p.qF8rbbmLh6yLuMTxV85";
        //public String InstanceUrl = "https://na1.salesforce.com";
        public String AccessToken = "";
        public String InstanceUrl = "";
        public String OrgId = "";
        public String UserId = "";
        public String ApiVersion = "v26.0";
        public String BasePath = "services/data";
        private String RefreshToken = "";
        private String ConsumerKey = "3MVG9rFJvQRVOvk45r5r6Nef.ZS37y44dr3lvAmkws6ZGGKK1oWgAQFITAkAcpdI3LNd22utrzvV.ObDuSwdB";
        private String RedirectUri = "sfdc://success";

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

        /**
         * return the root request url for all REST API requests
         * typically something along the lines of https://na1.salesforce.com/services/data/v26.0/
         **/
        public String RequestUrl
        {
            get
            {
                return InstanceUrl + "/" + BasePath + "/" + ApiVersion + "/";
            }

        }

        /**
         * Launch the oAuth User Agent login dialog
         **/
        public async Task<String> oAuthUserAgentFlow()
        {
            if (AccessToken != "")
            {
                return AccessToken;
            }

            Uri requestUri = new Uri("https://login.salesforce.com/services/oauth2/authorize?response_type=token&display=touch&client_id="+ConsumerKey+"&redirect_uri="+WebUtility.UrlEncode(RedirectUri));
            Uri callbackUri = new Uri(RedirectUri);

            WebAuthenticationResult webAuthenticationResult = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, requestUri, callbackUri);
            if (webAuthenticationResult.ResponseStatus == WebAuthenticationStatus.Success)
            {
                //parse the response Uri to get access token, etc.
                Uri responseUri = new Uri(webAuthenticationResult.ResponseData.ToString());
                //just treat it as a query string so we can use WwwFormUrlDecoder to parse it
                WwwFormUrlDecoder decoder = new WwwFormUrlDecoder(responseUri.Fragment.Replace("#", "?"));
                AccessToken = decoder.GetFirstValueByName("access_token");
                RefreshToken = decoder.GetFirstValueByName("refresh_token");
                InstanceUrl = WebUtility.UrlDecode(decoder.GetFirstValueByName("instance_url"));
                Debug.WriteLine("Access Token: "+AccessToken);
                return AccessToken;
            }
            else
            {
                return "";
            }
        }
    }
}
