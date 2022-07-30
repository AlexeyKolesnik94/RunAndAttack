using UniRx;
using UnityEngine;
using Zenject;

namespace UIGame.Canvases
{
    public class PauseCanvas : MonoBehaviour
    {
        [SerializeField] private Canvas pauseCanvas;

        private Pause _pause;

        [Inject]
        private void Construct(Pause pause)
        {
            _pause = pause;
        }
        
        private void Start()
        {
            _pause.IsPaused
                .Subscribe(_ =>
                {
                    switch (_pause.IsPaused.Value)
                    {
                        case true:
                            pauseCanvas.gameObject.SetActive(true);
                            break;
                        case false:
                            pauseCanvas.gameObject.SetActive(false);
                            break;
                    }
                }).AddTo(this);
        }
    }
}