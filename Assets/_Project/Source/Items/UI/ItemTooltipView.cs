using TMPro;
using UnityEngine;

namespace Tatsu.Core
{
    public class ItemTooltipView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _itemName;
        [SerializeField] private TextMeshProUGUI _itemDescription;
        
        public void Initialize(string itemName, string itemDescription)
        {
            _itemName.SetText(itemName);
            _itemDescription.SetText(itemDescription);
        }
    }
}