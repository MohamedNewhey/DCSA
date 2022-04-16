using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCSA.Areas.AdminPanel.Models
{
    public class HomeDataModel
    {
        public HomeDataModel()
        {
        }
        public int DonatersNumber { get; set; }
        public int CausesNumber { get; set; }
        public double TargetMoney{ get; set; }
        public double TargetRemain { get; set; }
        public List<CauseModel> Causes { get; set; }
       
    }

    public class CauseModel { 
    public int  ID { get; set; }
        public string Name { get; set; }    
        public double Target { get; set; }
        public double Remain    { get; set; }
        public double Per { get; set; }
    }
    public class GoverNumber
    {
        public string GoverName { get; set; }
        public int FormsNo { get; set; }
    }

    public class Percentages
    {
        public double Percentage { get; set; }
        public int Num { get; set; }
    }
}