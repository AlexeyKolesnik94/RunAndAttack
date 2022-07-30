using PlayerScripts;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace Bonuses.Coins
{
    public class Coin : MonoBehaviour
    {
        private CoinModel _coinModel;

        [Inject]
        private void Construct(CoinModel coinModel)
        {
            _coinModel = coinModel;
        }
        
        private void Awake()
        {
            transform.Rotate(0, 0, 90);
        }
        
        private void Start()
        {
            this.OnTriggerEnterAsObservable()
                .Subscribe(col =>
                {
                    if (col.GetComponent<PlayerMove>() == null) return;
                    _coinModel.AddCoin();
                    Destroy(gameObject);
                }).AddTo(this);
        }
    }
}