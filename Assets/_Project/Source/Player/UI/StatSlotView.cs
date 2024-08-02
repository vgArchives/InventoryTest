using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Tatsu.Core
{
    public class StatSlotView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _valueText;
        [Space(10)]
        
        [SerializeField] private StatDisplayType _displayType;
        [Space(10)]
        
        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _buffColor;
        [SerializeField] private Color _debuffColor;
        
        public void Initialize(KeyValuePair<StatType, Stat> stat)
        {
            _nameText.SetText(stat.Key.ToString());

            if (_displayType == StatDisplayType.Percentage)
            {
                float percentageValue = (float) stat.Value.OriginalValue / stat.Value.MaximumValue * 100;
                _valueText.SetText($"{percentageValue:0}%");
            }
            else
            {
                _valueText.SetText($"{stat.Value.OriginalValue:00}");
            }

            stat.Value.OnStatValueChange += HandleStatValueChange;
        }

        private void HandleStatValueChange(int originalValue, int effectiveValue, int maximumValue, int _)
        {
            if (_displayType == StatDisplayType.Percentage)
            {
                float percentageValue = (float) effectiveValue / maximumValue * 100;
                _valueText.SetText($"{percentageValue:0}%");
            }
            else
            { 
                _valueText.SetText($"{effectiveValue:00}");
            }
            
            if (originalValue == effectiveValue)
            {
                _valueText.color = _defaultColor;
                return;
            }
            
            _valueText.color = originalValue > effectiveValue ? _debuffColor : _buffColor;
        }
    }
}
