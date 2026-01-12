using UnityEngine;
using UniRx;
using DG.Tweening;
using System.Collections;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

public class TemplateTween : MonoBehaviour
{
    private enum ThisState
    {
        Default,
        In,
    }

    //public Subject<Unit> Default = new Subject<Unit>();


    /// <summary> アウトアニメーションのスタート時のディレイ </summary>
    private float _outStartDelay = 0;

    private Sequence _sequence;

    public bool _isDebug;

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

            case ThisState.In:
                _sequence?.Kill();
                _sequence = DOTween.Sequence();
                _sequence.SetLink(gameObject);
                _sequence.SetUpdate(true);

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
