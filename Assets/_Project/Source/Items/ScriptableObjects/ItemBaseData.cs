using Coimbra;
using UnityEngine;

namespace Tatsu.Core
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "Item Data/Item")]
    public class ItemBaseData : ScriptableObject
    {
        [SerializeField] private string _itemName;
        [Space(10)]
        
        [TextArea]
        [SerializeField] private string _itemDescription;
        [Space(10)]
        
        [SerializeField] private Sprite _itemSprite;
        [Space(10)]
        
        [SerializeField] private ItemType _itemType;
        [Space(10)]
        
        [SerializeField] private SerializableDictionary<StatType, int> _affectedStats = new ();
        
        protected IPlayerStatsService _playerStatsService;
        
        public Sprite ItemSprite => _itemSprite;
        public ItemType ItemType => _itemType;
        protected SerializableDictionary<StatType, int> AffectedStats => _affectedStats;
        
        public virtual void ResolveItemBehavior() { }
    }
}
