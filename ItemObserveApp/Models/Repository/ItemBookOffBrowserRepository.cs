using System;
using System.Linq;
using HtmlAgilityPack;
using ItemObserveApp.Common;
using ItemObserveApp.Models.Domain;

namespace ItemObserveApp.Models.Repository
{
    public class ItemBookOffBrowserRepository : IItemBrowserRepository
    {
        public ItemBookOffBrowserRepository()
        {
        }

        public WebItemInfo GetItemInfo(string url, string title, string bodyhtml)
        {
            var document = new HtmlDocument();
            document.LoadHtml(bodyhtml);

            var item = new WebItemInfo();
            item.StoreType = StoreType.bookoff;
            item.ProductID = GetProductID(url);
            item.ItemName = title;
            item.Price = GetPrice(document);

            return item;
        }

        private string GetProductID(string url)
        {
            return url.Split(new string[] { "/old/" }, StringSplitOptions.RemoveEmptyEntries)[1].Split(new string[] { "/", "?" }, StringSplitOptions.RemoveEmptyEntries)[0];
        }

        private int GetPrice(HtmlDocument document)
        {
            try
            {
                var data = document.DocumentNode.Descendants("td").FirstOrDefault(e => e.GetAttributeValue("class", "").Contains("oldprice"));
                if (data != null)
                {
                    var pp = data.InnerText.Replace("￥", "").Replace(",", "");
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
            if (url.StartsWith(Urls.BookOffProductURL, StringComparison.Ordinal))
            {
                return true;
            }
            return false;
        }
    }
}
