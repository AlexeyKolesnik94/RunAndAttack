using UniRx;

namespace Bonuses.Diamonds
{
    public class DiamondModel
    {
        public IntReactiveProperty DiamondProperty { get; private set; } = new IntReactiveProperty();

        public void AddDiamond() => DiamondProperty.Value++;
    }
}