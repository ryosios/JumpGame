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
        PlayerBounceCountTextUpdate,
        AddTimeCountTextUpdate,
        EnemyCountCountTextUpdate,
        MaxSelectCardCountTextUpdate,
        StaminaRecoveryCountTextUpdate,
    }

    [SerializeField] private TextMeshProUGUI _buffLevelText;

    [SerializeField] private Player _player;

    [SerializeField] private BuffCardManager _buffCardManager;

    [SerializeField] private TextMeshProUGUI _playerSizeCountText;

    [SerializeField] private TextMeshProUGUI _playerSpeedCountText;

    [SerializeField] private TextMeshProUGUI _playerBounceCountText;

    [SerializeField] private TextMeshProUGUI _addTimeCountText;

    [SerializeField] private TextMeshProUGUI _enemyCountCountText;

    [SerializeField] private TextMeshProUGUI _maxSelectCardCountText;

    [SerializeField] private TextMeshProUGUI _staminaRecoveryCountText;


    private void Awake()
    {
        SetBuffLevelGaugeState(BuffLevelGaugeState.Default);

        _buffCardManager._buffLevel.Subscribe(value => 
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

            buffCard.CardSelectedBuffPlayerBounce.Subscribe(buffPlayerBounce =>
            {
                SetBuffLevelGaugeState(BuffLevelGaugeState.PlayerBounceCountTextUpdate);

            }).AddTo(this);

            buffCard.CardSelectedBuffAddTime.Subscribe(buffAddTimeBounce =>
            {
                SetBuffLevelGaugeState(BuffLevelGaugeState.AddTimeCountTextUpdate);

            }).AddTo(this);

            buffCard.CardSelectedBuffEnemyCount.Subscribe(buffEnemyCount =>
            {
                SetBuffLevelGaugeState(BuffLevelGaugeState.EnemyCountCountTextUpdate);

            }).AddTo(this);

            buffCard.CardSelectedBuffMaxSelectCard.Subscribe(buffMaxSelectCardCount =>
            {
                SetBuffLevelGaugeState(BuffLevelGaugeState.MaxSelectCardCountTextUpdate);

            }).AddTo(this);

            buffCard.CardSelectedBuffStaminaRecovery.Subscribe(buffStaminaRecoveryCardCount =>
            {
                SetBuffLevelGaugeState(BuffLevelGaugeState.StaminaRecoveryCountTextUpdate);

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
                SetBuffLevelGaugeValue(_buffCardManager._buffLevel.Value);

                break;

            case BuffLevelGaugeState.Update:
                SetBuffLevelGaugeValue(_buffCardManager._buffLevel.Value);

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

            case BuffLevelGaugeState.PlayerBounceCountTextUpdate:

                int countTextBounce = int.Parse(_playerBounceCountText.text);
                countTextBounce += 1;
                _playerBounceCountText.text = countTextBounce.ToString();

                break;

            case BuffLevelGaugeState.AddTimeCountTextUpdate:

                int countTextTime = int.Parse(_addTimeCountText.text);
                countTextTime += 1;
                _addTimeCountText.text = countTextTime.ToString();

                break;

            case BuffLevelGaugeState.EnemyCountCountTextUpdate:

                int countTextEnemyCount = int.Parse(_enemyCountCountText.text);
                countTextEnemyCount += 1;
                _enemyCountCountText.text = countTextEnemyCount.ToString();

                break;

            case BuffLevelGaugeState.MaxSelectCardCountTextUpdate:

                int countTextMaxSelectCard = int.Parse(_maxSelectCardCountText.text);
                countTextMaxSelectCard += 1;
                _maxSelectCardCountText.text = countTextMaxSelectCard.ToString();

                break;

            case BuffLevelGaugeState.StaminaRecoveryCountTextUpdate:

                int countTextStaminaRecovery = int.Parse(_staminaRecoveryCountText.text);
                countTextStaminaRecovery += 1;
                _staminaRecoveryCountText.text = countTextStaminaRecovery.ToString();

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
