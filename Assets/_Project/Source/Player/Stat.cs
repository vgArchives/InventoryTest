using System;
using UnityEngine;

namespace Tatsu.Core
{
    [Serializable]
    public class Stat
    {
        public event Action<int, int, int> OnStatValueChange;
        
        [SerializeField] private int _originalValue;
        [SerializeField] private int _maximumValue;
        
        private int _effectiveValue;

        public int OriginalValue => _originalValue;
        
        public int MaximumValue => _maximumValue;

        public void Initialize()
        {
            _effectiveValue = _originalValue;
        }

        public void UpdateStat(int itemValue)
        {
            _effectiveValue += itemValue;
            _effectiveValue = Mathf.Clamp(_effectiveValue, 0, _maximumValue);
            
            OnStatValueChange?.Invoke(_originalValue, _effectiveValue, _maximumValue);
        }

        public void ResetStat()
        {
            _effectiveValue = _originalValue;
            
            OnStatValueChange?.Invoke(_originalValue, _effectiveValue, _maximumValue);
        }
    }
}
