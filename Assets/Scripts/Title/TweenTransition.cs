using UnityEngine;
using UniRx;
using DG.Tweening;
using System.Collections;
using UnityEngine.UI;
using System.Threading;
using Cysharp.Threading.Tasks;

public class TweenTransition : MonoBehaviour
{
    private enum ThisState
    {
        Default,
        In,
        Out,
    }

    //public Subject<Unit> Default = new Subject<Unit>();

    [SerializeField] private RectTransform _transitionRect;

    [SerializeField] private Image _transitionImage;

    [SerializeField] private CanvasGroup _transitionGroup;

    private Material _transitionMatrialInstance;

    private Sequence _thisSequence;

    private float _transitionTime = 0.5f;

    private float _transitionOutDelayTime = 0f;

    public Subject<Unit> InEnd = new Subject<Unit>();

    public Subject<Unit> OutEnd = new Subject<Unit>();


    private CancellationToken _destroyToken;

    private void Awake()
    {
        _destroyToken = this.GetCancellationTokenOnDestroy();
        _transitionMatrialInstance = Instantiate(_transitionImage.material);
        _transitionImage.material = _transitionMatrialInstance;

        SetThisState(ThisState.Default, _destroyToken).Forget();
    }

   

    /// <summary>
    /// ステート
    /// </summary>
    private async UniTask SetThisState(ThisState thisState,CancellationToken cancellationToken)
    {
        var state = thisState;

        switch (state)
        {
            case ThisState.Default:
                Time.timeScale = 1;
                _transitionGroup.alpha = 0;
                _transitionImage.enabled = false;

                break;

            case ThisState.Out:
                Debug.Log("タイトル開始-1");
                _transitionImage.enabled = true;
                _transitionRect.eulerAngles = new Vector3(0f, 0f, 0f);
                _transitionGroup.alpha = 1f;
                _thisSequence?.Kill();
                _thisSequence = DOTween.Sequence();
                _thisSequence.SetLink(gameObject);
                _transitionMatrialInstance.SetFloat("_Float", 0f);
                Debug.Log("タイトル開始0");

                _thisSequence.AppendInterval(_transitionOutDelayTime);
                _thisSequence.Append(DOVirtual.Float(0f, 0.6f, _transitionTime, value =>{_transitionMatrialInstance.SetFloat("_Float", value); }).SetEase(Ease.InCubic).SetLink(gameObject)).OnComplete(()=> 
                {
                    _transitionImage.enabled = false;
                    OutEnd.OnNext(Unit.Default);
                    Debug.Log("タイトル開始");

                });
                //非同期待機条件
                await _thisSequence.AsyncWaitForCompletion();
                break;

            case ThisState.In:
                _transitionImage.enabled = true;
                _transitionRect.eulerAngles = new Vector3(0f, 0f, 180f);
                _transitionGroup.alpha = 1f;
                _thisSequence?.Kill();
                _thisSequence = DOTween.Sequence();
                _thisSequence.SetLink(gameObject);
                _transitionMatrialInstance.SetFloat("_Float", 1f);
                
                _thisSequence.Append(DOVirtual.Float(0.6f, 0, _transitionTime, value => { _transitionMatrialInstance.SetFloat("_Float", value); }).SetEase(Ease.OutCubic).SetLink(gameObject)).OnComplete(() =>
                {
                    
                    InEnd.OnNext(Unit.Default);
                  
                });
                //非同期待機条件
                await _thisSequence.AsyncWaitForCompletion();
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

    public async UniTask PlayInAnim()
    {
        await SetThisState(ThisState.In, _destroyToken);
    }

    public async UniTask PlayOutAnim(float delayTime = 0f)
    {
        _transitionOutDelayTime = delayTime;
        await SetThisState(ThisState.Out,_destroyToken);
    }
}
