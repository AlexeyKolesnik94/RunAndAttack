using System;
using System.Collections.Generic;
using Common.Infrastructure;
using Locations;
using PlayerScripts;
using UIGame;
using UniRx;
using UnityEngine;
using Zenject;
using Zenject.SpaceFighter;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class EnemySpawnFactory : MonoBehaviour
    {
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private MeshCollider plane;
        [SerializeField] private float spawnRate;
        
        private float _x;
        private float _z;

        private Enemy _enemyPos;
        private CheckLocation _checkLocation;
        private DiContainer _container;
        private Pause _pause;
        private PlayerMove _player;

        [NonSerialized]
        public Vector3 Dir;
        
        public List<GameObject> _enemies = new List<GameObject>();

        [Inject]
        private void Construct(PlayerMove player, DiContainer container, Pause pause)
        {
            _checkLocation = player.GetComponent<CheckLocation>();
            _player = player;
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

        private void SetupTargetPlayer(PlayerMove playerMove)
        {
            _checkLocation = playerMove.GetComponent<CheckLocation>();
            _player = playerMove;
        }
            

        private void Spawn()
        {
            if (_pause.IsPaused.Value) return;
            
            var position = plane.transform.position;
            
            _x = Random.Range(position.x - Random.Range(0, plane.bounds.extents.x),
                position.x + Random.Range(0, plane.bounds.extents.x));
            _z = Random.Range(position.z - Random.Range(0, plane.bounds.extents.z),
                position.z + Random.Range(0, plane.bounds.extents.z));
            
            Vector3 pos = new Vector3(_x, 0, _z);
            
            // we are factory, so this is norm
            GameObject enemy = _container.InstantiatePrefab(enemyPrefab, pos, Quaternion.identity, null);
            _enemies.Add(enemy);

            _enemies.Sort((one, two) =>
                Vector3.Distance(one.transform.position, _player.transform.transform.position)
                    .CompareTo(Vector3.Distance(two.transform.position, _player.transform.position)));
            Transform target = _enemies[0].transform;
            Dir = target.position - _player.transform.position;
        }
    }
}