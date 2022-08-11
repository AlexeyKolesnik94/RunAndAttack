using Bonuses.Bank;
using Bonuses.Coins;
using Bonuses.Diamonds;
using Enemy;
using PlayerScripts;
using Stats.Health;
using UIGame;
using UniRx;
using UnityEngine;
using Zenject;

namespace Common.Infrastructure
{
    public class LocationInstaller : MonoInstaller
    {
        public Transform StartPoint;
        public GameObject PlayerPrefab;

        public EnemySpawnFactory EnemyFactory;

        private PlayerMove _player;
        private HealthModel _healthModel;
        
        public override void InstallBindings()
        {
            EnemyFactoryBind();
            
            BindPause();
            BindCoinModel();
            BindDiamondModel();
            BindBankModel();
            BingHealthModel();
            InstantiatePlayer();
            
            MessageBroker.Default.Receive<PlayerDeathEvent>().Subscribe(_ =>
            {
                Container.Unbind<PlayerMove>();
                Destroy(_player.gameObject);
                InstantiatePlayer();
                _healthModel.Resurrect();
            }).AddTo(this);
        }

        private void BindPause()
        {
            Container
                .Bind<Pause>()
                .AsSingle()
                .NonLazy();
        }

        private void BindDiamondModel()
        {
            Container.Bind<DiamondModel>()
                .AsSingle()
                .NonLazy();
        }

        private void BindBankModel()
        {
            Container
                .Bind<BonusesInBankModel>()
                .AsSingle()
                .NonLazy();
        }

        private void BindCoinModel()
        {
            Container
                .Bind<CoinModel>()
                .AsSingle()
                .NonLazy();
        }
        
        private void BingHealthModel()
        {
            Container
                .Bind<HealthModel>()
                .AsSingle()
                .NonLazy();
            _healthModel = Container.Resolve<HealthModel>();
        }

        private void InstantiatePlayer()
        {
            _player = Container
                .InstantiatePrefabForComponent<PlayerMove>(PlayerPrefab, StartPoint.position, 
                    Quaternion.identity, null);
            
            Container
                .Bind<PlayerMove>()
                .FromInstance(_player);
        }

        private void EnemyFactoryBind()
        {
            Container.Bind<EnemySpawnFactory>()
                .FromInstance(EnemyFactory);
        }

    }
}