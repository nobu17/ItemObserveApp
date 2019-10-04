using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ItemObserveApp.Models.Domain;
using ItemObserveApp.Models.Repository;
using Prism.Mvvm;

namespace ItemObserveApp.Models
{
    public class GroupListModel : BindableBase
    {
        private readonly IGroupRepository _groupRepository;
        public GroupListModel(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public async Task InitModelAsync()
        {
            var groupList = await _groupRepository.GetItemGroupListAsync(UserSetting.UserID, UserSetting.Password);
            ItemGroupList = new ObservableCollection<ItemGroup>(groupList);
        }

        public async Task DeleteGroupAsync(ItemGroup target)
        {
            await _groupRepository.DeleteGroupAsync(target.UserID, target.GroupID);
        }

        private UserSetting _userSetting;
        public UserSetting UserSetting
        {
            get { return _userSetting; }
            set
            {
                SetProperty(ref _userSetting, value);
            }
        }

        private ObservableCollection<ItemGroup> _itemGroupList;
        public ObservableCollection<ItemGroup> ItemGroupList
        {
            get { return _itemGroupList; }
            set
            {
                SetProperty(ref _itemGroupList, value);
            }
        }
    }
}
