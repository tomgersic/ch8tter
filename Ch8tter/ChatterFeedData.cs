using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ch8tter
{
    class ChatterFeedData
    {
        private List<ChatterFeedItem> _Items = new List<ChatterFeedItem>();
        public List<ChatterFeedItem> Items
        {
            get
            {
                return this._Items;
            }
        }
    }
}
