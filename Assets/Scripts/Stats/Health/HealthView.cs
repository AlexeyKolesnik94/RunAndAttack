using UnityEngine;
using UnityEngine.UI;

namespace Stats.Health
{
    public class HealthView : MonoBehaviour
    {
        [SerializeField] private Image healthImage;

        public void SetHealth(float startHealth)
        {
            healthImage.fillAmount = startHealth / 100f;
        }
        
        public void FillingHealth(float damage) => healthImage.fillAmount = damage / 100f;
    }
}
