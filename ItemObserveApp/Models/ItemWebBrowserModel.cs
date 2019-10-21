using System;
using ItemObserveApp.Common;
using ItemObserveApp.Models.Domain;
using ItemObserveApp.Models.Factory;
using ItemObserveApp.Models.Repository;

namespace ItemObserveApp.Models
{
    public class ItemWebBrowserModel : BaseModel
    {
        private IItemBrowserRepository _itemRepository;
        private readonly IItemBrowserFactory _itemBrowserFactory;

        public ItemWebBrowserModel(IItemBrowserFactory itemBrowserFactory)
        {
            _itemBrowserFactory = itemBrowserFactory;
        }

        public void InitFromUrl(string url)
        {
            _itemRepository = _itemBrowserFactory.GetRepository(url);
            if (_itemRepository == null)
            {
                throw new ArgumentException("repository is null");
            }
        }

        public void ChangeCommitable(string url)
        {
            IsCommitable = _itemRepository.IsComitable(url);
        }

        public WebItemInfo GetItem(string url, string title, string html)
        {
            return _itemRepository.GetItemInfo(url, title, html);
        }

        private bool _isCommitable = false;
        public bool IsCommitable
        {
            get { return _isCommitable; }
            set
            {
                SetProperty(ref _isCommitable, value);
            }
        }
    }
}
