using UnityEngine;
using UniRx;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

public class BuffCardManager : MonoBehaviour
{
    //バフカードプレファブ管理用

    public enum BuffCardManagerState
    {
        Default,
        CardCreate,
        CardSelected,
    }

    public Subject<BuffCard> CardCreated = new Subject<BuffCard>();
    

    [SerializeField] private Transform _posRoot;

    [SerializeField] private BuffCard _buffCard;

    private int _maxSelectCardValue = 3;

    private List<BuffCard> _activeCardsList = new List<BuffCard>();

    private void Awake()
    {
        //var buffCards = Resources.LoadAll<BuffCard>("Prefabs/Cards");
        //_buffCardsStock.AddRange(buffCards);




        //カードインスタンス生成（仮）
       
        SetBuffCardManagerState(BuffCardManagerState.CardCreate);
          
       
       
    }


    /// <summary>
    /// ステート
    /// </summary>
    public void SetBuffCardManagerState(BuffCardManagerState baffCardManagerState)
    {
        var state = baffCardManagerState;

        switch (state)
        {
            case BuffCardManagerState.Default:         
                
                break;

            case BuffCardManagerState.CardCreate:
                _activeCardsList?.Clear();
                _activeCardsList = new List<BuffCard>();

                for (int i = 0; i < _maxSelectCardValue; i++)
                {
                    //アクティブになっているカードをすべて取得
                    _activeCardsList.Add(CreateCard()); 
                }
                
                

                break;

            case BuffCardManagerState.CardSelected:
                //カードがどれか選択された
                Debug.Log("選択された");
                //ボタンコンポーネントを非アクティブに
                foreach(var activeCardsList in _activeCardsList)
                {
                    activeCardsList.ThisButton.enabled = false;
                }

                //アニメーション再生後にDestroy
                foreach (var activeCardsList in _activeCardsList)
                {
                    Destroy(activeCardsList.gameObject);
                }

                break;

        }
    }

    private BuffCard CreateCard()
    {
        BuffCard buffCardInstance = Instantiate(_buffCard, _posRoot) as BuffCard;
        buffCardInstance.gameObject.SetActive(true);
        buffCardInstance.CardSelected.Subscribe(_=> 
        {
            SetBuffCardManagerState(BuffCardManagerState.CardSelected);
            

        }).AddTo(this);
        CardCreated.OnNext(buffCardInstance);
        return buffCardInstance;
    }

}
