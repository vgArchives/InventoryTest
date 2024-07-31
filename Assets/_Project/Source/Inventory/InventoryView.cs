using System.Collections.Generic;
using UnityEngine;

namespace Tatsu.Core
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField] private List<ItemBaseData> _inventoryItems = new ();
        [Space(10)] 
        
        [SerializeField] private List<InventorySlotView> _inventorySlots = new ();
        
        protected void Start()
        { 
            for (int i = 0; i < _inventoryItems.Count; i++)
            {
                _inventorySlots[i].Initialize(_inventoryItems[i]);
            }
        }
    }
}