using UnityEngine;
using UniRx;
using DG.Tweening;
using System.Collections;
using UnityEngine.UI;

public class TweenTransition : MonoBehaviour
{
    private enum ThisState
    {
        Default,
        In,
    }

    //public Subject<Unit> Default = new Subject<Unit>();

    [SerializeField] private Image _transitionImage;

    [SerializeField] private CanvasGroup _transitionGroup;

    private Material _transitionMatrialInstance;

    private Sequence _thisSequence;

    private float _transitionTime = 0.5f;

    private void Awake()
    {
        _transitionMatrialInstance = Instantiate(_transitionImage.material);
        _transitionImage.material = _transitionMatrialInstance;

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
                _transitionGroup.alpha = 0;

                break;

            case ThisState.In:
                _transitionGroup.alpha = 1f;
                _thisSequence?.Kill();
                _thisSequence = DOTween.Sequence();
                _thisSequence.SetLink(gameObject);

                _thisSequence.Append(DOVirtual.Float(0.6f, 0, _transitionTime, value =>{_transitionMatrialInstance.SetFloat("_Float", value); }).SetEase(Ease.OutCubic).SetLink(gameObject));
               

                break;

        }
    }

    /// <summary>
    /// トランジションの時間を返す
    /// </summary>
    public float GetTransitionTime()
    {
        return _transitionTime;
    }

    public void PlayInAnim()
    {
        SetThisState(ThisState.In);
    }
}
