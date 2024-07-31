using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Tatsu.Core
{
    public class StatSlotView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _valueText;

        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _buffColor;
        [SerializeField] private Color _debuffColor;
        
        public void Initialize(KeyValuePair<StatType, Stat> stat)
        {
            _nameText.SetText(stat.Key.ToString());
            _valueText.SetText($"{stat.Value.OriginalValue:00}");

            stat.Value.OnStatValueChange += HandleStatValueChange;
        }

        private void HandleStatValueChange(int originalValue, int effectiveValue)
        {
            _valueText.SetText($"{effectiveValue:00}");
            
            if (originalValue == effectiveValue)
            {
                _valueText.color = _defaultColor;
                return;
            }
            
            _valueText.color = originalValue > effectiveValue ? _debuffColor : _buffColor;
        }
    }
}
