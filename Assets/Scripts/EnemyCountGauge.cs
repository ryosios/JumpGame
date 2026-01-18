using UnityEngine;
using UniRx;
using DG.Tweening;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class EnemyCountGauge : MonoBehaviour
{
    //敵カウント表示用クラス

    public enum EnemyCountGaugeState
    {
        Default,
        Update,
    }

    //public Subject<Unit> Default = new Subject<Unit>();

    [SerializeField] private TextMeshProUGUI _countText;

    [SerializeField] private EnemyCreater _enemyCreater;

    /// <summary>　ポイント係数（変動予定） </summary>
    private float _pointRatio = 1f;

    /// <summary>　現在のポイント：敵の数×ポイント係数 </summary>
    private float _currentPoint = 0f;

    private int _beforeCount = 0;

    private void Awake()
    {
        _enemyCreater.EnemyKillCount.Subscribe(value => 
        {
            SetEnemyCountGaugeState(EnemyCountGaugeState.Update,value);
        });

    }
    /// <summary>
    /// ステート
    /// </summary>
    /// <param name="value">代入する値</param>
    public void SetEnemyCountGaugeState(EnemyCountGaugeState enemyCountGaugeState, int value)
    {
        var state = enemyCountGaugeState;

        switch (state)
        {
            case EnemyCountGaugeState.Default:
                SetEnemyCountGaugeValue(0);
                break;

            case EnemyCountGaugeState.Update:
                SetEnemyCountGaugeValue(value);

                break;

        }
    }

    /// <summary>
    /// カウントをポイント計算して更新
    /// </summary>
    /// <param name="value">敵の数</param>
    private void SetEnemyCountGaugeValue(int value)
    {
        int updateCount = value;
        int diff = updateCount - _beforeCount;
        float updatePoint = diff* _pointRatio;
        _currentPoint += updatePoint;
        int currentInt = (int)_currentPoint;//表示はintにして少数切り捨て
        _countText.text = currentInt.ToString();
        _beforeCount = updateCount;
    }

    /// <summary>
    /// 現在のポイントを取得
    /// </summary>

    public int GetCurrentPoint()
    {
        return (int)_currentPoint;//表示はintにして少数切り捨て
    }

}
