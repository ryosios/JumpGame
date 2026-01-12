using UnityEngine;
using UniRx;
using DG.Tweening;
using System.Collections;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;
using UnityEngine.UI;

public class TweenResultRankTextParts : MonoBehaviour
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
                _sequence.SetUpdate(true);
                _thisGroup.alpha = 0f;
                _thisRect.anchoredPosition = new Vector2(_initThisPos.x +500f,_initThisPos.y);

                _sequence.Insert(0f,_thisRect.DOAnchorPosX(_initThisPos.x,0.5f).SetEase(Ease.OutExpo));
                _sequence.Insert(0f, _thisGroup.DOFade(1f, 0.5f).SetEase(Ease.OutCubic));

                //非同期待機条件
                await _sequence.AsyncWaitForCompletion();
                break;

            

        }

    }


    public async UniTask PlayInAnim(CancellationToken cancellationToken,float delay = 0)
    {
        _outStartDelay = delay;
        await SetThisState(ThisState.In, _destroyToken);
    }

}
