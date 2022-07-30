using Bonuses;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Locations
{
    public class CheckLocation : MonoBehaviour
    {
        public bool IsEnemyField { get; private set; }
        
        private void Start()
        {
            
            this.OnTriggerEnterAsObservable()
                .Subscribe(col =>
                {
                    if (col.GetComponent<EnemyField>()) IsEnemyField = true;
                    
                    if (col.GetComponent<PlayerField>()) IsEnemyField = false;
                }).AddTo(this);
        }
    }
}