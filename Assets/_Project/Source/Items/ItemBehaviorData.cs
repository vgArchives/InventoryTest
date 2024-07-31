using System.Collections.Generic;
using Coimbra;
using UnityEngine;

namespace Tatsu.Core
{
    public abstract class ItemBehaviorBaseData : ScriptableObject
    {
        public virtual void ResolveItemBehavior(SerializableDictionary<StatType, int> affectedStats) { }
    }
}
 