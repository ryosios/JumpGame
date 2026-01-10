using UnityEngine;
using UniRx;
using DG.Tweening;
using System.Collections;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

public class ResultRoot : MonoBehaviour
{
    private enum ThisState
    {
        Default,
        ResultUpdate,//内容更新
        ResultIn,//描画
    }

    //public Subject<Unit> Default = new Subject<Unit>();

    public bool _isDebug;

    [SerializeField] private CanvasGroup _thisGroup;

    [SerializeField] private TweenResultRoot _tweenResultRoot;

    private CancellationToken _destroyToken;

    private void Start()
    {
        //GameMasterのインスタンスができる前に購読するとまずいのでStart
        _destroyToken = this.GetCancellationTokenOnDestroy();

        GameMaster.Instance.ResultStart.Subscribe(_ =>
        {
            Debug.Log("Result2");//ここが表示来ない
            SetThisState(ThisState.ResultUpdate, _destroyToken).Forget();
           

        }).AddTo(this);


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

            case ThisState.ResultUpdate:
                Debug.Log("Result3");
                //内容更新

                //描画ステートに遷移
                SetThisState(ThisState.ResultIn, _destroyToken).Forget();

                break;
            case ThisState.ResultIn:
                //描画
                _tweenResultRoot.PlayInAnim();

                break;

        }

    }

   

}
