using Coimbra;
using UnityEngine;

namespace Tatsu.Core
{
    public class InventorySystem : Actor, IInventoryService
    {
        [SerializeField] private SerializableDictionary<ItemBaseData, int> _inventoryItems = new ();
        
        private InventorySystem() { }

        public SerializableDictionary<ItemBaseData, int> InventoryItems => _inventoryItems;
        
        public void ConsumeItem(ItemSlotView itemSlotView)
        {
            ItemBaseData itemBaseData = itemSlotView.ItemBaseData;

            if (!_inventoryItems.TryGetValue(itemBaseData, out int itemQuantity))
            {
                return;
            }

            if (itemBaseData is not ConsumableItemData consumableItem)
            {
                return;
            }

            consumableItem.ConsumeItem();

            itemQuantity--;

            new ItemConsumedEvent(itemSlotView, itemQuantity).Invoke(this);

            if (itemQuantity > 0)
            {
                _inventoryItems[itemBaseData] = itemQuantity;
            }
            else
            {
                _inventoryItems.Remove(itemBaseData);
            }
        }
    }
}