using Coimbra;
using Coimbra.Services;
using Coimbra.Services.Events;
using UnityEngine;

namespace Tatsu.Core
{
    public class EquipmentSystem : Actor, IEquipmentService
    {
        [Space(10)]
        [SerializeField] private SerializableDictionary<ItemType, EquipmentItemData> _playerEquippedItems = new();
        
        private EquipmentSystem() { }

        private IPlayerAnimationsService _playerAnimationsService;
        
        private EventHandle _itemEquippedHandler;
        private EventHandle _itemMovedHandler;

        public SerializableDictionary<ItemType, EquipmentItemData> PlayerEquippedItems => _playerEquippedItems;
        
        public void EquipItem(EquipmentItemData equipmentData)
        {
            ItemType equipmentType = equipmentData.ItemType;
            
            if (!_playerEquippedItems.ContainsKey(equipmentType))
            {
                return;
            }

            if (_playerEquippedItems.TryGetValue(equipmentType, out EquipmentItemData equipmentItemData))
            {
                if (equipmentItemData != null)
                {
                    equipmentItemData.RemoveEquipmentStats();
                }
            }
            
            equipmentData.AddEquipmentStats();
            _playerEquippedItems[equipmentType] = equipmentData;
            
            _playerAnimationsService.PlayTriggerAnimation(AnimationType.Positive);
        }

        public void UnequipItem(ItemType equipmentType)
        {
            if (!_playerEquippedItems.TryGetValue(equipmentType, out EquipmentItemData equipmentItemData))
            {
                return;
            }

            if (equipmentItemData == null)
            {
                return;
            }
            
            equipmentItemData.RemoveEquipmentStats();
            _playerEquippedItems[equipmentType] = null;
            
            _playerAnimationsService.PlayTriggerAnimation(AnimationType.LookAround);
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            _playerAnimationsService = ServiceLocator.GetChecked<IPlayerAnimationsService>();
        }
    }
}