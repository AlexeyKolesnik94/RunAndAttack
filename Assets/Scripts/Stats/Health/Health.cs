using UniRx;
using UnityEngine;
using Zenject;

namespace Stats.Health
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private HealthView healthView;

        private HealthModel _healthModel;

        [Inject]
        private void Construct(HealthModel healthModel)
        {
            _healthModel = healthModel;
        }
        
        private void Start()
        {
            healthView.SetHealth(_healthModel.Health.Value);
            
            _healthModel.Health
                .Subscribe(newHpVal => { healthView.FillingHealth(newHpVal); }).AddTo(this);
        }
    }
}