using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UIGame.Buttons
{
    public class PauseBtn : MonoBehaviour
    {
        private Pause _pause;
        private Button _pauseBtn;

        [Inject]
        private void Construct(Pause pause)
        {
            _pause = pause;
        }
        
        private void Start()
        {
            _pauseBtn = GetComponent<Button>();

            _pauseBtn.OnClickAsObservable()
                .Subscribe(_ => { _pause.IsPaused.Value = true; }).AddTo(this);
        }
    }
}