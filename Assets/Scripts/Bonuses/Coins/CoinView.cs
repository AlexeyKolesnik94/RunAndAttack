using TMPro;
using UnityEngine;

namespace Bonuses.Coins
{
    public class CoinView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI coiInInventoryText;
        
        public void CoinTextView(int coinInInventory, int coinInBank) => 
            coiInInventoryText.text = $"{coinInInventory} / {coinInBank}";

        public void BankCoinView(int coinInBank) => coiInInventoryText.text = $"0 / {coinInBank}";
    }
}