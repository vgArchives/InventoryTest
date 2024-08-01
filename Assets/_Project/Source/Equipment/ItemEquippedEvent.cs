using Coimbra.Services.Events;

namespace Tatsu.Core
{
    public readonly partial struct ItemEquippedEvent : IEvent
    {
        public readonly EquipmentItemData ItemBaseData;

        public ItemEquippedEvent(EquipmentItemData itemBaseData)
        {
            ItemBaseData = itemBaseData;
        }
    }
}
