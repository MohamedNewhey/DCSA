using DCSA.Database;
using DCSA.Helpers;
using DCSA.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Moyasar;
using Moyasar.Models;
using Moyasar.Services;



namespace DCSA.Controllers
{
    public class HomeController : BaseController
    {
        DefaultConnection db = new DefaultConnection();

        [Route("")]
        [Route("Home/Index")]
        [Route("Home")]
        public ActionResult Index()
        {

            ViewBag.Causes = GlobalHelper.GetCauses();
            ViewBag.Slider = db.Images.ToList();
            return View();
        }
        public ActionResult CauseDetails(int? id)
        {
            if (!id.HasValue)
                return RedirectToAction("Index");

            var Model = db.Causes.FirstOrDefault(x => x.ID == id);
            if (Model == null)
                return RedirectToAction("Index");

            return View(Model);

        }

        [Route("سلة-التبرعات")]
        public ActionResult DonationCart()
        {
            var Cart = Session["Cart"] as List<CartItem>;
            if (Cart == null)
                Cart = new List<CartItem>();
            return View(Cart);
        }

        public ActionResult RefreshCart() {
            var Cart = Session["Cart"] as List<CartItem>;
            if (Cart == null)
                Cart = new List<CartItem>();
            return PartialView("_PartialCart", Cart);
        }

        [HttpPost]
        public ActionResult AddToSession(CartItem model)
        {
            var Cart = Session["Cart"] as List<CartItem>;
            if(Cart==null)
                Cart = new List<CartItem>();
            var ParsedID = int.Parse(model.ID);
            model.Currency = db.Causes.FirstOrDefault(x => x.ID == ParsedID).Currency;
            model.CoverPhoto = db.Causes.FirstOrDefault(x => x.ID == ParsedID).CoverPhoto;
            Cart.Add(model);
            Session.Add("Cart", Cart);

            return Json(new { Count = Cart.Count }, JsonRequestBehavior.AllowGet) ;
        }

