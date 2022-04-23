using DCSA.Database;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace DCSA.Helpers
{
    public class SitemapNode
    {
        public SitemapFrequency? Frequency { get; set; }
        public DateTime? LastModified { get; set; }
        public double? Priority { get; set; }
        public string Url { get; set; }

        public string GetSitemapDocument(IEnumerable<SitemapNode> sitemapNodes)
        {
            XNamespace xmlns = "http://www.sitemaps.org/schemas/sitemap/0.9";
            XElement root = new XElement(xmlns + "urlset");

            foreach (SitemapNode sitemapNode in sitemapNodes)
            {
                XElement urlElement = new XElement(
                    xmlns + "url",
                    new XElement(xmlns + "loc", Uri.EscapeUriString(sitemapNode.Url)),
                    sitemapNode.LastModified == null ? null : new XElement(
                        xmlns + "lastmod",
                        sitemapNode.LastModified.Value.ToLocalTime().ToString("yyyy-MM-ddTHH:mm:sszzz")),
                    sitemapNode.Frequency == null ? null : new XElement(
                        xmlns + "changefreq",
                        sitemapNode.Frequency.Value.ToString().ToLowerInvariant()),
                    sitemapNode.Priority == null ? null : new XElement(
                        xmlns + "priority",
                        sitemapNode.Priority.Value.ToString("F1", CultureInfo.InvariantCulture)));
                root.Add(urlElement);
            }

            XDocument document = new XDocument(root);
            return document.ToString();
        }

        public IReadOnlyCollection<SitemapNode> GetSitemapNodes(UrlHelper urlHelper)
        {
            List<SitemapNode> nodes = new List<SitemapNode>();
            var ShareURL = string.Format("{0}://{1}{2}{3}",
            System.Web.HttpContext.Current.Request.Url.Scheme,
            System.Web.HttpContext.Current.Request.Url.Host,
            System.Web.HttpContext.Current.Request.Url.Port == 80 ? string.Empty : ":" + System.Web.HttpContext.Current.Request.Url.Port,
            System.Web.HttpContext.Current.Request.ApplicationPath);
            nodes.Add(
                new SitemapNode()
                {
                    Url = ShareURL,
                    Priority = 1
                });

            DefaultConnection db = new DefaultConnection();

            foreach (var item in db.Causes.ToList())
                nodes.Add(new SitemapNode()
                {
                    Url = ShareURL + "Home/CauseDetails/"+item.ID,
                    Frequency = SitemapFrequency.Weekly,
                    Priority = 1
                });

            foreach (var item in db.StaticPages.ToList())
                nodes.Add(new SitemapNode()
                {
                    Url = ShareURL + item.URL,
                    Frequency = SitemapFrequency.Weekly,
                    Priority = 0.8
                });



            return nodes;
        }
    }

    public enum SitemapFrequency
    {
        Never,
        Yearly,
        Monthly,
        Weekly,
        Daily,
        Hourly,
        Always
    }


    public static class UrlHelperExtensions
    {
        public static string AbsoluteRouteUrl(
            this UrlHelper urlHelper,
            string routeName,
            object routeValues = null)
        {
            string scheme = urlHelper.RequestContext.HttpContext.Request.Url.Scheme;
            return urlHelper.RouteUrl(routeName, routeValues, scheme);
        }
    }
}