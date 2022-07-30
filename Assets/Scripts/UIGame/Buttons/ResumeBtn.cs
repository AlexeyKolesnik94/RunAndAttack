using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UIGame.Buttons
{
    public class ResumeBtn : MonoBehaviour
    {
        private Button _resumeBtn;
        private Pause _pause;

        [Inject]
        private void Construct(Pause pause)
        {
            _pause = pause;
        }
        
        private void Start()
        {
            _resumeBtn = GetComponent<Button>();

            _resumeBtn.OnClickAsObservable()
                .Subscribe(_ => { _pause.IsPaused.Value = false; }).AddTo(this);
        }
    }
}