using Coimbra.Services.Events;

namespace Project.Core
{
    public readonly partial struct PlayerHealthChangeEvent : IEvent
    {
        public readonly int EffectiveHealthValue;
        public readonly int PreviousEffectiveHealthValue;

        public PlayerHealthChangeEvent(int effectiveHealthValue, int previousEffectiveHealthValue)
        {
            EffectiveHealthValue = effectiveHealthValue;
            PreviousEffectiveHealthValue = previousEffectiveHealthValue;
        }
    }
}
