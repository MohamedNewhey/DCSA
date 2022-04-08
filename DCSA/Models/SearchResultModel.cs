using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCSA.Models
{
    public class SearchResultModel
    {
        public int ID { get; set; }

        public string Name { get; set; }
        public string URL { get; set; }
        public int TypeID { get; set; }

        public string CoverURL { get; set; }
        public string CatName { get; set; }
        public DateTime date { get; set; }
    }
}