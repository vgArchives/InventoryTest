using Coimbra;
using Coimbra.Services;
using UnityEngine;

namespace Tatsu.Core
{
    public class InventorySystem : Actor, IInventoryService
    {
        [Space(10)]
        [SerializeField] private SerializableDictionary<ItemBaseData, int> _inventoryItems = new ();
        
        private InventorySystem() { }

        private const int DefaultItemQuantity = 1;
        private const int PoisonItemId = 3;
        
        private IPlayerAnimationsService _playerAnimationsService;
        private IPlayerStatsService _playerStatsService;

        public SerializableDictionary<ItemBaseData, int> InventoryItems => _inventoryItems;
        
        public void ConsumeItem(ItemSlotView itemSlotView)
        {
            if (!_playerStatsService.IsAlive)
            {
                return;
            }
            
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

            _playerAnimationsService.PlayTriggerAnimation(itemBaseData.ItemId == PoisonItemId
                ? AnimationType.TakeDamage
                : AnimationType.PointAt);

            if (itemQuantity > 0)
            {
                _inventoryItems[itemBaseData] = itemQuantity;
            }
            else
            {
                RemoveItem(itemBaseData);
            }
        }

        public void RemoveItem(ItemBaseData itemToRemove)
        {
            _inventoryItems.Remove(itemToRemove);
        }

        public void TryAddItem(ItemBaseData itemToAdd)
        {
            if (!_inventoryItems.ContainsKey(itemToAdd))
            {
                _inventoryItems.Add(itemToAdd, DefaultItemQuantity);
            }
        }
        
        protected override void OnInitialize()
        {
            base.OnInitialize();

            _playerAnimationsService = ServiceLocator.GetChecked<IPlayerAnimationsService>();
            _playerStatsService = ServiceLocator.GetChecked<IPlayerStatsService>();
        }
    }
}