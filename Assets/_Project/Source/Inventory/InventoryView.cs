using System.Collections.Generic;
using UnityEngine;

namespace Tatsu.Core
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField] private List<ItemBaseData> _inventoryItems = new ();
        [Space(10)] 
        
        [SerializeField] private GameObject _itemSlotPrefab;
        [Space(10)] 
        
        [SerializeField] private List<Transform> _inventorySlotHolders = new ();
        
        protected void Start()
        {
            InitializeInventory();
        }

        private void InitializeInventory()
        {
            int count = Mathf.Min(_inventoryItems.Count, _inventorySlotHolders.Count);
        
            for (int i = 0; i < count; i++)
            {
                InitializeInventoryItem(_inventoryItems[i], _inventorySlotHolders[i]);
            }
        }

        private void InitializeInventoryItem(ItemBaseData itemBaseData, Transform slotHolder)
        {
            GameObject inventoryItemObject = Instantiate(_itemSlotPrefab, slotHolder.transform);
            ItemSlotView itemSlotView = inventoryItemObject.GetComponent<ItemSlotView>();
            itemSlotView.Initialize(itemBaseData);
        }
    }
}