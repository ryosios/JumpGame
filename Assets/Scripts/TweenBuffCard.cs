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
    }

    //public Subject<Unit> Default = new Subject<Unit>();

    [SerializeField] RectTransform _rootRect;

    [SerializeField] CanvasGroup _rootGroup;

    private Vector2 _intRootPos = new Vector2(0,0);

    private float _ySlideValue = 100f;

    private float _xSlideValue = 100f;

    private Sequence _sequence;

    private float _delay = 0f;
    
    private void Awake()
    {
        _intRootPos = new Vector2(0f, 500f);
        SetThisState(ThisState.Default);
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
                
                break;

            case ThisState.In:
                _sequence?.Kill();
                _sequence = DOTween.Sequence();
                _sequence.SetLink(gameObject);

                _rootGroup.alpha = 0;

                _rootRect.anchoredPosition = _intRootPos;
                
                Debug.Log(_intRootPos);
                Debug.Log(_rootRect.anchoredPosition);
                _sequence.Insert(_delay,_rootRect.DOAnchorPosY(0f,0.5f).SetEase(Ease.OutExpo)).SetUpdate(true);
                _sequence.Insert(_delay, _rootGroup.DOFade(1f, 0.5f).SetEase(Ease.OutExpo)).SetUpdate(true);

                break;

        }
    }

    public void PlayInAnim(float delay = 0f)
    {
        _delay = delay;
        SetThisState(ThisState.In);

    }
}
