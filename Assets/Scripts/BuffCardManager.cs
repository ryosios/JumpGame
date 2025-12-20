using UnityEngine;
using UniRx;
using DG.Tweening;
using System.Collections;

public class BuffCardManager : MonoBehaviour
{
    //バフカード管理用

    public enum BuffCardManagerState
    {
        Default,
        CardCreate,
        CardSelect,
    }

    public Subject<BuffCard> CardCreated = new Subject<BuffCard>();

    [SerializeField] private BuffCard[] _buffCards;

    [SerializeField] private Transform _posRoot;

    private void Awake()
    {
        SeBuffCardManagerState(BuffCardManagerState.CardCreate);
    }
    /// <summary>
    /// ステート
    /// </summary>
    public void SeBuffCardManagerState(BuffCardManagerState baffCardManagerState)
    {
        var state = baffCardManagerState;

        switch (state)
        {
            case BuffCardManagerState.Default:         
                
                break;

            case BuffCardManagerState.CardCreate:
                CreateCard();

                break;

            case BuffCardManagerState.CardSelect:

                break;

        }
    }

    private void CreateCard()
    {
        BuffCard buffCard = Instantiate(_buffCards[0], _posRoot) as BuffCard;
        buffCard.gameObject.SetActive(true);
        CardCreated.OnNext(buffCard);
    }

}
