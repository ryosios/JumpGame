using UnityEngine;
using UniRx;
using DG.Tweening;
using System.Collections;

public class CommonTweenAnimation : MonoBehaviour
{
    public enum CommonTweenAnimationState
    {
        Default,
        Scale,
    }

    // public Subject<Unit> Default = new Subject<Unit>();

    [SerializeField] private float _startScale = 0.3f;

    [SerializeField] private float _time = 0.8f;

    private Sequence _sequance;

    private float _initScale;

    private void Awake()
    {
        _initScale = this.transform.localScale.x;
        _sequance = DOTween.Sequence();
        SetCommonTweenAnimationState(CommonTweenAnimationState.Scale);
        
    }
    /// <summary>
    /// ステート
    /// </summary>
    public void SetCommonTweenAnimationState(CommonTweenAnimationState commonTweenAnimationState)
    {
        var state = commonTweenAnimationState;

        switch (state)
        {
            case CommonTweenAnimationState.Default:         
                
                break;

            case CommonTweenAnimationState.Scale:
                this.transform.localScale = new Vector3(_startScale, _startScale);
                _sequance.Append(this.transform.DOScale(_initScale, _time).SetEase(Ease.OutQuart)).SetLoops(-1,LoopType.Yoyo).SetLink(gameObject) ;

                break;

        }
    }

}
