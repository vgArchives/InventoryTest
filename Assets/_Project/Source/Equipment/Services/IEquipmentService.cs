using Coimbra;
using Coimbra.Services;

namespace Project.Core
{
    [RequiredService]
    public interface IEquipmentService : IService
    {
        public SerializableDictionary<ItemType, EquipmentItemData> PlayerEquippedItems { get; }

        public void EquipItem(EquipmentItemData equipmentData);
        
        public void UnequipItem(ItemType equipmentType);
    }
}
