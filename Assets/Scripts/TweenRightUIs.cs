using UnityEngine;
using UniRx;
using DG.Tweening;
using System.Collections;

public class TweenRightUIs : MonoBehaviour
{
    private enum ThisState
    {
        Default,
        In,
    }

    //public Subject<Unit> Default = new Subject<Unit>();

    /// <summary> スライド距離 </summary>
    private float _outSlideDistancey = 300f;

    /// <summary> アウトアニメーションのスタート時のディレイ </summary>
    private float _outStartDelay = 0;

    private Sequence _sequence;

    [SerializeField] private RectTransform[] _uiPartsRects;
    [SerializeField] private CanvasGroup[] _uiPartsGroups;

    private void Awake()
    {
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
                foreach (var uiPartsGroup in _uiPartsGroups)
                {
                    uiPartsGroup.alpha = 0f;
                }


                break;

            case ThisState.In:
                _sequence?.Kill();
                _sequence = DOTween.Sequence();
                _sequence.SetLink(gameObject);

                foreach (var uiPartsRect in _uiPartsRects)
                {
                    uiPartsRect.anchoredPosition = new Vector2(585f + _outSlideDistancey, uiPartsRect.anchoredPosition.y);
                }
                foreach (var uiPartsGroup in _uiPartsGroups)
                {
                    uiPartsGroup.alpha = 0f;
                }

                float time = _outStartDelay;

                for (int i = 0; i < _uiPartsRects.Length; i++)
                {
                    _sequence.Insert(time + i * 0.05f, _uiPartsRects[i].DOAnchorPosX(585f, 0.35f).SetEase(Ease.OutExpo));
                    _sequence.Insert(time + i * 0.05f, _uiPartsGroups[i].DOFade(1f, 0.35f).SetEase(Ease.OutExpo));
                    
                }

                break;

        }
    }

    public void PlayInAnim(float delay = 0) 
    {
        _outStartDelay = delay;
        SetThisState(ThisState.In);
    }

}
