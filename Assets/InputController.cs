using UnityEngine;
using UniRx;
using UnityEngine.UI;
using UniRx.Triggers;

public class InputController : MonoBehaviour
{
    private enum InputState
    {
        LeftClickEnter,
        LeftClickExit,
        RightClickEnter,
        RightClickExit,
    }

    public static InputController Instance { get; private set; }

    [SerializeField] private Button _leftAreaButton;

    [SerializeField] private ObservableEventTrigger _leftAreaButtonTrigger;

    [SerializeField] private Button _rightAreaButton;

    [SerializeField] private ObservableEventTrigger _rightAreaButtonTrigger;

    public Subject<Unit> LeftClickEnter = new Subject<Unit>();

    public Subject<Unit> LeftClickExit = new Subject<Unit>();

    public Subject<Unit> RightClickEnter = new Subject<Unit>();

    public Subject<Unit> RightClickExit = new Subject<Unit>();

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

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SetInputState(InputState.LeftClickEnter);
        }
        if (Input.GetMouseButtonUp(0))
        {
            SetInputState(InputState.LeftClickExit);
        }
        if (Input.GetMouseButtonDown(1))
        {
            SetInputState(InputState.RightClickEnter);
        }
        if (Input.GetMouseButtonUp(1))
        {
            SetInputState(InputState.RightClickExit);
        }

    }

    private void SetInputState(InputState inputState)
    {
        var state = inputState;

        switch (state)
        {
            case InputState.LeftClickEnter:
                LeftClickEnter.OnNext(Unit.Default);

                break;

            case InputState.LeftClickExit:
                LeftClickExit.OnNext(Unit.Default);

                break;

            case InputState.RightClickEnter:
                RightClickEnter.OnNext(Unit.Default);

                break;

            case InputState.RightClickExit:
                RightClickExit.OnNext(Unit.Default);

                break;


        }
    }
}
