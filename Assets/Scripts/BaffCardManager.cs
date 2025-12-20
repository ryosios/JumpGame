using UnityEngine;
using UniRx;
using DG.Tweening;
using System.Collections;

public class BaffCardManager : MonoBehaviour
{
    //バフカード管理用

    public enum BaffCardManagerState
    {
        Default,
        CardCreate,
    }

    //public Subject<Unit> Default = new Subject<Unit>();
    
    private void Awake()
    {

    }
    /// <summary>
    /// ステート
    /// </summary>
    public void SeBaffCardManagerState(BaffCardManagerState baffCardManagerState)
    {
        var state = baffCardManagerState;

        switch (state)
        {
            case BaffCardManagerState.Default:         
                
                break;

            case BaffCardManagerState.CardCreate:

                break;

        }
    }

}
