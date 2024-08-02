using Coimbra;
using Coimbra.Services;
using Coimbra.Services.Events;
using UnityEngine;

namespace Tatsu.Core
{
    public class EquipmentSystem : Actor, IEquipmentService
    {
        [SerializeField] private SerializableDictionary<ItemType, EquipmentItemData> _playerEquippedItems = new();
        
        private EquipmentSystem() { }

        private IPlayerAnimationsService _playerAnimationsService;
        
        private EventHandle _itemEquippedHandler;
        private EventHandle _itemMovedHandler;

        public SerializableDictionary<ItemType, EquipmentItemData> PlayerEquippedItems => _playerEquippedItems;
        
        public void EquipItem(EquipmentItemData itemBaseData)
        {
            ItemType itemType = itemBaseData.ItemType;
            
            if (!_playerEquippedItems.ContainsKey(itemType))
            {
                return;
            }

            if (_playerEquippedItems.TryGetValue(itemType, out EquipmentItemData equipmentItemData))
            {
                if (equipmentItemData != null)
                {
                    equipmentItemData.RemoveEquipmentStats();
                }
            }
            
            _playerEquippedItems[itemType] = itemBaseData;
            itemBaseData.AddEquipmentStats();
            
            _playerAnimationsService.PlayTriggerAnimation(AnimationType.Positive);
        }

        public void UnequipItem(ItemType itemType)
        {
            if (!_playerEquippedItems.TryGetValue(itemType, out EquipmentItemData currentEquipmentData))
            {
                return;
            }

            if (currentEquipmentData == null)
            {
                return;
            }
            
            currentEquipmentData.RemoveEquipmentStats();
            _playerEquippedItems[itemType] = null;
            
            _playerAnimationsService.PlayTriggerAnimation(AnimationType.LookAround);
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            _playerAnimationsService = ServiceLocator.GetChecked<IPlayerAnimationsService>();
        }
    }
}