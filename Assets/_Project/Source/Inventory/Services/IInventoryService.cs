using Coimbra;
using Coimbra.Services;

namespace Tatsu.Core
{
    [RequiredService]
    public interface IInventoryService : IService
    {
        public SerializableDictionary<ItemBaseData, int> InventoryItems { get; }
        
        public void ConsumeItem(ItemSlotView itemSlotView);
    }
}
