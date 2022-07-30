using System;
using Locations;
using Pool;
using UIGame;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace AmmoScripts
{
    [RequireComponent(typeof(PoolObject))]
    public class Ammo : MonoBehaviour
    {
        private float _ammoSpeed = 15f;

        private PoolObject _poolObject;
        private Pause _pause;

        [Inject]
        private void Construct(Pause pause)
        {
            _pause = pause;
        }
        
        private void Start()
        {
            _poolObject = GetComponent<PoolObject>();
            
            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    
                    if (_pause.IsPaused.Value) return;
                    transform.Translate(Vector3.forward * _ammoSpeed * Time.deltaTime);
                    
                }).AddTo(this);
            
            Observable.Timer(TimeSpan.FromSeconds(5))
                .Subscribe(_ =>
                {
                    if (_pause.IsPaused.Value) return;
                    _poolObject.ReturnToPool();
                }).AddTo(this);

            this.OnTriggerEnterAsObservable()
                .Subscribe(col =>
                {
                    if (col.GetComponent<Wall>() != null)
                        gameObject.SetActive(false);
                }).AddTo(this);
        }
    }
}
