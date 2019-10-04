using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ItemObserveApp.Models;
using ItemObserveApp.Models.Domain;
using ItemObserveApp.Models.Repository;
using ItemObserveApp.Models.Validator;
using Prism.Navigation;
using Xamarin.Forms;

namespace ItemObserveApp.ViewModels
{
    public class GroupEditPageViewModel : ViewModelBase
    {
        public GroupEditPageViewModel(IGroupRepository groupRepository, IValidate<ItemGroup> validater, INavigationService navigationService)
            : base(navigationService)
        {
            _model = new GroupEditModel(groupRepository, validater);
        }

        private GroupEditModel _model;
        public GroupEditModel Model
        {
            get { return _model; }
            set
            {
                SetProperty(ref _model, value);
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters != null)
            {
                Model.EditTarget = parameters["ItemGroup"] as ItemGroup;
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

        private async Task Commit()
        {
            ErrorMessage = _model.Validate();
            if (ErrorMessage == string.Empty)
            {
                try
                {
                    IsLoading = true;
                    await _model.CommitAsync();
                    GoBack();
                }
                finally
                {
                    IsLoading = false;
                }
            }
        }

        private void GoBack()
        {
            NavigateAsync("GroupListPage", null);
        }
    }
}
