using System;
using Enemy;
using Locations;
using UIGame;
using UniRx;
using UnityEngine;
using Zenject;

namespace PlayerScripts
{
    [RequireComponent(typeof(Pool.Pool))]
    public class PlayerFire : MonoBehaviour
    {
        [SerializeField] private float fireRate;
        [SerializeField] private Transform gun;
        [SerializeField] private CheckLocation _checkLocation;

        private Pool.Pool _pool;
        private Pause _pause;

        private bool isFire;
        
        [Inject]
        private void Construct(Pause pause)
        {
            _pause = pause;
        }

        private void Start()
        {
            _pool = GetComponent<Pool.Pool>();
            Fire();
        }

        private void Fire()
        {
            Observable.Timer(TimeSpan.FromSeconds(fireRate))
                .Repeat()
                .Subscribe(_ =>
                {
                    if (_checkLocation.IsEnemyField && !_pause.IsPaused.Value)
                    {
                        _pool.GetFreeElement(gun.position, transform.rotation);
                    }
                }).AddTo(this);
        }
    }
}
