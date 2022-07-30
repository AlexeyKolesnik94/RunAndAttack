using Bonuses.Bank;
using Bonuses.Coins;
using Bonuses.Diamonds;
using Common.Infrastructure;
using Locations;
using PlayerScripts;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace Bonuses
{
    public class BonusCounter : MonoBehaviour
    {
        [SerializeField] private CoinView coinView;
        [SerializeField] private DiamondView diamondView;

        private CoinModel _coinModel;
        private DiamondModel _diamondModel;
        private BonusesInBankModel _bankModel;
        
        private CheckLocation _checkLocation;

        [Inject]
        private void Construct(PlayerMove player, CoinModel coinModel, BonusesInBankModel bankModel, DiamondModel diamondModel)
        {
            _checkLocation = player.GetComponent<CheckLocation>();
            
            _coinModel = coinModel;
            _diamondModel = diamondModel;
            _bankModel = bankModel;
        }

        private void Awake()
        {
            MessageBroker.Default.Receive<PlayerInstantiateEvent>()
                .Subscribe(PlayerInstantiated).AddTo(this);
        }

        
        private void Start()
        {
            CoinCollection();
            DiamondCollection();

            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    if (_checkLocation.IsEnemyField) return;
                    
                    CoinCollectInBank();
                    DiamondCollectInBank();
                }).AddTo(this);
        }
        
        private void PlayerInstantiated(PlayerInstantiateEvent pie)
        {
            SetupPlayerLocation(pie.PlayerMove);
            _coinModel.CoinProperty.Value = 0;
            _diamondModel.DiamondProperty.Value = 0;
        }
        
        private void SetupPlayerLocation(PlayerMove playerMove)
        {
            _checkLocation = playerMove.GetComponent<CheckLocation>();
        }


        private void DiamondCollection()
        {
            _diamondModel.DiamondProperty
                .Subscribe(_ =>
                {
                    diamondView.DiamondTextView(_diamondModel.DiamondProperty.Value,
                        _bankModel.DiamondInBankProperty.Value);
                }).AddTo(this);
        }

        private void CoinCollection()
        {
            _coinModel.CoinProperty
                .Subscribe(_ =>
                {
                    coinView.CoinTextView(_coinModel.CoinProperty.Value,
                        _bankModel.CoinInBankProperty.Value);
                }).AddTo(this);
        }

        private void DiamondCollectInBank()
        {
            _bankModel.DiamondInBankProperty.Value += _diamondModel.DiamondProperty.Value;
            diamondView.BankDiamondView(_bankModel.DiamondInBankProperty.Value);
            _diamondModel.DiamondProperty.Value = 0;
        }

        private void CoinCollectInBank()
        {
            _bankModel.CoinInBankProperty.Value += _coinModel.CoinProperty.Value;
            coinView.BankCoinView(_bankModel.CoinInBankProperty.Value);
            _coinModel.CoinProperty.Value = 0;
        }
    }
}