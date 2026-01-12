using UnityEngine;
using UniRx;
using DG.Tweening;
using System.Collections;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;
using UnityEngine.UI;

public class TweenResultRankTextRoot : MonoBehaviour
{
    private enum ThisState
    {
        Default,
        In,

    }

    //public Subject<Unit> Default = new Subject<Unit>();


    /// <summary> アウトアニメーションのスタート時のディレイ </summary>
    private float _outStartDelay = 0;

    public bool _isDebug;

    [SerializeField] private RectTransform _thisRect;

    [SerializeField] private CanvasGroup _thisGroup;

    [SerializeField] private TweenResultRankTextParts[] _tweenResultRankTextParts;

    private Vector2 _initThisPos;

    private Sequence _sequence;

    private CancellationToken _destroyToken;

    private void Awake()
    {
        _initThisPos = _thisRect.anchoredPosition;
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
                _thisGroup.alpha = 0f;


                break;

            case ThisState.In:
                _sequence?.Kill();
                _sequence = DOTween.Sequence();
                _sequence.SetLink(gameObject);
                _thisGroup.alpha = 1f;

                for (int i = 0; i < _tweenResultRankTextParts.Length; i++)
                {
                    _tweenResultRankTextParts[i].PlayInAnim(_destroyToken, 0.05f*i).Forget();
                }

                //非同期待機条件
                await _sequence.AsyncWaitForCompletion();
                break;

            

        }

    }


    public async UniTask PlayInAnim(float delay = 0)
    {
        _outStartDelay = delay;
        await SetThisState(ThisState.In, _destroyToken);
    }

}
