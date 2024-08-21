using Coimbra.Services.Events;

namespace Project.Core
{
    public readonly partial struct PlayerManaChangeEvent : IEvent
    {
        public readonly int EffectiveManaValue;
        public readonly int PreviousEffectiveManaValue;

        public PlayerManaChangeEvent(int effectiveManaValue, int previousEffectiveManaValue)
        {
            EffectiveManaValue = effectiveManaValue;
            PreviousEffectiveManaValue = previousEffectiveManaValue;
        }
    }
}