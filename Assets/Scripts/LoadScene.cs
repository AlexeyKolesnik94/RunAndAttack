using System;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    private void Start()
    {
        Observable.Timer(TimeSpan.FromSeconds(2.5f)).Subscribe(_ => { Load(); }).AddTo(this);
    }

    private void Load() => 
        SceneManager.LoadSceneAsync("SampleScene");
}