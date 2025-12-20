using UnityEngine;
using UniRx;
using DG.Tweening;
using System.Collections;

public class BuffCard : MonoBehaviour
{
    private enum BuffCardState
    {
        Default,
        CardSelect,
    }

    public Subject<BuffBase> CardSelected = new Subject<BuffBase>();

    /// <summary> バフ能力のスクリプタブルオブジェクト </summary>
    [SerializeField] private BuffBase _buffBase;

  
    /// <summary>
    /// ステート
    /// </summary>
    private void SetBuffCardState(BuffCardState buffCardState)
    {
        var state = buffCardState;

        switch (state)
        {
            case BuffCardState.Default:

                break;

            case BuffCardState.CardSelect:
                CardSelected.OnNext(_buffBase);

                break;


        }
    }

    public void SetStateCardSelect()
    {
        SetBuffCardState(BuffCardState.CardSelect);
    }

}
