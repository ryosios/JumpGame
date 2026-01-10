using UnityEngine;
using UniRx;
using DG.Tweening;
using System.Collections;
using UnityEngine.UI;

public class TweenResultRoot : MonoBehaviour
{
    private enum ThisState
    {
        Default,
        In,

    }

    public Subject<Unit> OutEnd = new Subject<Unit>();

    [SerializeField] CanvasGroup _thisGroup;

    [SerializeField] RectTransform _resultTitleRect;

    [SerializeField] CanvasGroup _resultTitleGroup;

    [SerializeField] CanvasGroup _whiteGroup;

    [SerializeField] ParticleSystem _effect1;

    [SerializeField] ParticleSystem _effect2;

    private Sequence _sequence;

    private float _delay = 0f;


    public bool _isDebug;

    private void Awake()
    {
        SetThisState(ThisState.Default);
    }

    private void Update()
    {
        if (_isDebug)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                SetThisState(ThisState.Default);
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                SetThisState(ThisState.In);
            }

        }
    }

    /// <summary>
    /// ステート
    /// </summary>
    private void SetThisState(ThisState thisState)
    {
        var state = thisState;

        switch (state)
        {
            case ThisState.Default:
                _sequence?.Kill();
                _thisGroup.alpha = 0;
                _resultTitleGroup.alpha = 0f;
                _whiteGroup.alpha = 0f;

                break;

            case ThisState.In:
                _sequence?.Kill();
                _sequence = DOTween.Sequence();
                _sequence.SetLink(gameObject);

                _thisGroup.alpha = 1;
                _resultTitleGroup.alpha = 0f;
                _resultTitleRect.localScale = Vector3.one * 0.7f;
                _whiteGroup.alpha = 0f;


                _sequence.InsertCallback(_delay, () =>
                {
                    _effect1.Play();


                }).SetUpdate(true);

                _sequence.Insert(_delay + 0.2f, _resultTitleGroup.DOFade(1f, 0.1f).SetEase(Ease.OutExpo)).SetUpdate(true);
                _sequence.Insert(_delay + 0.2f, _whiteGroup.DOFade(1f, 0.1f).SetEase(Ease.OutExpo)).SetUpdate(true);
                _sequence.Insert(_delay + 0.2f, _resultTitleRect.DOScale(1f,0.7f).SetEase(Ease.OutElastic)).SetUpdate(true);

                _sequence.Insert(_delay + 0.3f, _whiteGroup.DOFade(0f, 0.5f).SetEase(Ease.OutSine)).SetUpdate(true);
                _sequence.InsertCallback(_delay + 0.3f, () =>
                {
                    _effect2.Play();


                }).SetUpdate(true);

                break;

           

        }
    }

    public void PlayInAnim(float delay = 0f)
    {
        _delay = delay;
        SetThisState(ThisState.In);

    }

}
