using Common.Infrastructure;
using Locations;
using PlayerScripts;
using UIGame;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace Enemy
{
    public class Hunting : MonoBehaviour
    {
        [SerializeField] private float enemySpeed;

        private Animator _animator;
        private Transform target;
        private CheckLocation checkLocation;
        private Pause _pause;
        
        private static readonly int Run = Animator.StringToHash("Run");

        [Inject]
        private void Construct(PlayerMove playerMove, Pause pause)
        {
            SetupTargetPlayer(playerMove);
            _pause = pause;
        }

        
        private void Start()
        {
            
            MessageBroker.Default.Receive<PlayerInstantiateEvent>()
                .Subscribe(PlayerInstantiated).AddTo(this);
            
            _animator = GetComponent<Animator>();
            
            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    _animator.SetBool(Run, checkLocation.IsEnemyField);
                    if (checkLocation.IsEnemyField)
                    {
                        MoveToTarget();
                    }
                }).AddTo(this);
        }
        
        private void PlayerInstantiated(PlayerInstantiateEvent pie) =>
            SetupTargetPlayer(pie.PlayerMove);

        private void SetupTargetPlayer(PlayerMove playerMove)
        {
            target = playerMove.transform;
            checkLocation = playerMove.GetComponent<CheckLocation>();
        }


        private void MoveToTarget()
        {
            if (_pause.IsPaused.Value) return;
            var position = target.position;
            transform.position = Vector3.MoveTowards(transform.position,
                position, enemySpeed * Time.deltaTime);
            
            Vector3 diff = target.position - transform.position;
            diff.Normalize();

            float rot_y = Mathf.Atan2(diff.x, diff.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, rot_y,  0);
        }
        
    }
}