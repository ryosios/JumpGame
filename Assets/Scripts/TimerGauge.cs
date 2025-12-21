using UnityEngine;
using UniRx;
using DG.Tweening;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class TimerGauge : MonoBehaviour
{
    //タイマー管理用クラス

    public enum TimerGaugeState
    {
        TimerStop,
        TimerStart,
      

    }

    public Subject<Unit> Default = new Subject<Unit>();

    [SerializeField] private TextMeshProUGUI _countText;
    private float _time = 0f;
    private bool _isRunning = false;


    [SerializeField] BuffCardManager _buffCardManager;

    private BuffCard _buffCard;

    

    private void Awake()
    {
        SetTimerValue(30f);
        SetTimerGaugeState(TimerGaugeState.TimerStart);

        _buffCardManager?.CardCreated.Subscribe(buffCard=> 
        {
            _buffCard = buffCard;
            

            _buffCard?.CardSelectedBuffAddTime.Subscribe(buffAddTime =>
            {
                
                SetTimerValueAdd(buffAddTime._addTimeValue);
               
                
            }).AddTo(this);

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
            }
            if (_countText != null)
            {
                _countText.text = Mathf.FloorToInt(_time).ToString();
            }
        }

       
        
    }
}
