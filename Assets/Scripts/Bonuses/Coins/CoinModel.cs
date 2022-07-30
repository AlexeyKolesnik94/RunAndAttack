using UniRx;

namespace Bonuses.Coins
{
    public class CoinModel
    {
        public IntReactiveProperty CoinProperty { get; private set; } = new IntReactiveProperty();

        public void AddCoin() => CoinProperty.Value++;
    }
}