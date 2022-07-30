using AmmoScripts;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace Enemy
{
    public class DestroyEnemies : MonoBehaviour
    {
        [SerializeField] private GameObject[] bonuses;
        [SerializeField] private int percentBonusesCreate;

        private DiContainer _container;
        
        [Inject]
        private void Construct(DiContainer container)
        {
            _container = container;
        }
        
        private void Start()
        {
            this.OnTriggerEnterAsObservable()
                .Subscribe(col =>
                {
                    Ammo ammo = col.GetComponent<Ammo>();
                    if (!ammo) return;
                    ammo.gameObject.SetActive(false);
                    CreateBonus(gameObject.transform);
                    Destroy(gameObject);
                }).AddTo(this);
        }

        private void CreateBonus(Transform pos)
        {
            if (Random.Range(0, 100) >= percentBonusesCreate) return;
            int rand = Random.Range(0, bonuses.Length);
            var position = pos.position;
            _container.InstantiatePrefab(bonuses[rand], new Vector3(position.x, position.y + 1.5f
                , position.z), Quaternion.identity, null);
        }
    }
}