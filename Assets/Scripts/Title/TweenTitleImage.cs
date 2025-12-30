using UnityEngine;
using UniRx;
using DG.Tweening;
using System.Collections;
using Spine;
using Spine.Unity;

public class TweenTitleImage : MonoBehaviour
{
    public enum TweenTitleImageState
    {
        Default,
        In,
    }

    //public Subject<Unit> Default = new Subject<Unit>();

    [SerializeField] private TitleManager _titleManager;

    [SerializeField] private SkeletonGraphic _titleSpine;

    private DG.Tweening.Sequence _titleSequence;

    [SerializeField] private RectTransform _titleImageRect;

    private Vector2 _initTitleImagePos;

    private Vector3 _initTitleImageScale;

    private void Awake()
    {
        _initTitleImagePos = _titleImageRect.anchoredPosition;
        _initTitleImageScale = _titleImageRect.localScale;
        
        SetTweenTitleImageState(TweenTitleImageState.Default);
    }
    /// <summary>
    /// ステート
    /// </summary>
    private void SetTweenTitleImageState(TweenTitleImageState tweenTitleImageState)
    {
        var state = tweenTitleImageState;

        switch (state)
        {
            case TweenTitleImageState.Default:
                _titleImageRect.anchoredPosition = _initTitleImagePos;
                _titleImageRect.localScale = _initTitleImageScale;
                _titleSequence?.Kill();
                _titleSpine.AnimationState.SetAnimation(0, "default", false);

                SetTweenTitleImageState(TweenTitleImageState.In);

                break;

            case TweenTitleImageState.In:
                _titleSequence?.Kill();
                _titleSequence = DOTween.Sequence();
                _titleSequence.SetLink(gameObject);

                _titleImageRect.anchoredPosition = _initTitleImagePos;
                _titleImageRect.localScale = _initTitleImageScale;

                _titleSequence.InsertCallback(0.5f, () =>
                {
                    _titleSpine.AnimationState.SetAnimation(0, "in", false);

                });

                _titleSequence.Insert(1.5f, _titleImageRect.DOScale(0.6f, 1f).SetEase(Ease.OutExpo));
                _titleSequence.Insert(1.5f, _titleImageRect.DOAnchorPosY(190f, 1f).SetEase(Ease.OutExpo));
                _titleSequence.InsertCallback(1.5f, () =>
                {
                    _titleSpine.AnimationState.SetAnimation(0, "loop", false);

                });

                

                break;

        }
    }

    /// <summary>
    /// 呼び出し用。ベースクラス作ってもいいかも。
    /// </summary>
    public void PlauDefaultAnim()
    {
        SetTweenTitleImageState(TweenTitleImageState.In);
    }

    public void PlauInAnim()
    {
        SetTweenTitleImageState(TweenTitleImageState.In);
    }

}
