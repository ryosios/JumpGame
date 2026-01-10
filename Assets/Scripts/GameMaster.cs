using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using System.Threading;
public class GameMaster : MonoBehaviour
{
    //ゲーム進行管理用クラス

    public static GameMaster Instance { get; private set; }

    public enum GameMasterState
    {
        Default,
        EnemyCreate,
        Playing,
        GameEnd,
        GameTimeStop,
        GameTimeStart

    }


    [SerializeField] Player _player;

    [SerializeField] private BuffCardManager _buffCardManager;

    [SerializeField] TweenTransition _tweenTransition;

    [SerializeField] TweenRightUIs _tweenRightUIs;

    [SerializeField] EnemyCreater _enemyCreater;

    /// <summary>  EnemyCreateのSubject </summary>
    public Subject<Unit> EnemyCreateStart = new Subject<Unit>();

    /// <summary>  PlayStartのSubject </summary>
    public Subject<Unit> PlayStart = new Subject<Unit>();

    /// <summary>  GameEndのSubject </summary>
    public Subject<Unit> GameEnd = new Subject<Unit>();

    /// <summary>  GameTimeStartのSubject </summary>
    public Subject<Unit> GameTimeStart = new Subject<Unit>();

    /// <summary>  GameTimeStopのSubject </summary>
    public Subject<Unit> GameTimeStop = new Subject<Unit>();

    private CancellationToken _destroyToken;

    private void Awake()
    {
        _destroyToken = this.GetCancellationTokenOnDestroy();

        // すでにインスタンスがある → 破棄して重複を防ぐ
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // シーン切り替えでも破棄しない場合
        DontDestroyOnLoad(gameObject);

       

        _player.JumpOneTime.Subscribe(_ =>
        {
            Instance.SetGameMasterState(GameMasterState.Playing).Forget();

        }).AddTo(this);

        _buffCardManager.CardCreateStart.Subscribe(_=> 
        {
            Instance.SetGameMasterState(GameMasterState.GameTimeStop).Forget();

        }).AddTo(this);

        _buffCardManager.CardSelectedEnd.Subscribe(_ =>
        {
            Instance.SetGameMasterState(GameMasterState.GameTimeStart).Forget();

        }).AddTo(this);


    }

    private void Start()
    {
        Instance.SetGameMasterState(GameMasterState.Default).Forget();
    }

    /// <summary>
    /// ステート
    /// </summary>
    /// <param name="timeScaleValue">タイムスケール変更値</param>
    public async UniTask SetGameMasterState(GameMasterState gameMasterState)
    {
        var state = gameMasterState;

        switch (state)
        {
            case GameMasterState.Default:
                //トランジション開始
                _tweenTransition.PlayOutAnim(1f).Forget();
                
                //UI入場が終わるまで待機2
                await _tweenRightUIs.PlayInAnim(1.5f);
                //仮
                Debug.Log("待機済");
                SetGameMasterState(GameMasterState.EnemyCreate).Forget();

                break;

            case GameMasterState.EnemyCreate:
                EnemyCreateStart.OnNext(Unit.Default);


                break;

            case GameMasterState.Playing:
                GameMaster.Instance.PlayStart.OnNext(Unit.Default);

                break;

            case GameMasterState.GameEnd:
                
                break;

            case GameMasterState.GameTimeStart:
                GameMaster.Instance.GameTimeStart.OnNext(Unit.Default);
                ChangeTimeScale(1);
                break;

            case GameMasterState.GameTimeStop:
                GameMaster.Instance.GameTimeStop.OnNext(Unit.Default);
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
