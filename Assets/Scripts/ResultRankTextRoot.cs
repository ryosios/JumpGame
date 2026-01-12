using UnityEngine;
using UniRx;
using DG.Tweening;
using System.Collections;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;
using UnityEngine.UI;

public class ResultRankTextRoot : MonoBehaviour
{
    private enum ThisState
    {
        Default,
        Update,
  
    }

    //public Subject<Unit> Default = new Subject<Unit>();


    /// <summary> アウトアニメーションのスタート時のディレイ </summary>
    private float _outStartDelay = 0;

    public bool _isDebug;

    [SerializeField] private RectTransform _thisRect;

    [SerializeField] private ResultRankTextParts[] _resultRankTextParts;

    [SerializeField] private TweenResultRankTextRoot _tweenResultRankTextRoot;

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
    private async UniTask SetThisState(ThisState thisState, CancellationToken cancellationToken, int[] pointValue = null)
    {
        var state = thisState;

        switch (state)
        {
            case ThisState.Default:


                break;

            case ThisState.Update:
                //リザルト内容更新
                for (int i = 0; i < _resultRankTextParts.Length; i++)
                {
                    _resultRankTextParts[i].SetText(cancellationToken, pointValue[i]);
                }

                

                break;

          


        }

    }

    /// <summary>
    /// テキスト設定用
    /// </summary>
    public void SetText(CancellationToken cancellationToken, int[] pointValue)
    {
        SetThisState(ThisState.Update, cancellationToken, pointValue).Forget();
    }


}
