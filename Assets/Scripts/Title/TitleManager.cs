using UnityEngine;
using UniRx;
using DG.Tweening;
using System.Collections;
using Spine;
using Spine.Unity;

public class TitleManager : MonoBehaviour
{
    public enum TitleManagerState
    {
        Default,
        Title,
        GamePlay,
        GameEnd,

    }

    [SerializeField] TweenTitleImage _tweenTitleImage;

    private void Awake()
    {

        SetTitleManagerState(TitleManagerState.Default);

    }
    /// <summary>
    /// ステート
    /// </summary>
    public void SetTitleManagerState(TitleManagerState titleManagerStatee)
    {
        var state = titleManagerStatee;

        switch (state)
        {
            case TitleManagerState.Default:
                SetTitleManagerState(TitleManagerState.Title);

                break;

            case TitleManagerState.Title:
                _tweenTitleImage.PlauInAnim();
               
               

                break;

            case TitleManagerState.GamePlay:

                break;

            case TitleManagerState.GameEnd:

                break;

        }
    }



}
