using UnityEngine;
using UniRx;
using DG.Tweening;
using System.Collections;
using Spine;
using Spine.Unity;
using UnityEngine.UI;
using UniRx.Triggers;
using Cysharp.Threading.Tasks;
using System.Threading;

public class TitleManager : MonoBehaviour
{
    public enum TitleManagerState
    {
        Default,
        Title,
        GamePlay,
        GameEnd,

    }
    /// <summary> TitleImage </summary>
    [SerializeField] TweenTitleImage _tweenTitleImage;
    [SerializeField] TweenTitleButtonRoot _tweenTitleButtonRoot;

    [SerializeField] TweenTransition _tweenTransition;

    /// <summary> オーディオ再生管理コンポーネント </summary>
    [SerializeField] AudioManager _audioManager;

    /// <summary> タイトルBGM </summary>
    [SerializeField] AudioClipHolder _audioTitleBGMHolder;

    private void Awake()
    {
        
    }

    private void Start()
    {
        SetTitleManagerState(TitleManagerState.Default).Forget();
    }

    /// <summary>
    /// ステート
    /// </summary>
    public async UniTask SetTitleManagerState(TitleManagerState titleManagerState)
    {
        var state = titleManagerState;

        switch (state)
        {
            case TitleManagerState.Default:
                
                //タイトル遷移前にトランジションとかで少し待つ
                _tweenTitleImage.PlayDefaultAnim();
                await _tweenTransition.PlayOutAnim();

                SetTitleManagerState(TitleManagerState.Title).Forget();

                break;

            case TitleManagerState.Title:
                _tweenTitleImage.PlayInAnim();
                _tweenTitleButtonRoot.PlayInAnim(1.7f);
                _audioManager.PlayMusic(_audioTitleBGMHolder.HolderClip[0], _audioTitleBGMHolder.HolderAudio, true);


                break;

            case TitleManagerState.GamePlay:

                break;

            case TitleManagerState.GameEnd:

                break;

        }
    }



}
