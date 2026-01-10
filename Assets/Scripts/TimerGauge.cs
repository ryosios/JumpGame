using UnityEngine;
using UniRx;
using DG.Tweening;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class TimerGauge : MonoBehaviour
{
    //時間管理用クラス

    public enum TimerGaugeState
    {
        TimerStop,
        TimerStart,
        TimerZero,


    }

    public Subject<Unit> Default = new Subject<Unit>();

    public Subject<Unit> TimerZero = new Subject<Unit>();

    /// <summary> GameMaster </summary>
    [SerializeField] private GameMaster _gameMaster;

    [SerializeField] private TextMeshProUGUI _countText;

    private float _time = 0f;

    private bool _isRunning = false;


    [SerializeField] BuffCardManager _buffCardManager;

    private BuffCard _buffCard;

    

    private void Awake()
    {
        SetTimerValue(30f);
        SetTimerGaugeState(TimerGaugeState.TimerStop);

        //各バフカードが全部流れてくる
        _buffCardManager?.CardCreated.Subscribe(buffCard=> 
        {
            _buffCard = buffCard;
            

            _buffCard?.CardSelectedBuffAddTime.Subscribe(buffAddTime =>
            {
                
                SetTimerValueAdd(buffAddTime._addTimeValue);
               
                
            }).AddTo(this);            

        }).AddTo(this);

        //時間停止と一緒にタイマーも止める
        _gameMaster.GameTimeStop.Subscribe(_ =>
        {
            SetTimerGaugeState(TimerGaugeState.TimerStop);

        }).AddTo(this);

        //解除
        _gameMaster.GameTimeStart.Subscribe(_ =>
        {
            SetTimerGaugeState(TimerGaugeState.TimerStart);

        }).AddTo(this);

        _gameMaster.PlayStart.Subscribe(_ =>
        {
            
            SetTimerGaugeState(TimerGaugeState.TimerStart);


        }).AddTo(this);
    }
    private void Start()
    {
        Observable.EveryUpdate().Subscribe(_ =>
        {
            TimerUpdate();

        }).AddTo(this);

      
    }

    /// <summary>
    /// ステート
    /// </summary>
    public void SetTimerGaugeState(TimerGaugeState timerGaugeState)
    {
        var state = timerGaugeState;

        switch (state)
        {
            case　TimerGaugeState.TimerStop:
                TimerStop();
                break;

            case TimerGaugeState.TimerStart:
                TimerStart();
                break;

            case TimerGaugeState.TimerZero:
                //Timeが0になったとき
                TimerZero.OnNext(Unit.Default);
                
                break;



        }
    }

    /// <summary>
    /// ゲージに値をセット
    /// </summary>
    public void SetTimerValue(float time) 
    {
        _time = time;

    }

    /// <summary>
    /// ゲージに値を加算
    /// </summary>
    public void SetTimerValueAdd(float time)
    {
        _time += time;

    }

    /// <summary>
    /// タイマー開始
    /// </summary>
    private void TimerStart()
    {
        _isRunning = true;
    }

    /// <summary>
    /// タイマーストップ
    /// </summary>
    private void TimerStop()
    {
        _isRunning = false;
    }

    /// <summary>
    /// タイマー更新
    /// </summary>
    private void TimerUpdate()
    {
        if (_isRunning)
        {
            _time -= Time.deltaTime;

            if (_time <= 0f)
            {
                _time = 0f;
                _isRunning = false;
               
                SetTimerGaugeState(TimerGaugeState.TimerZero);
            }
            if (_countText != null)
            {
                _countText.text = Mathf.FloorToInt(_time).ToString();
            }
        }

       
        
    }
}
