using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class SutaminaSlider : MonoBehaviour
{
    public enum SutaminaState
    {
        Default,
        ValueChange
    }


    private float _sutaminaValue;

    [SerializeField] private Slider _slider;
    [SerializeField] private Player _player;

  

    private void Awake()
    {
        SetSutaminaState(SutaminaState.Default,1);

        _player.SutaminaChange.Subscribe(value =>
        {
           
            SetSutaminaState(SutaminaState.ValueChange, value);


        }).AddTo(this);
    }
  

    public void SetSutaminaState(SutaminaState sutaminaState,float changedValue)
    {
        var state = sutaminaState;

        switch (state)
        {
            case SutaminaState.Default:
              

                break;

            case SutaminaState.ValueChange:
                Debug.Log("sutamina1"+changedValue);
                SetSliderValue(changedValue);

                break;
        }

    }

    /// <summary>
    /// スライダーに値を代入する
    /// </summary>
    /// <param name="value">代入する値</param>
    private void SetSliderValue(float value)
    {
        _slider.value = value;
    }
}
