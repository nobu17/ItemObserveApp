using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ItemObserveApp.Common;
using ItemObserveApp.Models;
using ItemObserveApp.Models.Domain;
using ItemObserveApp.Models.Repository;
using ItemObserveApp.Models.Validator;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace ItemObserveApp.ViewModels
{
    public class GroupEditPageViewModel : ViewModelBase
    {
        public GroupEditPageViewModel(IGroupRepository groupRepository, IValidate<ItemGroup> validater, INavigationService navigationService, IPageDialogService pageDialogService)
            : base(navigationService, pageDialogService)
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
                return new Command(async () =>
                {
                    await CommitAsync();
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

        private async Task CommitAsync()
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
                catch (UnAuthException)
                {
                    // 認証エラーの場合はログインページへ
                    NavigateAsync(Route.LoginInitPage, new NavigationParameters());
                }
                catch (Exception e)
                {
                    await ShowOKDialog("エラー", "エラーが発生しました。" + e.ToString());
                }
                finally
                {
                    IsLoading = false;
                }
            }
        }

        private void GoBack()
        {
            NavigateGoBackAsync();
        }
    }
}
