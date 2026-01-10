using UnityEngine;
using UniRx;
using DG.Tweening;
using System.Collections;
using Cysharp.Threading.Tasks;
using System.Threading;


public class TweenMainCamera : MonoBehaviour
{
    private enum ThisState
    {
        Default,
        UpdatePos,
        ZoomIn,
        ZoomOut,
    }

    //public Subject<Unit> Default = new Subject<Unit>();

    /// <summary> アウトアニメーションのスタート時のディレイ </summary>
    private float _outStartDelay = 0;

    private Sequence _sequence;

    [SerializeField] private Transform _thisTrans;

    private Vector3 _initThisPos;

    private Vector3 _updatePos;

    private CancellationToken _destroyToken;

    public bool _isDebug;

    private void Awake()
    {
        _initThisPos = _thisTrans.localPosition;
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

            case ThisState.UpdatePos:
                _thisTrans.localPosition = _updatePos;


                break;

            case ThisState.ZoomIn:
                _sequence?.Kill();
                _sequence = DOTween.Sequence();
                _sequence.SetLink(gameObject);                

                _sequence.Insert(0, _thisTrans.DOLocalMoveZ(_initThisPos.z, 0.5f).SetEase(Ease.OutExpo));
               
                //非同期待機条件
                await _sequence.AsyncWaitForCompletion();
                break;

        }

    }

    public void UpdatePos(Vector3 pos)
    {
        _updatePos = pos;
        SetThisState(ThisState.UpdatePos, _destroyToken).Forget();
    }

    public async UniTask PlayZoomInAnim(float delay = 0)
    {
        Debug.Log("ズームインアニメーション");
        _outStartDelay = delay;
        await SetThisState(ThisState.ZoomIn, _destroyToken);
    }

}
