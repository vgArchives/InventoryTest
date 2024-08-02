using System.Collections.Generic;
using Coimbra.Services;
using Coimbra.Services.Events;
using UnityEngine;

namespace Tatsu.Core
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField] private GameObject _itemSlotPrefab;
        [Space(10)] 
        
        [SerializeField] private List<Transform> _inventorySlotHolders = new ();

        private EventHandle _itemConsumedHandler;
        private IInventoryService _inventoryService;
        
        protected void Start()
        {
            _itemConsumedHandler = ItemConsumedEvent.AddListener(HandleItemConsumedEvent);
            
            _inventoryService = ServiceLocator.GetChecked<IInventoryService>();
            
            InitializeInventory();
        }

        protected void OnDestroy()
        {
            ServiceLocator.GetChecked<IEventService>().RemoveListener(_itemConsumedHandler);
        }

        private void HandleItemConsumedEvent(ref EventContext context, in ItemConsumedEvent e)
        {
            e.ItemSlotView.UpdateQuantity(e.QuantityRemaining);
        }

        private void InitializeInventory()
        {
            int count = Mathf.Min(_inventoryService.InventoryItems.Count, _inventorySlotHolders.Count);
            int i = 0;

            foreach (KeyValuePair<ItemBaseData, int> inventoryItem in _inventoryService.InventoryItems)
            {
                if (i >= count)
                {
                    break;
                }

                InitializeInventoryItem(inventoryItem.Key, inventoryItem.Value, _inventorySlotHolders[i]);
                i++;
            }
        }

        private void InitializeInventoryItem(ItemBaseData itemBaseData, int quantity, Transform slotParent)
        {
            GameObject inventoryItemObject = Instantiate(_itemSlotPrefab, slotParent.transform);
            ItemSlotView itemSlotView = inventoryItemObject.GetComponent<ItemSlotView>();
            itemSlotView.Initialize(itemBaseData, quantity);
        }
    }
}