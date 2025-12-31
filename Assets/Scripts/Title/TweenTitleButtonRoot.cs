using UnityEngine;
using UniRx;
using DG.Tweening;
using System.Collections;
using UnityEngine.UI;

public class TweenTitleButtonRoot : MonoBehaviour
{
    private enum ThisState
    {
        Default,
        In,
    }

    //public Subject<Unit> Default = new Subject<Unit>();

    [SerializeField] private RectTransform[] _buttonRects;

    [SerializeField] private CanvasGroup[] _buttonGroups;

    [SerializeField] private Button[] _buttonButtons;

    private Sequence[] _sequences;

    private void Awake()
    {

        _sequences = new Sequence[2];
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
                for (int i = 0; i < 2; i++)
                {
                    _sequences[i]?.Kill();
                }
                foreach (var buttonRect in _buttonRects)
                {
                    buttonRect.localScale = Vector3.zero;                   
                }
                foreach (var buttonGroup in _buttonGroups)
                {
                    buttonGroup.alpha = 0f;
                }
                foreach (var buttonButton in _buttonButtons)
                {
                    buttonButton.enabled = false;
                }

                break;

            case ThisState.In:

                for (int i = 0; i < _buttonRects.Length; i++)
                {
                    _sequences[i]?.Kill();
                    _sequences[i] = DOTween.Sequence();
                    _sequences[i].SetLink(gameObject);
                }              


                foreach (var buttonRect in _buttonRects)
                {
                    buttonRect.localScale = Vector3.zero;
                    _sequences[0].Insert(0f,buttonRect.DOScale(Vector3.one,1f).SetEase(Ease.OutElastic));
                    _sequences[0].AppendInterval(0.2f);
                }
                foreach (var buttonGroup in _buttonGroups)
                {
                    buttonGroup.alpha = 0f;
                    _sequences[1].Insert(0f, buttonGroup.DOFade(1f, 0.2f).SetEase(Ease.OutExpo));
                    _sequences[1].AppendInterval(0.2f);
                }
                foreach (var buttonButton in _buttonButtons)
                {                   
                   buttonButton.enabled = true;
                }

                break;

        }
    }

    /// <summary>
    /// Inを再生
    /// </summary>
    public void PlayInAnim(float delayTime)
    {
        StartCoroutine(InAnim(delayTime));
    }

    /// <summary>
    /// InAnim(内部用)
    /// </summary>
    private IEnumerator InAnim(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        SetThisState(ThisState.In);

    }
}
