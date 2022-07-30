using TMPro;
using UnityEngine;

namespace Bonuses.Diamonds
{
    public class DiamondView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI diamondInInventoryText;
        
        public void DiamondTextView(int diamondInInventory, int diamondInBank) => 
            diamondInInventoryText.text = $"{diamondInInventory} / {diamondInBank}";

        public void BankDiamondView(int diamondInBank) => diamondInInventoryText.text = $"0 / {diamondInBank}";
    }
}