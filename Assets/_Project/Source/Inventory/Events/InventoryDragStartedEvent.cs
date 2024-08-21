using Coimbra.Services.Events;

namespace Project.Core
{
    public readonly partial struct InventoryDragStartedEvent : IEvent
    {
        public readonly ItemType ItemType;

        public InventoryDragStartedEvent(ItemType itemType)
        {
            ItemType = itemType;
        }
    }
}