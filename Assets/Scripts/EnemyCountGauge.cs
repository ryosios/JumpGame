using UnityEngine;
using UniRx;
using DG.Tweening;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class EnemyCountGauge : MonoBehaviour
{
    public enum EnemyCountGaugeState
    {
        Default,
        Update,
    }

    //public Subject<Unit> Default = new Subject<Unit>();

    [SerializeField] private TextMeshProUGUI _countText;

    [SerializeField] private EnemyCreater _enemyCreater;

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
    /// カウント数更新
    /// </summary>
    /// <param name="value">更新する値</param>
    private void SetEnemyCountGaugeValue(int value)
    {
        _countText.text = value.ToString();
    }

}
