using UnityEngine;
using UniRx;
using UnityEngine.UI;
using UniRx.Triggers;

public class InputController : MonoBehaviour
{
    //入力管理クラス
    private enum InputState
    {
        CenterAreaButtonEnter,
        CenterAreaButtonExit,
        CenterAreaButtonActiveFalse,
        CenterAreaButtonActiveTrue,

    }
    [SerializeField] private GameMaster _gameMaster;

    public static InputController Instance { get; private set; }

    [SerializeField] private Button _centerAreaButton;

    public Subject<Unit> CenterAreaButtonEnter = new Subject<Unit>();

    public Subject<Unit> CenterAreaButtonExit = new Subject<Unit>();


    private bool _isPlayable = true;

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

        // 押した瞬間
        _centerAreaButton.OnPointerDownAsObservable()
            .Where(_ => _isPlayable == true)
            .Subscribe(_ =>
            {
                SetInputState(InputState.CenterAreaButtonEnter);
            })
            .AddTo(this);

        // 離した瞬間
        _centerAreaButton.OnPointerUpAsObservable()
            .Where(_ => _isPlayable == true)
            .Subscribe(_ =>
            {
                SetInputState(InputState.CenterAreaButtonExit);
            })
            .AddTo(this);

        _gameMaster.GameTimeStop.Subscribe(_ =>
        {
            SetInputState(InputState.CenterAreaButtonActiveFalse);

        }).AddTo(this);

        _gameMaster.GameTimeStart.Subscribe(_ =>
        {
            SetInputState(InputState.CenterAreaButtonActiveTrue);

        }).AddTo(this);

        _gameMaster.ResultStart.Subscribe(_ =>
        {
            SetInputState(InputState.CenterAreaButtonActiveFalse);

        }).AddTo(this);

        //開始時入力不可にしておく。
        SetInputState(InputState.CenterAreaButtonActiveFalse);
        _gameMaster.PlayStart.Subscribe(_=> 
        {//ゲームが開始したとき
            SetInputState(InputState.CenterAreaButtonActiveTrue);

        
        }).AddTo(this);

    }

    private void Update()
    {
        /*
        if (Input.GetMouseButtonDown(0))
        {
            SetInputState(InputState.LeftClickEnter);
        }
        if (Input.GetMouseButtonUp(0))
        {
            SetInputState(InputState.LeftClickExit);
        }
        */
         /*
        if (Input.GetMouseButtonDown(1))
        {
            SetInputState(InputState.RightClickEnter);
        }
        if (Input.GetMouseButtonUp(1))
        {
            SetInputState(InputState.RightClickExit);
        }
         */

    }

    private void SetInputState(InputState inputState)
    {
        var state = inputState;

        switch (state)
        {
            case InputState.CenterAreaButtonEnter:
                CenterAreaButtonEnter.OnNext(Unit.Default);

                break;

            case InputState.CenterAreaButtonExit:
                CenterAreaButtonExit.OnNext(Unit.Default);

                break;

            case InputState.CenterAreaButtonActiveFalse:
                //このBoolがTrueのときにクリック入力を通す
                _isPlayable = false;
                

                break;

            case InputState.CenterAreaButtonActiveTrue:
                _isPlayable = true;
           

                break;




        }
    }
}
