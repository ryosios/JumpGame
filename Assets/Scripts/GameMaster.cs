using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using System.Threading;
using System.Collections;
using System;

public class GameMaster : MonoBehaviour
{
    //ゲーム進行管理用クラス

    public enum GameMasterState
    {
        Default,
        EnemyCreate,
        Playing,//操作可能開始
        TimeOut,//制限時間時間終了
        Result,//リザルト
        GameEnd,//ゲーム終了
        GameTimeStop,
        GameTimeStart

    }


    [SerializeField] Player _player;

    [SerializeField] private BuffCardManager _buffCardManager;

    [SerializeField] TweenTransition _tweenTransition;

    [SerializeField] TweenRightUIs _tweenRightUIs;

    [SerializeField] EnemyCreater _enemyCreater;

    [SerializeField] TweenMainCamera _tweenMainCamera;

    [SerializeField] TimerGauge _timerGauge;

    /// <summary>  EnemyCreateのSubject </summary>
    public Subject<Unit> EnemyCreateStart = new Subject<Unit>();

    /// <summary>  PlayStartのSubject </summary>
    public Subject<Unit> PlayStart = new Subject<Unit>();

    /// <summary>  ResultのSubject </summary>
    public Subject<Unit> ResultStart = new Subject<Unit>();

    /// <summary>  GameEndのSubject </summary>
    public Subject<Unit> GameEnd = new Subject<Unit>();

    /// <summary>  GameTimeStartのSubject </summary>
    public Subject<Unit> GameTimeStart = new Subject<Unit>();

    /// <summary>  GameTimeStopのSubject </summary>
    public Subject<Unit> GameTimeStop = new Subject<Unit>();

    private CancellationToken _destroyToken;

    private void Awake()
    {       

        _player.JumpOneTime.Subscribe(_ =>
        {
            SetGameMasterState(GameMasterState.Playing, _destroyToken).Forget();

        }).AddTo(this);

        _buffCardManager.CardCreateStart.Subscribe(_=> 
        {
            SetGameMasterState(GameMasterState.GameTimeStop, _destroyToken).Forget();

        }).AddTo(this);

        _buffCardManager.CardSelectedEnd.Subscribe(_ =>
        {
            SetGameMasterState(GameMasterState.GameTimeStart, _destroyToken).Forget();

        }).AddTo(this);
        _timerGauge.TimerZero.Subscribe(_ =>
        {
            SetGameMasterState(GameMasterState.TimeOut, _destroyToken).Forget();

        }).AddTo(this);

    }

    private void Start()
    {
        SetGameMasterState(GameMasterState.Default,_destroyToken).Forget();
    }

    /// <summary>
    /// ステート
    /// </summary>
    /// <param name="timeScaleValue">タイムスケール変更値</param>
    public async UniTask SetGameMasterState(GameMasterState gameMasterState, CancellationToken cancellationToken)
    {
        var state = gameMasterState;

        switch (state)
        {
            case GameMasterState.Default:
                Time.timeScale = 1;
                //カメラを初期位置に
                _tweenMainCamera.UpdatePos(new Vector3(0,0,-32f));
                
                //トランジション開始
                _tweenTransition.PlayOutAnim(1f).Forget();   

                //UI入場が終わるまで待機
                await _tweenRightUIs.PlayInAnim(1.5f);

                await UniTask.Delay(TimeSpan.FromSeconds(0.2f), ignoreTimeScale: true);

                //敵クリエイトステートに遷移
                SetGameMasterState(GameMasterState.EnemyCreate,_destroyToken).Forget();

                break;

            case GameMasterState.EnemyCreate:
                //敵生成
                EnemyCreateStart.OnNext(Unit.Default);

                await UniTask.Delay(TimeSpan.FromSeconds(1.5f), ignoreTimeScale: true);
                //カメラをプレイ位置にズームイン。
                await _tweenMainCamera.PlayZoomInAnim();

                //待機後にステート遷移
                SetGameMasterState(GameMasterState.Playing, _destroyToken).Forget();

                break;

            case GameMasterState.Playing:
                PlayStart.OnNext(Unit.Default);

                break;

            case GameMasterState.TimeOut:
                Debug.Log("時間切れ！");
                //操作不可にする

                //ちょっと待ってもいいかも

                //リザルトに遷移
                SetGameMasterState(GameMasterState.Result, _destroyToken).Forget();

                break;

            case GameMasterState.Result:
                //リザルトUI表示 //プレイヤー側で入力できないようにする。
                ResultStart.OnNext(Unit.Default);               
                
                //時間を止める。
                SetGameMasterState(GameMasterState.GameTimeStop,cancellationToken).Forget();

                break;

            case GameMasterState.GameEnd:
                
                break;

            case GameMasterState.GameTimeStart:
                GameTimeStart.OnNext(Unit.Default);
                ChangeTimeScale(1);
                break;

            case GameMasterState.GameTimeStop:
                GameTimeStop.OnNext(Unit.Default);
                ChangeTimeScale(0);
                break;

                //非同期待機条件
               // await _sequence.AsyncWaitForCompletion();
        }
    }

    /// <summary>
    /// 時間変更用
    /// </summary>
    /// <param name="value">タイムスケール変更値</param>
    private void ChangeTimeScale(float value)
    {
        Time.timeScale = value;
    }
}
