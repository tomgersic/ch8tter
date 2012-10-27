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
        private ObservableCollection<ChatterFeedData> _Feeds = new ObservableCollection<ChatterFeedData>();
        public ObservableCollection<ChatterFeedData> Feeds
        {
            get
            {
                return this._Feeds;
            }
        }
    }
}
