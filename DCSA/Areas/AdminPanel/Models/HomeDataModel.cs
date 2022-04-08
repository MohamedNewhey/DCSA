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
            GoverForms = new List<GoverNumber>();
            MissingChildPer = new Percentages();
            FoundChildPer = new Percentages();
            InDangerChildPer = new Percentages();
            SupportFormsPer = new Percentages();
            MissingNoPerMonth = new List<int>();
            InDangerNoPerMonth = new List<int>();
            FoundNoPerMonth = new List<int>();
            SupportNoPerMonth = new List<int>();
        }
        public int NewsNo { get; set; }
        public int InfoNo { get; set; }
        public int PagesNo { get; set; }
        public int InDangerChildNo { get; set; }
        public int SupportFormsNo { get; set; }
        public int MissingChildNo { get; set; }
        public int FoundChildNo { get; set; }
        public int TotalForms { get; set; }
        public List<GoverNumber> GoverForms { get; set; }
        public Percentages MissingChildPer { get; set; }
        public Percentages FoundChildPer { get; set; }
        public Percentages InDangerChildPer { get; set; }
        public Percentages SupportFormsPer { get; set; }
        public List<int> MissingNoPerMonth { get; set; }
        public List<int> InDangerNoPerMonth { get; set; }
        public List<int> FoundNoPerMonth { get; set; }
        public List<int> SupportNoPerMonth { get; set; }
        public int TotalForGover { get; set; }
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