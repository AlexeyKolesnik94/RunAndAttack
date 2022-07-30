using UniRx;

namespace Bonuses.Bank
{
    public class BonusesInBankModel
    {
        public IntReactiveProperty CoinInBankProperty { get; private set; } = new IntReactiveProperty();
        public IntReactiveProperty DiamondInBankProperty { get; private set; } = new IntReactiveProperty();
    }
}