using Coimbra.Services.Events;

namespace Project.Core
{
    public readonly partial struct ItemConsumedEvent : IEvent
    {
        public readonly ItemSlotView ItemSlotView;
        public readonly int QuantityRemaining;

        public ItemConsumedEvent(ItemSlotView itemSlotView, int quantityRemaining)
        {
            ItemSlotView = itemSlotView;
            QuantityRemaining = quantityRemaining;
        }
    }
}