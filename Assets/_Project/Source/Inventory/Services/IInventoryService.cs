using Coimbra;
using Coimbra.Services;

namespace Project.Core
{
    [RequiredService]
    public interface IInventoryService : IService
    {
        public SerializableDictionary<ItemBaseData, int> InventoryItems { get; }
        
        public void ConsumeItem(ItemSlotView itemSlotView);

        public void RemoveItem(ItemBaseData itemToRemove);

        public void TryAddItem(ItemBaseData itemToAdd);
    }
}
