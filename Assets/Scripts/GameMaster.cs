using UnityEngine;
using UniRx;

public class GameMaster : MonoBehaviour
{
    public static GameMaster Instance { get; private set; }

    public enum GameMasterState
    {
        GameStart,
        GameEnd,
        GameTimeChange

    }


    [SerializeField] Player _player;

    /// <summary>  EnemyCollisionEnterのSubject </summary>
    public Subject<Unit> GameStart = new Subject<Unit>();

    /// <summary>  EnemyCollisionEnterのSubject </summary>
    public Subject<Unit> GameEnd = new Subject<Unit>();

    /// <summary>  EnemyCollisionEnterのSubject </summary>
    public Subject<Unit> GameTimeChange = new Subject<Unit>();


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
    }

    /// <summary>
    /// ステート
    /// </summary>
    /// <param name="timeScaleValue">タイムスケール変更値</param>
    public void SetTimerGaugeState(GameMasterState gameMasterState ,float timeScaleValue = 1f)
    {
        var state = gameMasterState;

        switch (state)
        {
            case GameMasterState.GameStart:

                break;

            case GameMasterState.GameEnd:
                
                break;

            case GameMasterState.GameTimeChange:
                ChangeTimeScale(timeScaleValue);
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
