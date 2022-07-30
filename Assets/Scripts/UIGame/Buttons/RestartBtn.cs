using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UIGame.Buttons
{
    public class RestartBtn : MonoBehaviour
    {
        private Button _restartBtn;

        private void Start()
        {
            _restartBtn = GetComponent<Button>();

            _restartBtn.OnClickAsObservable()
                .Subscribe(_ => { SceneManager.LoadSceneAsync("SampleScene"); }).AddTo(this);
        }
    }
}