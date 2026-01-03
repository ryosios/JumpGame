using UnityEngine;
using UniRx;
using DG.Tweening;
using System.Collections;
using UnityEngine.UI;

public class TweenBuffCard : MonoBehaviour
{
    private enum ThisState
    {
        Default,
        In,
        Selected,
        Out,
    }

    //public Subject<Unit> Default = new Subject<Unit>();

    [SerializeField] RectTransform _rootRect;

    [SerializeField] CanvasGroup _rootGroup;

    [SerializeField] CanvasGroup _baseWhiteGroup;

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
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                SetThisState(ThisState.Selected);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                SetThisState(ThisState.Out);
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
                _rootGroup.alpha = 0;
                _baseWhiteGroup.alpha = 0;

                break;

            case ThisState.In:
                _sequence?.Kill();
                _sequence = DOTween.Sequence();
                _sequence.SetLink(gameObject);

                _rootGroup.alpha = 1;
                _rootRect.localScale = Vector3.one * 0.7f;
                _baseWhiteGroup.alpha = 0;

                _sequence.Insert(_delay,_rootRect.DOScale(1f,0.5f).SetEase(Ease.OutElastic)).SetUpdate(true);
               
                break;

            case ThisState.Selected:
                _sequence?.Kill();
                _sequence = DOTween.Sequence();
                _sequence.SetLink(gameObject);

                _rootGroup.alpha = 1;

                _rootRect.localScale = Vector3.one * 0.9f;
                _baseWhiteGroup.alpha = 1f;

                _sequence.Insert(_delay, _rootRect.DOScale(1f, 0.5f).SetEase(Ease.OutElastic)).SetUpdate(true);
                _sequence.Insert(_delay, _baseWhiteGroup.DOFade(0f, 1f).SetEase(Ease.Linear)).SetUpdate(true);
             

                break;

            case ThisState.Out:
                _sequence?.Kill();
                _sequence = DOTween.Sequence();
                _sequence.SetLink(gameObject);

                _rootGroup.alpha = 0;

                _rootRect.localScale = Vector3.one;
                _baseWhiteGroup.alpha = 0f;

               


                break;

        }
    }

    public void PlayInAnim(float delay = 0f)
    {
        _delay = delay;
        SetThisState(ThisState.In);

    }
}
