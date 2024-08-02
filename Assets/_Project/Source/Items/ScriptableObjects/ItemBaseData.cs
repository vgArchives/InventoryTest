using Coimbra;
using UnityEngine;

namespace Tatsu.Core
{ 
    public abstract class ItemBaseData : ScriptableObject
    {
        [SerializeField] private int _itemId;
        [Space(10)]
        
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
        
        protected IPlayerStatsService PlayerStatsService;

        public int ItemId => _itemId;
        public string ItemName => _itemName;
        public string ItemDescription => _itemDescription;
        public Sprite ItemSprite => _itemSprite;
        public ItemType ItemType => _itemType;
        protected SerializableDictionary<StatType, int> AffectedStats => _affectedStats;
    }
}
