using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ch8tter
{
    class ChatterFeedDataSource
    {
        private ObservableCollection<ChatterFeedItem> _Items = new ObservableCollection<ChatterFeedItem>();
        public ObservableCollection<ChatterFeedItem> Items
        {
            get
            {
                return this._Items;
            }
            set
            {
                this._Items = value;
            }
        }
    }
}
