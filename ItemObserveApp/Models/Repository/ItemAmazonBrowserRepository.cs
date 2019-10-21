using System;
using System.Linq;
using HtmlAgilityPack;
using ItemObserveApp.Common;
using ItemObserveApp.Models.Domain;

namespace ItemObserveApp.Models.Repository
{
    public class ItemAmazonBrowserRepository : IItemBrowserRepository
    {
        public ItemAmazonBrowserRepository()
        {
        }

        public WebItemInfo GetItemInfo(string url, string title, string bodyhtml)
        {
            var document = new HtmlDocument();
            document.LoadHtml(bodyhtml);

            var item = new WebItemInfo();
            item.ProductID = GetProductID(url);
            item.ItemName = GetItemTitle(title);
            item.Price = GetPrice(document);

            return item;
        }

        private string GetItemTitle(string title)
        {
            return title.Replace("Amazon.co.jp: こちらもどうぞ:", "").Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
        }

        private string GetProductID(string url)
        {
            return url.Split(new string[] { "?" }, StringSplitOptions.RemoveEmptyEntries)[0].Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
        }

        private int GetPrice(HtmlDocument document)
        {
            try
            {
                var data = document.DocumentNode.Descendants("div").FirstOrDefault(e => e.GetAttributeValue("class", "").Contains("olp-link-widget"));

                //var data = document.DocumentNode.Descendants("span").FirstOrDefault(e => e.GetAttributeValue("class", "").Contains("olpOfferPrice"));
                if (data != null)
                {
                    var pp = data.InnerText.Split(new string[] { "¥", "￥" }, StringSplitOptions.RemoveEmptyEntries)[1].Replace(",", "").Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)[0];
                    var pr = int.Parse(pp.Trim());
                    return pr;
                }
            }
            catch (Exception)
            {
                return -1;
            }

            return -1;
        }

        public bool IsComitable(string url)
        {
            if (url.Contains(Urls.AmazonProductURL))
            {
                return true;
            }
            return false;
        }
    }
}
