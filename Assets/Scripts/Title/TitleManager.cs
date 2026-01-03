using UnityEngine;
using UniRx;
using DG.Tweening;
using System.Collections;
using Spine;
using Spine.Unity;
using UnityEngine.UI;
using UniRx.Triggers;

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

    private void Awake()
    {
        
    }

    private void Start()
    {
        SetTitleManagerState(TitleManagerState.Default);
    }

    /// <summary>
    /// ステート
    /// </summary>
    public void SetTitleManagerState(TitleManagerState titleManagerState)
    {
        var state = titleManagerState;

        switch (state)
        {
            case TitleManagerState.Default:
                //タイトル遷移前にトランジションとかで少し待つ
                _tweenTitleImage.PlayDefaultAnim();
                _tweenTransition.PlayOutAnim();
                _tweenTransition.OutEnd.Subscribe(_ =>
                {
                    SetTitleManagerState(TitleManagerState.Title);


                }).AddTo(this);


                break;

            case TitleManagerState.Title:
                _tweenTitleImage.PlayInAnim();
                _tweenTitleButtonRoot.PlayInAnim(1.7f);
               



                break;

            case TitleManagerState.GamePlay:

                break;

            case TitleManagerState.GameEnd:

                break;

        }
    }



}
