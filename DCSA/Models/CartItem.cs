using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCSA.Models
{
    public class CartItem
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }  
        
        public string CoverPhoto { get; set; }
        public string Currency { get; set; }
        public int? GiftID { get; set; }
    }

    public class GiftModel
    {
      public string Sender { get; set; }
        public string Rname{ get; set; }
        public string REmail { get; set; }
        public string DMessage { get; set; }
        public int  DAmount { get; set; }

        public int CauseID { get; set; }


    }
}