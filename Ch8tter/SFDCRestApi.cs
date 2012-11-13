using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Net;

namespace Ch8tter
{
    public class SFDCRestApi
    {
        private HttpClient httpClient;

        /**
         * Some dummy data used for testing -- probably refactor this to a Unit Test at some point
         ***/
        public void GenerateDummyData()
        {
            ChatterFeedDataSource chatterFeedDataSource = (ChatterFeedDataSource)App.Current.Resources["chatterFeedDataSource"];
            if (chatterFeedDataSource != null)
            {
                ChatterFeedItem feedItem1 = new ChatterFeedItem();
                feedItem1.Id = "sdfkldjskldfsjhkdfshj";
                feedItem1.Title = feedItem1.Content = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam";
                feedItem1.AuthorName = "Barack Obama";
                feedItem1.GroupName = "Mobile";
                feedItem1.CreatedDate = "2012-10-01T16:24:29.000Z";
                chatterFeedDataSource.Items.Add(feedItem1);

                ChatterFeedItem feedItem2 = new ChatterFeedItem();
                feedItem2.Id = "dsfdfsf32sdfsdfdsf";
                feedItem2.Title = feedItem2.Content = "Test Test Test Test";
                feedItem2.AuthorName = "Harry Houdini";
                feedItem2.CreatedDate = "2012-10-01T16:24:29.000Z";
                chatterFeedDataSource.Items.Add(feedItem2);
            }
        }

        /**
         * Issue a GET or POST HTTP request to the SFDC REST API
         **/
        public async Task<JObject> Request(String method, String path)
        {
            //get session instance
            SFDCSession session = SFDCSession.Instance;
            
            //build request
            String request = session.RequestUrl + path;

            return await Request(method, request, "");
        }

        /**
         * Issue a GET or POST HTTP request to the SFDC REST API -- with parameters for POST
         ***/
        public async Task<JObject> Request(String method, String request, String parameters)
        {
            httpClient = new HttpClient();

            //get session instance
            SFDCSession session = SFDCSession.Instance;

            //add access token to request header
            httpClient.DefaultRequestHeaders.Add("Authorization","OAuth " + session.AccessToken);

            Debug.WriteLine("HTTP Request: "+request);

            try
            {
                //send the request, get a response
                HttpResponseMessage response;
                if (method == "POST")
                {
                    HttpContent content = new StringContent(parameters);
                    content.Headers.Remove("Content-Type");
                    content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                    response = await httpClient.PostAsync(request,content);
                }
                else //assume GET if unspecified
                {
                    response = await httpClient.GetAsync(request);
                }
                
                Debug.WriteLine(response.ToString());

                Debug.WriteLine(await response.Content.ReadAsStringAsync());

                return JObject.Parse(await response.Content.ReadAsStringAsync());
            }
            catch (HttpRequestException hre)
            {
                Debug.WriteLine(hre.Message);
                return null;
            }            
        }
    }
}
