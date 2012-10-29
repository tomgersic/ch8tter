using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
