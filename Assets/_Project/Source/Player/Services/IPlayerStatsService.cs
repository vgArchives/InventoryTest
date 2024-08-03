using Coimbra;
using Coimbra.Services;

namespace Tatsu.Core
{
    [RequiredService]
    public interface IPlayerStatsService : IService
    { 
        public bool IsAlive { get; }
        
        public PlayerInformationData PlayerData { get; }
        
        public SerializableDictionary<StatType, Stat> PlayerStats { get;}
        
        public void AddStatValue(StatType statType, int value);
        
        public void SubtractStatValue(StatType statType, int value);

        public Stat GetStat(StatType statType);
    }
}
