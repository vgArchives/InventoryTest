using Coimbra.Services.Events;

namespace Tatsu.Core._Project.Source.Inventory.Events
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