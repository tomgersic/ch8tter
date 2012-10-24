using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Diagnostics;

namespace Ch8tter
{
    public class SFDCRestApi
    {
        private HttpClient httpClient;

        public async void Request(String method, String path)
        {
            httpClient = new HttpClient();

            //get session instance
            SFDCSession session = SFDCSession.Instance;

            //add access token to request header
            httpClient.DefaultRequestHeaders.Add("Authorization","OAuth " + session.AccessToken);

            //build request
            String request = session.RequestUrl + path;

            Debug.WriteLine("HTTP Request: "+request);

            try
            {
                //send the request, get a response
                HttpResponseMessage response = await httpClient.GetAsync(request);
                
                Debug.WriteLine(response.ToString());

                Debug.WriteLine(await response.Content.ReadAsStringAsync());
            }
            catch (HttpRequestException hre)
            {
                Debug.WriteLine(hre.Message);
            }
            
        }
    }
}
