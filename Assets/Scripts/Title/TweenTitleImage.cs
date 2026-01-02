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

    [SerializeField] private SkeletonGraphic _titleSpine;

    private DG.Tweening.Sequence _titleSequence;

    [SerializeField] private RectTransform _titleImageRect;

    [SerializeField] private CanvasGroup _titleImageGroup;

    private Vector2 _initTitleImagePos;

    private Vector3 _initTitleImageScale;

    public Subject<Unit> InEnd = new Subject<Unit>();

    private void Awake()
    {
        _initTitleImagePos = _titleImageRect.anchoredPosition;
        _initTitleImageScale = _titleImageRect.localScale;
        Debug.Log(_initTitleImageScale);
        
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
                _titleImageGroup.alpha = 0;
                _titleSequence?.Kill();
                _titleSpine.AnimationState.SetAnimation(0, "default", false);               

                break;

            case TweenTitleImageState.In:
                _titleSequence?.Kill();
                _titleSequence = DOTween.Sequence();
                _titleSequence.SetLink(gameObject);

                _titleImageGroup.alpha = 1;
                _titleImageRect.anchoredPosition = _initTitleImagePos;
                _titleImageRect.localScale = _initTitleImageScale;

                //0.5
                _titleSequence.InsertCallback(0.5f, () =>
                {             
                    var entry = _titleSpine.AnimationState.SetAnimation(0, "in", false);
                    entry.TimeScale = 2f; // 1.5倍速

                });

                //2.5
                _titleSequence.Insert(1.9f, _titleImageRect.DOScale(0.67f, 0.35f).SetEase(Ease.InExpo));
                _titleSequence.Insert(1.9f, _titleImageRect.DOAnchorPosY(190f, 0.35f).SetEase(Ease.InExpo));                

                break;

        }
    }

    /// <summary>
    /// 呼び出し用。ベースクラス作ってもいいかも。
    /// </summary>
    public void PlayDefaultAnim()
    {
        SetTweenTitleImageState(TweenTitleImageState.Default);
    }

    public void PlayInAnim()
    {
        SetTweenTitleImageState(TweenTitleImageState.In);
    }

}
