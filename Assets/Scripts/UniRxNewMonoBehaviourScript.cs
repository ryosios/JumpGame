using UnityEngine;
using UniRx;
using DG.Tweening;
using System.Collections;

public class UniRxNewMonoBehaviourScript : MonoBehaviour
{
    public enum ExsampleState
    {
        Default,
    }

    public Subject<Unit> Default = new Subject<Unit>();
    
    private void Awake()
    {

    }
    /// <summary>
    /// ステート
    /// テスト
    /// </summary>
    public void SetExsampleState(ExsampleState exsampleState)
    {
        var state = exsampleState;

        switch (state)
        {
            case ExsampleState.Default:         
                
                break;
           
        }
    }

}
