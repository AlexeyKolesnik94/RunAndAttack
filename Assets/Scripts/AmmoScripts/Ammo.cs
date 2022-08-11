using System;
using Common.Infrastructure;
using Enemy;
using Locations;
using PlayerScripts;
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
        [SerializeField] private float _ammoSpeed;

        private PoolObject _poolObject;
        private Pause _pause;

        private Vector3 _target;

        private EnemySpawnFactory _enemies;
        private PlayerMove _player;
        
        [Inject]
        private void Construct(Pause pause, EnemySpawnFactory enemySpawnFactory, PlayerMove playerMove)
        {
            _pause = pause;
            _enemies = enemySpawnFactory;
            _player = playerMove;
        }
        

        private void Start()
        {
            MessageBroker.Default.Receive<PlayerInstantiateEvent>()
                .Subscribe(PlayerInstantiated).AddTo(this);

            _poolObject = GetComponent<PoolObject>();


            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    if (_pause.IsPaused.Value || _enemies._enemies.Count == 0) return;
                    
                    _target = _enemies._enemies[0].transform.position;

                    // var position = transform.position;
                    // position = Vector3.MoveTowards(position,
                    //     new Vector3(_target.x, position.y, _target.z),
                    //     _ammoSpeed * Time.deltaTime);
                    // transform.position = position;

                    transform.position += _enemies.Dir * _ammoSpeed * Time.deltaTime;

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

        private void PlayerInstantiated(PlayerInstantiateEvent pie) =>
            SetupTargetPlayer(pie.PlayerMove);

        private void SetupTargetPlayer(PlayerMove playerMove) =>
            _player = playerMove;
    }
}
