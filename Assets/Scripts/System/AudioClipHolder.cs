using UnityEngine;
using UniRx;
using DG.Tweening;
using System.Collections;

public class AudioClipHolder : MonoBehaviour
{
    [SerializeField] private AudioClip[] _clip;
    public AudioClip[] HolderClip => _clip;

    [SerializeField] private AudioSource _audio;
    public AudioSource HolderAudio => _audio;

}
