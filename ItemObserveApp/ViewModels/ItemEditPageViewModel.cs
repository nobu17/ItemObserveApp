using System;
using System.Windows.Input;
using ItemObserveApp.Models;
using ItemObserveApp.Models.Domain;
using ItemObserveApp.Models.Repository;
using ItemObserveApp.Models.Validator;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace ItemObserveApp.ViewModels
{
    public class ItemEditViewModel : ViewModelBase
    {
        public ItemEditViewModel(IItemRepository itemRepository, IValidate<Item> validater, INavigationService navigationService, IPageDialogService pageDialogService)
            : base(navigationService, pageDialogService)
        {
            _model = new ItemEditModel(itemRepository, validater);
        }

        private ItemEditModel _model;
        public ItemEditModel Model
        {
            get { return _model; }
            set
            {
                SetProperty(ref _model, value);
            }
        }

        public ICommand CommitCommand
        {
            get
            {
                return new Command(() =>
                {
                    Commit();
                });
            }
        }
        public ICommand CancelCommand
        {
            get
            {
                return new Command(() =>
                {
                    GoBack();
                });
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters != null)
            {
                _model.EditTarget = parameters["Item"] as Item;
            }
        }

        private void Commit()
        {

        }

        private void GoBack()
        {
            NavigateAsync("GroupListPage", null);
        }
    }

    public class ItemEditViewModelTransitParam
    {
        public Item EditItem { get; set; }
    }
}
