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

    private void Awake()
    {
       


        SetTitleManagerState(TitleManagerState.Default);

    }

    private void Start()
    {
        SetTitleManagerState(TitleManagerState.Title);
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
                

                break;

            case TitleManagerState.Title:
                _tweenTitleImage.PlayInAnim();
                _tweenTitleButtonRoot.PlayInAnim(2.2f);
               
               

                break;

            case TitleManagerState.GamePlay:

                break;

            case TitleManagerState.GameEnd:

                break;

        }
    }



}
