using UnityEngine;
using UniRx;
using DG.Tweening;
using System.Collections;

public class AudioManager : MonoBehaviour
{

    private enum ThisState
    {
        Default,
        Play,
        Stop
       
    }

    //public Subject<Unit> Default = new Subject<Unit>();

    private void Awake()
    {
       
    }
    /// <summary>
    /// ステート
    /// </summary>
    private void SetThisState(ThisState thisState, AudioClip audioClip = null, AudioSource audioSource = null,bool isLoop = false)
    {
        var state = thisState;

        switch (state)
        {
            case ThisState.Default:         
                
                break;
            case ThisState.Play:
                audioSource.clip = audioClip;
                audioSource.loop = isLoop;
                audioSource.Play();
                break;
            case ThisState.Stop:
                audioSource.Stop();
                break;

        }
    }

    /// <summary>
    /// 音声再生メソッド
    /// </summary>
    public void PlayMusic(AudioClip audioClip , AudioSource audioSource ,bool isLoop = false)
    {
        SetThisState(ThisState.Play, audioClip, audioSource, isLoop);
    }

    /// <summary>
    /// 音声停止メソッド
    /// </summary>
    public void StopMusic(AudioSource audioSource)
    {
        SetThisState(ThisState.Play, null, audioSource);
    }
}
