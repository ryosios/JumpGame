using UnityEngine;
using UniRx;
using DG.Tweening;
using System.Collections;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;
using UnityEngine.UI;

public class ResultRankTextParts : MonoBehaviour
{
    private enum ThisState
    {
        Default,
        Update,
        SlideIn,
    }

    //public Subject<Unit> Default = new Subject<Unit>();


    /// <summary> アウトアニメーションのスタート時のディレイ </summary>
    private float _outStartDelay = 0;

    public bool _isDebug;

    [SerializeField] private RectTransform _thisRect;

    [SerializeField] private Text _rankingText;

    private string _rankingString = "test";

    [SerializeField] private TweenResultRankTextParts _tweenResultRankTextParts;

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

            case ThisState.Update:
                //リザルト内容更新
                
                SetText(_rankingText,_rankingString);
                
                SetThisState(ThisState.SlideIn,cancellationToken).Forget();

                break;

            case ThisState.SlideIn:
                //スライドインアニメーション
                _tweenResultRankTextParts.PlayInAnim().Forget();
              

               
                break;

        }

    }

    private void SetText(Text text, string comment)
    {
        text.text = comment;
    }

    public async UniTask PlayInAnim(float delay = 0)
    {
        _outStartDelay = delay;
        await SetThisState(ThisState.SlideIn, _destroyToken);
    }

}
