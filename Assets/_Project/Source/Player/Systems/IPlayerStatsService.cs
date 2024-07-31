using Coimbra;
using Coimbra.Services;

namespace Tatsu.Core
{
    [RequiredService]
    public interface IPlayerStatsService : IService
    {
        public SerializableDictionary<StatType, Stat> PlayerStats { get;}
    }
}
