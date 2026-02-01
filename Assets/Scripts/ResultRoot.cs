using UnityEngine;
using UniRx;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;
using System.Linq;

public class ResultRoot : MonoBehaviour
{
    private enum ThisState
    {
        Default,
        ResultUpdate,//内容更新
        ResultAnimation,//アニメーション
        ResultClear,//ポイント0に戻す
    }

    public bool _isDebug;

    [SerializeField] private GameMaster _gameMaster;

    [SerializeField] private CanvasGroup _thisGroup;

    [SerializeField] private TweenResultRoot _tweenResultRoot;

    [SerializeField] private EnemyCreater _enemyCreaeter;

    [SerializeField] private ResultRankTextRoot _resultRankTextRoot;

    [SerializeField] private EnemyCountGauge _enemyCountGauge;

    private int _resultPointCurrent;

    private int[] _resultPoints = new int[5];

    //一時保存用
    private static List<int> _resultPointsNow = new List<int>();

    

   

    /// <summary>リザルト最大行数</summary>
    private int _resultMaxValue = 5;

    private CancellationToken _destroyToken;

    private void Start()
    {
        //GameMasterのインスタンスができる前に購読するとまずいのでStart
        _destroyToken = this.GetCancellationTokenOnDestroy();

        _gameMaster.ResultStart.Subscribe(_ =>
        {
            
            SetThisState(ThisState.ResultUpdate, _destroyToken).Forget();
           

        }).AddTo(this);


        SetThisState(ThisState.Default, _destroyToken).Forget();
    }

    //デバッグ用
    /*
    private void Update()
    {
        if (_isDebug)
        {
            if (Input.GetKeyDown(KeyCode.Keypad9))
            {
                SetThisState(ThisState.ResultClear, _destroyToken).Forget();
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
                this.gameObject.SetActive(true);

                break;

            case ThisState.ResultUpdate:
                //内容更新
                //今回のポイントを保存
                Debug.Log("リザルト開始");
                _resultPointCurrent = _enemyCountGauge.GetCurrentPoint();

                //読み込み
                _resultPointsNow?.Clear();
                var data = SaveManager.Load();
                if (data != null)
                {
                    int[] resultPoints = data.resultPoints;
                    for (int i = 0; i < resultPoints.Length; i++)
                    {
                        _resultPointsNow.Add(resultPoints[i]);
                    }
                }
                
                //新規分を追加
                _resultPointsNow.Add(_resultPointCurrent);

                // 降順ソート（in-place）
                _resultPointsNow.Sort((a, b) => b.CompareTo(a));

                // 5個超えたら後ろから削る
                if (_resultPointsNow.Count > 5)
                {
                    _resultPointsNow.RemoveRange(5, _resultPointsNow.Count - 5);
                    
                }

                for (int i = 0; i < _resultPointsNow.Count; i++)
                {
                    _resultPoints[i] = _resultPointsNow[i];
                }

                //保存
                SaveManager.Save(new SaveData
                {
                    resultPoints = _resultPoints
                });

                //ポイントをUI描画に送信
                _resultRankTextRoot.SetText(_destroyToken, _resultPoints);

                //ResultAnimationステートに遷移
                SetThisState(ThisState.ResultAnimation, _destroyToken).Forget();

                break;
            case ThisState.ResultAnimation:
                Debug.Log("リザルトアニメーションスタート");
                //描画
                _tweenResultRoot.PlayInAnim();

                break;

            case ThisState.ResultClear:

                //読み込み
                _resultPointsNow?.Clear();

                var data2 = SaveManager.Load();
                if (data2 != null)
                {
                    int[] resultPoints = data2.resultPoints;
                    for (int i = 0; i < resultPoints.Length; i++)
                    {
                        _resultPoints[i] = 0;
                    }
                }

                //保存
                SaveManager.Save(new SaveData
                {
                    resultPoints = _resultPoints
                });

                //ポイントをUI描画に送信
                _resultRankTextRoot.SetText(_destroyToken, _resultPoints);

              
                break;

        }

    }

   

}
