using System;
using System.Linq;
using HtmlAgilityPack;
using ItemObserveApp.Common;
using ItemObserveApp.Models.Domain;

namespace ItemObserveApp.Models.Repository
{
    public class ItemSurugayaBrowserRepository : IItemBrowserRepository
    {
        public ItemSurugayaBrowserRepository()
        {
        }

        public WebItemInfo GetItemInfo(string url, string title, string bodyhtml)
        {
            var document = new HtmlDocument();
            document.LoadHtml(bodyhtml);

            var item = new WebItemInfo();
            item.StoreType = StoreType.surugaya;
            item.ProductID = GetProductID(url);
            item.ItemName = title;
            item.Price = GetPrice(document);

            return item;
        }

        private string GetProductID(string url)
        {
            return url.Split(new string[] { "/detail/" }, StringSplitOptions.RemoveEmptyEntries)[1].Split(new string[] { "/", "?" }, StringSplitOptions.RemoveEmptyEntries)[0];
        }

        private int GetPrice(HtmlDocument document)
        {
            try
            {
                var data = document.DocumentNode.Descendants("span").FirstOrDefault(e => e.GetAttributeValue("class", "").Contains("mgnL10"));
                if (data != null)
                {
                    var pp = data.InnerText.Replace("円 (税込)", "");
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
            if (url.StartsWith(Urls.SurugayaProductURL, StringComparison.Ordinal))
            {
                return true;
            }
            return false;
        }
    }
}
