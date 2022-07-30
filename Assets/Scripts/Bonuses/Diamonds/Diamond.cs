using PlayerScripts;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace Bonuses.Diamonds
{
    public class Diamond : MonoBehaviour
    {
        private DiamondModel _diamondModel;

        [Inject]
        private void Construct(DiamondModel diamondModel)
        {
            _diamondModel = diamondModel;
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
                    _diamondModel.AddDiamond();
                    Destroy(gameObject);
                }).AddTo(this);
        }
    }
}