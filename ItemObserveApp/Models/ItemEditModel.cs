using System;
using System.Threading.Tasks;
using ItemObserveApp.Common;
using ItemObserveApp.Models.Domain;
using ItemObserveApp.Models.Repository;
using ItemObserveApp.Models.Validator;

namespace ItemObserveApp.Models
{
    public class ItemEditModel : BaseModel
    {
        private readonly IItemRepository _itemRepository;
        private readonly IValidate<Item> _validator;
        public ItemEditModel(IItemRepository itemRepository, IValidate<Item> validator)
        {
            _itemRepository = itemRepository;
            _validator = validator;
        }

        private Item _editTarget;
        public Item EditTarget
        {
            get { return _editTarget; }
            set
            {
                SetProperty(ref _editTarget, value);
            }
        }

        public async Task CommitAsync()
        {
            await _itemRepository.PutItemAsync(_editTarget);
        }

        public override string Validate()
        {
            return _validator.Validate(EditTarget);
        }
    }
}
