using UnityEngine;
using UniRx;

public class GameMaster : MonoBehaviour
{
    public static GameMaster Instance { get; private set; }

    public Subject<Unit> GameStart = new Subject<Unit>();

    public Subject<Unit> GameEnd = new Subject<Unit>();


    [SerializeField] Player _player;




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


    }
    
}
