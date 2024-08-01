using System;
using UnityEngine;

namespace Tatsu.Core
{
    [Serializable]
    public class Stat
    {
        public event Action<int, int> OnStatValueChange;
        
        [SerializeField] private int _originalValue;
        [SerializeField] private int _maximumValue;
        
        private int _effectiveValue;

        public int OriginalValue => _originalValue;
        
        public int EffectiveValue => _effectiveValue;

        public void Initialize()
        {
            _effectiveValue = _originalValue;
        }

        public void UpdateStat(int itemValue)
        {
            _effectiveValue += itemValue;
            _effectiveValue = Mathf.Clamp(_effectiveValue, 0, _maximumValue);
            
            OnStatValueChange?.Invoke(_originalValue, _effectiveValue);
        }

        public void ResetStat()
        {
            _effectiveValue = _originalValue;
            
            OnStatValueChange?.Invoke(_originalValue, _effectiveValue);
        }
    }
}
