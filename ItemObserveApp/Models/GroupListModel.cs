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
        private readonly IUserRepository _userRepository;
        public GroupListModel(IUserRepository userRepository, IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
            _userRepository = userRepository;
        }

        public async Task InitModelAsync()
        {
            _userSetting = await _userRepository.GetUserSettingAsync();
            if (_userSetting == null)
            {
                throw new Exception("UserSetting is null");
            }
            var groupList = await _groupRepository.GetItemGroupListAsync(_userSetting.UserID, _userSetting.Password);
            ItemGroupList = new ObservableCollection<ItemGroup>(groupList);
        }

        public async Task DeleteGroupAsync(ItemGroup target)
        {
            await _groupRepository.DeleteGroupAsync(target.UserID, target.GroupID);
        }

        public ItemGroup GetNewItemGroup()
        {
            var group = new ItemGroup();
            group.UserID = _userSetting.UserID;
            return group;
        }

        private UserSetting _userSetting;

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
