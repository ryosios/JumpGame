using UnityEngine;
using UniRx;
using DG.Tweening;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class BuffLevelGauge : MonoBehaviour
{
    //敵カウント表示用クラス

    public enum BuffLevelGaugeState
    {
        Default,
        Update,
    }

    [SerializeField] private TextMeshProUGUI _buffLevelText;

    [SerializeField] private Player _player;

    private void Awake()
    {
        SetBuffLevelGaugeState(BuffLevelGaugeState.Default);

        _player.BuffLevelUpEnd.Subscribe(value => 
        {
            SetBuffLevelGaugeState(BuffLevelGaugeState.Update);
        });

    }
    /// <summary>
    /// ステート
    /// </summary>
    public void SetBuffLevelGaugeState(BuffLevelGaugeState buffLevelGaugeState)
    {
        var state = buffLevelGaugeState;

        switch (state)
        {
            case BuffLevelGaugeState.Default:
                SetBuffLevelGaugeValue(_player.GetBuffLevel());

                break;

            case BuffLevelGaugeState.Update:
                SetBuffLevelGaugeValue(_player.GetBuffLevel());

                break;

        }
    }

    /// <summary>
    /// バフレベルに数値をいれる
    /// </summary>
    /// <param name="value">更新する値</param>
    private void SetBuffLevelGaugeValue(int value)
    {
        _buffLevelText.text = value.ToString();
    }

}
