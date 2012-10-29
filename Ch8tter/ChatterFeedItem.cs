using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ch8tter
{
    class ChatterFeedItem
    {
        public string Id { get; set; }
        public string AuthorName { get; set; }
        public string Title { get; set; }
        public string GroupName { get; set; }
        public string Content { get; set; }
        public string CreatedDate { get; set; }
        public Uri Link { get; set; }
        public string Image { get; set; }
    }
}
