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
        PlayerSizeCountTextUpdate,
        PlayerSpeedCountTextUpdate,
    }

    [SerializeField] private TextMeshProUGUI _buffLevelText;

    [SerializeField] private Player _player;

    [SerializeField] private BuffCardManager _buffCardManager;

    [SerializeField] private TextMeshProUGUI _playerSizeCountText;

    [SerializeField] private TextMeshProUGUI _playerSpeedCountText;


    private void Awake()
    {
        SetBuffLevelGaugeState(BuffLevelGaugeState.Default);

        _player.BuffLevelUpEnd.Subscribe(value => 
        {
            SetBuffLevelGaugeState(BuffLevelGaugeState.Update);
        });

        _buffCardManager.CardCreated.Subscribe(buffCard =>
        {
            buffCard.CardSelectedBuffPlayerSize.Subscribe(buffPlayerSize =>
            {
                SetBuffLevelGaugeState(BuffLevelGaugeState.PlayerSizeCountTextUpdate);

            }).AddTo(this);

            buffCard.CardSelectedBuffPlayerSpeed.Subscribe(buffPlayerSpeed =>
            {
                SetBuffLevelGaugeState(BuffLevelGaugeState.PlayerSpeedCountTextUpdate);

            }).AddTo(this);

        }).AddTo(this);


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

            case BuffLevelGaugeState.PlayerSizeCountTextUpdate:
                
                int countTextSize = int.Parse(_playerSizeCountText.text);
                countTextSize += 1;
                _playerSizeCountText.text = countTextSize.ToString();

                break;

            case BuffLevelGaugeState.PlayerSpeedCountTextUpdate:

                int countTextSpeed = int.Parse(_playerSpeedCountText.text);
                countTextSpeed += 1;
                _playerSpeedCountText.text = countTextSpeed.ToString();

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
