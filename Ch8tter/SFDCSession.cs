using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Security.Authentication.Web;
using Windows.Security.Credentials;

namespace Ch8tter
{
    public sealed class SFDCSession
    {
        const String VAULT_RESOURCE = "ch8tter credentials";
        const string LOCAL_USER = "localUser";

        private static volatile SFDCSession instance;
        private static object syncRoot = new Object();

        public String AccessToken = "";
        public String InstanceUrl = "";
        public String OrgId = "";
        public String UserId = "";
        public String ApiVersion = "v26.0";
        public String BasePath = "services/data";
        private String ConsumerKey = "3MVG9rFJvQRVOvk45r5r6Nef.ZS37y44dr3lvAmkws6ZGGKK1oWgAQFITAkAcpdI3LNd22utrzvV.ObDuSwdB";
        private String RedirectUri = "sfdc://success";
        private PasswordVault vault = new PasswordVault();

        /**
         * Use the Windows 8 PasswordValut to get/save the Refresh Token from encrypted storage
         **/
        private String RefreshToken
        {
            get
            {
                try
                {
                    var creds = vault.FindAllByResource(VAULT_RESOURCE).FirstOrDefault();
                    if (creds != null)
                    {
                        return vault.Retrieve(VAULT_RESOURCE, LOCAL_USER).Password;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception e)
                {
                    // this exception likely means that no credentials have been stored... 
                    return null;
                }
            }
            set
            {
                vault.Add(new PasswordCredential(VAULT_RESOURCE, LOCAL_USER, value));
            }
        }

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
            //if we already have an Access Token, just return that
            if (AccessToken != "")
            {
                return AccessToken;
            }

            //If we have a refresh token available, use the Refresh Token flow to get a new Access Token
            if (RefreshToken != "" && RefreshToken != null)
            {
                AccessToken = await RefreshTokenFlow();
                return AccessToken;
            }

            //prepare the request uri and the callback uri
            Uri requestUri = new Uri("https://login.salesforce.com/services/oauth2/authorize?response_type=token&display=popup&client_id="+ConsumerKey+"&redirect_uri="+WebUtility.UrlEncode(RedirectUri));
            Uri callbackUri = new Uri(RedirectUri);

            //launch the authentication process using WebAuthenticationBroker
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
                PersistData.SetSerializedValue("instance_url", InstanceUrl);
                Debug.WriteLine("Access Token: "+AccessToken);
                return AccessToken;
            }
            else
            {
                return "";
            }
        }

        /**
         * Refresh the Access Token using the Refresh Token Flow
         **/
        public async Task<String> RefreshTokenFlow()
        {
            if (InstanceUrl == null || InstanceUrl == "")
            {
                InstanceUrl = PersistData.GetSerializedStringValue("instance_url");
            }
            //if we don't have a refresh token, we can't do much -- return null
            if (RefreshToken == null || RefreshToken == "")
            {
                return null;
            }
            else
            {
                SFDCRestApi sfdcRestApi = new SFDCRestApi();
                JObject responseObject = await sfdcRestApi.Request("POST", 
                                                                   "https://login.salesforce.com/services/oauth2/token", 
                                                                   string.Format("grant_type=refresh_token&client_id={0}&refresh_token={1}", 
                                                                   ConsumerKey,
                                                                   RefreshToken));
                AccessToken = (string)responseObject["access_token"];
                InstanceUrl = (string)responseObject["instance_url"];
                return AccessToken;
            }
        }
    }
}
