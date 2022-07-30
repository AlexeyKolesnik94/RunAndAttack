using PlayerScripts;
using Stats.Health;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace Damage
{
    public class DamageSystem : MonoBehaviour
    {
        private HealthModel _healthModel;

        [Inject]
        private void Construct(HealthModel healthModel)
        {
            _healthModel = healthModel;
        }
        
        private void Start()
        {
            this.OnCollisionEnterAsObservable()
                .Subscribe(col =>
                {
                    if (col.gameObject.GetComponent<PlayerMove>() != null)
                    {
                        _healthModel.AddDamage(25);
                    }
                }).AddTo(this);
        }
        
    }
}