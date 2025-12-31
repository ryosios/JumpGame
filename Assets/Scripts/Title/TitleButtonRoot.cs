using UnityEngine;
using UniRx;
using DG.Tweening;
using System.Collections;
using UnityEngine.UI;
using UniRx.Triggers;
using System.Collections.Generic;

public class TitleButtonRoot : MonoBehaviour
{
    private enum TitleButtonRootState
    {
        Default,      
        SceneMove,
        GameQuit
    }

    private enum ButtonType
    {
        SelectStartButton,
        SelectQuitButton,
    }

    [SerializeField] Button _titleStartButton;

    [SerializeField] Button _quitButton;

    private ButtonType _selectedButtonType;

    private List<Button> _activeButtonList = new();

    [SerializeField] TweenTransition _tweenTransition;

    /// <summary>トランジションの時間</summary>
    private float _transitionTime = 1f;

    private void Awake()
    {
        //アクティブ用のリストに追加
        _activeButtonList.Add(_titleStartButton);
        _activeButtonList.Add(_quitButton);

        //購読
        _titleStartButton.OnPointerUpAsObservable().Subscribe(_ =>
        {
            _selectedButtonType = ButtonType.SelectStartButton;
            SetTitleButtonRootState(TitleButtonRootState.SceneMove);

        }).AddTo(this);

        _quitButton.OnPointerUpAsObservable().Subscribe(_ =>
        {
            _selectedButtonType = ButtonType.SelectQuitButton;
            SetTitleButtonRootState(TitleButtonRootState.GameQuit);

        }).AddTo(this);

        SetTitleButtonRootState(TitleButtonRootState.Default);

    }
    /// <summary>
    /// ステート
    /// </summary>
    private void SetTitleButtonRootState(TitleButtonRootState exsampleState)
    {
        var state = exsampleState;

        switch (state)
        {
            case TitleButtonRootState.Default:


                break;

            case TitleButtonRootState.SceneMove:
                if(_selectedButtonType == ButtonType.SelectStartButton)
                {                   
                    _tweenTransition.PlayInAnim();
                    StartCoroutine(SceneMove("GameScene", _tweenTransition.GetTransitionTime()));
                }
                else
                {
                    return;
                }

                break;

            case TitleButtonRootState.GameQuit:

                break;

        }
    }

    /// <summary>
    /// ステート
    /// </summary>
    /// <param name="sceneName"> シーン名 </param>
    /// <param name="delayTime">ディレイ時間</param>
    private IEnumerator SceneMove(string sceneName,float delayTime) 
    {
        yield return new WaitForSeconds(delayTime);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
