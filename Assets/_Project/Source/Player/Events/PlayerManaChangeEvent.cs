using Coimbra.Services.Events;

namespace Tatsu.Core
{
    public readonly partial struct PlayerManaChangeEvent : IEvent
    {
        public readonly int EffectiveHealthValue;

        public PlayerManaChangeEvent(int effectiveHealthValue)
        {
            EffectiveHealthValue = effectiveHealthValue;
        }
    }
}