        [HttpPost]
        public ActionResult AddFreeDonation(double Amount)
        {
            var Cart = Session["Cart"] as List<CartItem>;
            if (Cart == null)
                Cart = new List<CartItem>();
            CartItem model = new CartItem();
            model.Name = "تبرع سريع";
            Guid GeneratedID = Guid.NewGuid();
            model.ID = GeneratedID.ToString();
            model.Amount = Amount;
            Cart.Add(model);
            Session.Add("Cart", Cart);

            return Json(new { Count = Cart.Count }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteFromCart(string id)
        {
            var Cart = Session["Cart"] as List<CartItem>;
            if (Cart == null)
                Cart = new List<CartItem>();

           var Wanted= Cart.First(x=>x.ID==id);
            Cart.Remove(Wanted);
            Session.Add("Cart", Cart);

            return PartialView("_PartialCart",Cart);
        }

        public ActionResult Thanks(string id, string status, string amount,string message) {

            //MoyasarService.ApiKey = "sk_test_QtHqu8NyBqMZ66ePEYh7599mMv8HMcXqeBHUnGtF";
            MoyasarService.ApiKey = "sk_live_UWXWPyx1qDR49XuVXzSm94sGNs6s6sD6zXNS5SeQ";
            var payment = Payment.Fetch(id);

            if (payment.Status == "paid")
            {
                string PaymentString = Newtonsoft.Json.JsonConvert.SerializeObject(payment);

                PaymentsRequest PR = new PaymentsRequest();
                PR.Status = payment.Status;
                PR.Amount = payment.Amount;
                PR.RequestContent = PaymentString;
                db.PaymentsRequests.Add(PR);


                var Cart = Session["Cart"] as List<CartItem>;
                foreach (var item in Cart)
                {
                    Donation dono = new Donation();
                    dono.Amount = item.Amount;
                    int CauseID;
                    var ParseCheck = int.TryParse(item.ID, out CauseID);
                    if (ParseCheck)
                        dono.CauseID = CauseID;
                    else
                        dono.CauseID = null;

                    dono.PaymentMethod = payment.Source.Type;
                    db.Donations.Add(dono);

                    if (item.GiftID.HasValue)
                    {
                        var gift = db.Gifts.First(x => x.ID == item.GiftID.Value);
                        EmailManager.SendEmail(gift.REmail, gift.DAmount.Value, item.Name, gift.RName);
                    }

                }
                db.SaveChanges();
                Cart = new List<CartItem>();
                Session.Add("Cart", Cart);
            }

            TempData["ThanksModal"] = true;
            return RedirectToAction("DonationCart");
        }


        [Route("نتائج-البحث/{SearchWord?}")]
        public ActionResult SearchResult(string SearchWord)
        {
            List<SearchResultModel> model = new List<SearchResultModel>();
            model.AddRange(db.Causes.Where(x => (x.Header.Contains(SearchWord) || x.CauseContent.Contains(SearchWord)) && x.Publish.Value).Select(x =>
             new SearchResultModel { ID = x.ID, Name = x.Header, TypeID = x.TypeID.Value ,URL = x.URL, date = x.CauseDate.Value, CoverURL = x.CoverPhoto }));

            ViewBag.SearchWord = SearchWord;
            return View(model);
        }

        [HttpPost]
        public ActionResult GetSearchResults(string SearchWord)
        {
            List<SearchResultModel> model = new List<SearchResultModel>();

           model.AddRange(db.Causes.Where(x => (x.Header.Contains(SearchWord) || x.CauseContent.Contains(SearchWord))&&x.Publish.Value).Select(x =>
            new SearchResultModel { ID = x.ID, Name = x.Header, TypeID = x.TypeID.Value, URL = x.URL, date = x.CauseDate.Value,CoverURL=x.CoverPhoto }));

          //  model.AddRange(db.PDFLibraries.Where(x => x.Header.Contains(SearchWord) && x.Publish.Value).Select(x =>
          //new SearchResultModel { ID = x.ID, Name = x.Header, TypeID = x.TypeID.Value, CatName = x.PDFLibraryType.Name, date = x.PdfDate.Value }));


            return PartialView("_GetSearchResults",model);
        }



        [HttpPost]
        public ActionResult SendGift(GiftModel model)
        {
            if (model.REmail == "" || model.REmail == null)
                return null;
            if(model.DAmount==0)
                return null;

            string CauseName = db.Causes.First(x => x.ID == model.CauseID).Header;

            Gift gift = new Gift();
            gift.DMessage = model.DMessage;
            gift.REmail = model.REmail;
            gift.SenderName = model.Sender;
            gift.CauseID = model.CauseID;
            gift.RName = model.Rname;
            gift.DAmount = model.DAmount;
            db.Gifts.Add(gift);

            db.SaveChanges();
            CartItem CI = new CartItem();
            var Cart = Session["Cart"] as List<CartItem>;
            if (Cart == null)
                Cart = new List<CartItem>();
            var ParsedID = model.CauseID;
            CI.ID = model.CauseID.ToString();
            CI.Name = CauseName +"(اهداء)";
            CI.Amount = model.DAmount;
            CI.Currency = db.Causes.FirstOrDefault(x => x.ID == ParsedID).Currency;
            CI.CoverPhoto = db.Causes.FirstOrDefault(x => x.ID == ParsedID).CoverPhoto;
            CI.GiftID = gift.ID;
            Cart.Add(CI);
            Session.Add("Cart", Cart);
          

            return Json(new { Count = Cart.Count }, JsonRequestBehavior.AllowGet);
        }


        [Route("{name}")]
        public ActionResult DisplayStaticContent(string name)
        {
            var model = db.StaticPages.Where(x => x.URL.Contains(name)).FirstOrDefault();

            return View(model);
        }

        [Route("Robots.txt")]
        public ContentResult RobotsText()
        {
            StringBuilder stringBuilder = new StringBuilder();
            var ShareURL = string.Format("{0}://{1}{2}{3}",
            System.Web.HttpContext.Current.Request.Url.Scheme,
            System.Web.HttpContext.Current.Request.Url.Host,
            System.Web.HttpContext.Current.Request.Url.Port == 80 ? string.Empty : ":" + System.Web.HttpContext.Current.Request.Url.Port,
            System.Web.HttpContext.Current.Request.ApplicationPath);

            stringBuilder.AppendLine("user-agent: *");
            stringBuilder.AppendLine("disallow: /error/");
            stringBuilder.AppendLine("disallow: /adminpanel/");
            stringBuilder.Append("sitemap: ");
            stringBuilder.AppendLine(ShareURL + "sitemap.xml");

            return this.Content(stringBuilder.ToString(), "text/plain", Encoding.UTF8);
        }

        [Route("sitemap.xml")]
        public ActionResult SitemapXml()
        {
            var SM = new SitemapNode();
            var sitemapNodes = SM.GetSitemapNodes(this.Url);
            string xml = SM.GetSitemapDocument(sitemapNodes);
            return this.Content(xml, "text/xml", Encoding.UTF8);
        }



    }
}