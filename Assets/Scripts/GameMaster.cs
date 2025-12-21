using UnityEngine;
using UniRx;

public class GameMaster : MonoBehaviour
{
    //ゲーム進行管理用クラス

    public static GameMaster Instance { get; private set; }

    public enum GameMasterState
    {
        GameStart,
        PlayStart,
        GameEnd,
        GameTimeStop,
        GameTimeStart

    }


    [SerializeField] Player _player;

    [SerializeField] private BuffCardManager _buffCardManager;

    /// <summary>  PlayStartのSubject </summary>
    public Subject<Unit> PlayStart = new Subject<Unit>();

    /// <summary>  GameEndのSubject </summary>
    public Subject<Unit> GameEnd = new Subject<Unit>();

    /// <summary>  GameTimeStartのSubject </summary>
    public Subject<Unit> GameTimeStart = new Subject<Unit>();

    /// <summary>  GameTimeStopのSubject </summary>
    public Subject<Unit> GameTimeStop = new Subject<Unit>();



    private void Awake()
    {
        // すでにインスタンスがある → 破棄して重複を防ぐ
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // シーン切り替えでも破棄しない場合
        DontDestroyOnLoad(gameObject);

        SetTimerGaugeState(GameMasterState.GameStart);

        _player.JumpOneTime.Subscribe(_ =>
        {
            SetTimerGaugeState(GameMasterState.PlayStart);

        }).AddTo(this);

        _buffCardManager.CardCreateStart.Subscribe(_=> 
        {
            SetTimerGaugeState(GameMasterState.GameTimeStop);

        }).AddTo(this);

        _buffCardManager.CardSelectedEnd.Subscribe(_ =>
        {
            SetTimerGaugeState(GameMasterState.GameTimeStart);

        }).AddTo(this);


    }

    /// <summary>
    /// ステート
    /// </summary>
    /// <param name="timeScaleValue">タイムスケール変更値</param>
    public void SetTimerGaugeState(GameMasterState gameMasterState)
    {
        var state = gameMasterState;

        switch (state)
        {
            case GameMasterState.GameStart:

                break;

            case GameMasterState.PlayStart:
                Debug.Log("ここに");
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
