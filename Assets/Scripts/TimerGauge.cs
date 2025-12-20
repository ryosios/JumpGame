using UnityEngine;
using UniRx;
using DG.Tweening;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class TimerGauge : MonoBehaviour
{
    public enum TimerGaugeState
    {
        TimerStop,
        TimerStart,
        TimerUpdate,

    }

    [SerializeField] private TextMeshProUGUI _timerText;
    private float _time;
    private bool _isRunning;

    

    private void Awake()
    {
        SetTimerValue(30f);
        SetTimerGaugeState(TimerGaugeState.TimerStart);

        Observable.EveryUpdate().Subscribe(_ =>
        {
            SetTimerGaugeState(TimerGaugeState.TimerUpdate);

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

            case TimerGaugeState.TimerUpdate:
                TimerUpdate();
                break;

        }
    }

    public void SetTimerValue(float time) 
    {
        _time = time;

    }
    private void TimerStart()
    {
        _isRunning = true;
    }
    private void TimerStop()
    {
        _isRunning = false;
    }

    private void TimerUpdate()
    {
        if (!_isRunning) return;

        _time -= Time.deltaTime;
        if (_time <= 0f)
        {
            _time = 0f;            
            _isRunning = false;
        }
        _timerText.text = Mathf.FloorToInt(_time).ToString();
    }
}
