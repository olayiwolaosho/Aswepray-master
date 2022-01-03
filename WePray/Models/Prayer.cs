using System;
using System.Collections.Generic;
using System.Text;

namespace WePray.Models
{
    public class Prayer 
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Date { get; set; }
        public string strDate { get; set; }
        public string image { get; set; }
    }
}
