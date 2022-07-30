using Common.Infrastructure;
using Locations;
using UIGame;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace PlayerScripts
{
    [RequireComponent(typeof(PlayerFire), typeof(CheckLocation),
        typeof(Pool.Pool))]
    public class PlayerMove : MonoBehaviour
    {
        [SerializeField] private float movingSpeed;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private Animator animator;
        
        private Vector3 _direction;
        private Vector3 _rotation;
        
        private Control _control;
        private Rigidbody _rb;
        private Pause _pause;
        
        private static readonly int Run = Animator.StringToHash("Run");

        [Inject]
        private void Construct(Pause pause)
        {
            _pause = pause;
        }
        
        private void Awake()
        {
            _control = new Control();
            _rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            MessageBroker.Default.Publish(new PlayerInstantiateEvent {PlayerMove = this});
            
            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    if (_pause.IsPaused.Value) return;
                    animator.SetBool(Run, _direction != Vector3.zero);
                    Moving();
                    RotationPlayer();
                }).AddTo(this);
        }

        private void OnEnable()
        {
            _control.Enable();
        }

        private void OnDisable()
        {
            _control.Disable();
        }

        private void Moving()
        {
            _direction = _control.Player.Move.ReadValue<Vector3>();
            Vector3 move = transform.right * _direction.x + transform.forward * _direction.z;
            _rb.velocity = move * movingSpeed;
        }

        private void RotationPlayer()
        {
            _rotation = _control.Player.Look.ReadValue<Vector3>();
            transform.Rotate(0, _rotation.x * rotationSpeed * Time.deltaTime, 0);
        }
    }
}
