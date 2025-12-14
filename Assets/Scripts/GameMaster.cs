using UnityEngine;
using UniRx;

public class GameMaster : MonoBehaviour
{
    public static GameMaster Instance { get; private set; }

    public Subject<Unit> GameStart = new Subject<Unit>();

    public Subject<Unit> GameEnd = new Subject<Unit>();

    public Subject<Unit> DirectionEventStart = new Subject<Unit>();

    public Subject<Unit> DirectionEventEnd = new Subject<Unit>();

    public Subject<Unit> AttackEventStart = new Subject<Unit>();

    public Subject<Unit> AttackEventEnd = new Subject<Unit>();


    [SerializeField] Player _player;

    [SerializeField] Transform _playerTrans;

    [SerializeField] Transform _cameraTrans;


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

        Observable.EveryUpdate().Subscribe(_ =>
        {
            float dt = Time.deltaTime;
            // 毎フレームの処理
            Vector3 playerPos = _playerTrans.position;
            var updateCameraPos = new Vector3(playerPos.x + 3f, playerPos.y,-20f) ; 
            _cameraTrans.position = updateCameraPos;

        }).AddTo(this);

        GameMaster.Instance.GameStart.Subscribe(_ =>
        {
            Debug.Log("ゲームスタート");

        }).AddTo(this);

    }
    
}
