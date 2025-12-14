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

    public Subject<float> SutaminaValueChange = new Subject<float>();

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

    private void SetSliderValue(float value)
    {
        var sv = _slider.value;
        _slider.value = sv + value;
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
}
