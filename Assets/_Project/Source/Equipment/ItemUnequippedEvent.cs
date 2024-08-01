using Coimbra.Services.Events;

namespace Tatsu.Core
{
    public readonly partial struct ItemUnequippedEvent : IEvent
    {
        public readonly ItemType ItemType;

        public ItemUnequippedEvent(ItemType itemType)
        {
            ItemType = itemType;
        }
    }
}