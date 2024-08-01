using Coimbra;
using Coimbra.Services;
using Coimbra.Services.Events;
using UnityEngine;

namespace Tatsu.Core
{
    public class PlayerEquipmentSystem : MonoBehaviour
    {
        [SerializeField] private SerializableDictionary<ItemType, EquipmentItemData> _playerEquippedItems = new();
        
        private EventHandle _itemEquippedHandler;
        private EventHandle _itemMovedHandler;

        protected void Start()
        {
            _itemEquippedHandler = ItemEquippedEvent.AddListener(HandleItemEquippedEvent);
            _itemEquippedHandler = ItemUnequippedEvent.AddListener(HandleItemUnequippedEvent);
        }

        protected void OnDestroy()
        {
            ServiceLocator.GetChecked<IEventService>().RemoveListener(_itemEquippedHandler);
        }

        private void HandleItemEquippedEvent(ref EventContext context, in ItemEquippedEvent e)
        {
            ItemType itemType = e.ItemBaseData.ItemType;
            
            if (!_playerEquippedItems.ContainsKey(itemType))
            {
                return;
            }

            if (_playerEquippedItems.TryGetValue(itemType, out EquipmentItemData equipmentItemData))
            {
                if (equipmentItemData != null)
                {
                    equipmentItemData.RemoveStats();
                }
            }
            
            _playerEquippedItems[itemType] = e.ItemBaseData;
            e.ItemBaseData.AddStats();
        }

        private void HandleItemUnequippedEvent(ref EventContext context, in ItemUnequippedEvent e)
        {
            if (!_playerEquippedItems.TryGetValue(e.ItemType, out EquipmentItemData currentEquipmentData))
            {
                return;
            }

            if (currentEquipmentData == null)
            {
                return;
            }
            
            currentEquipmentData.RemoveStats();
            _playerEquippedItems[e.ItemType] = null;
        }
    }
}