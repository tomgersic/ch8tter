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

        public String AccessToken = "00Dd0000000dbdj!AR4AQNsQE0Xg3rsgpOmaeSPDjYWFdAHejSRy2xw6uGlwxlEu9ZzLxdxiOz3yMkpjSeKIV9Z3Ur4Dxgv1AIK24rjtiAjkO4tL";
        public String InstanceUrl = "https://na14.salesforce.com/";
        public String ApiVersion = "v26.0";      

        private SFDCSession() { }

        public static SFDCSession Instance
        {
            get
            {
                if (instance == null)
                {
                    lock(syncRoot)
                    {
                        if(instance == null)
                        {
                            instance = new SFDCSession();
                        }
                }
                return instance;
            }
        }
    }
}
