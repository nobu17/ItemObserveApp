using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ItemObserveApp.Common;
using ItemObserveApp.Models.Domain;
using ItemObserveApp.Models.Repository;
using ItemObserveApp.Models.Validator;

namespace ItemObserveApp.Models
{
    public class GroupEditModel : BaseModel
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IValidate<ItemGroup> _validator;
        public GroupEditModel(IGroupRepository groupRepository, IValidate<ItemGroup> validater)
        {
            _groupRepository = groupRepository;
            _validator = validater;
        }

        private ItemGroup _editTarget;
        public ItemGroup EditTarget
        {
            get { return _editTarget; }
            set
            {
                SetProperty(ref _editTarget, value);
            }
        }

        public async Task CommitAsync()
        {
            await _groupRepository.PutGroupAsync(EditTarget);
        }

        public override string Validate()
        {
            return _validator.Validate(EditTarget);
        }
    }
}
