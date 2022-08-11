using UniRx;

namespace Stats.Health
{
    public class HealthModel
    {
        public FloatReactiveProperty Health { get; private set; } = new FloatReactiveProperty(100);
        
        private readonly float _baseHealthValue;
        private bool _isDead;

        private HealthModel()
        {
            _baseHealthValue = Health.Value;
        }

        public void AddDamage(float damage)
        {
            Health.Value -= damage;
            if (!(Health.Value <= 0) || _isDead) return;
            _isDead = true;
            MessageBroker.Default.Publish(new PlayerDeathEvent());
        }

        public void Resurrect()
        {
            Health.Value = _baseHealthValue;
            _isDead = false;
        }
    }
}