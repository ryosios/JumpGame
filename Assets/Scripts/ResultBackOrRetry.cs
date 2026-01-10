using UnityEngine;
using UniRx;
using DG.Tweening;
using System.Collections;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;
using UnityEngine.UI;

public class ResultBackOrRetry : MonoBehaviour
{
    private enum ThisState
    {
        Default,
        TitleButton,
    }

    //public Subject<Unit> Default = new Subject<Unit>();


    /// <summary> アウトアニメーションのスタート時のディレイ </summary>
    private float _outStartDelay = 0;

    public bool _isDebug;

    [SerializeField] private Button ButtonTitle;

    [SerializeField] private Button ButtonRetry;

    [SerializeField] private RectTransform _thisRect;

    private CancellationToken _destroyToken;

    private void Awake()
    {
        _destroyToken = this.GetCancellationTokenOnDestroy();
        SetThisState(ThisState.Default, _destroyToken).Forget();
    }

    /* デバッグ用
    private void Update()
    {
        if (_isDebug)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                SetThisState(ThisState.Default,_destroyToken).Forget();
            }
           
        }
    }
    */

    /// <summary>
    /// ステート
    /// </summary>
    private async UniTask SetThisState(ThisState thisState, CancellationToken cancellationToken)
    {
        var state = thisState;

        switch (state)
        {
            case ThisState.Default:


                break;

            case ThisState.TitleButton:
                //タイトルに戻る
                UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScene");
            

              
                break;

        }

    }

    /*
    public async UniTask PlayInAnim(float delay = 0)
    {
        _outStartDelay = delay;
        await SetThisState(ThisState.TitleButton, _destroyToken);
    }
    */

    public void OnTitleButton()
    {
        SetThisState(ThisState.TitleButton,_destroyToken).Forget();
    }
}
