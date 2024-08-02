using Coimbra;
using Coimbra.Services;

namespace Tatsu.Core
{
    [RequiredService]
    public interface IEquipmentService : IService
    {
        public SerializableDictionary<ItemType, EquipmentItemData> PlayerEquippedItems { get; }

        public void EquipItem(EquipmentItemData itemBaseData);
        
        public void UnequipItem(ItemType itemType);
    }
}
