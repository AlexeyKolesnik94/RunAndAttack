using System;
using Common.Infrastructure;
using Locations;
using PlayerScripts;
using UIGame;
using UniRx;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class EnemySpawnFactory : MonoBehaviour
    {
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private MeshCollider plane;
        [SerializeField] private float spawnRate;
        
        private float _x;
        private float _z;

        private Enemy _enemyPos;
        private CheckLocation _checkLocation;
        private DiContainer _container;
        private Pause _pause;
        
        [Inject]
        private void Construct(PlayerMove player, DiContainer container, Pause pause)
        {
            _checkLocation = player.GetComponent<CheckLocation>();
            _container = container;
            _pause = pause;
        }

        private void Start()
        {
            MessageBroker.Default.Receive<PlayerInstantiateEvent>()
                .Subscribe(PlayerInstantiated).AddTo(this);
            
            Observable.Timer(TimeSpan.FromSeconds(spawnRate))
                .Repeat()
                .Subscribe(_ => { if (_checkLocation.IsEnemyField) Spawn(); }).AddTo(this);
        }
        
        private void PlayerInstantiated(PlayerInstantiateEvent pie) =>
            SetupTargetPlayer(pie.PlayerMove);
        
        private void SetupTargetPlayer(PlayerMove playerMove) =>
            _checkLocation = playerMove.GetComponent<CheckLocation>();

        private void Spawn()
        {
            if (_pause.IsPaused.Value) return;
            
            var position = plane.transform.position;
            
            _x = Random.Range(position.x - Random.Range(0, plane.bounds.extents.x),
                position.x + Random.Range(0, plane.bounds.extents.x));
            _z = Random.Range(position.z - Random.Range(0, plane.bounds.extents.z),
                position.z + Random.Range(0, plane.bounds.extents.z));
            
            Vector3 pos = new Vector3(_x, spawnPoint.position.y, _z);
            
            // we are factory, so this is norm
            _container.InstantiatePrefab(enemyPrefab, pos, Quaternion.identity, null);
        }
    }
}