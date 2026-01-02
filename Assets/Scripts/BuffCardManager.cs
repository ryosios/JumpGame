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

    /// <summary>  バフカードが生成された最初 </summary>
    public Subject<Unit> CardCreateStart = new Subject<Unit>();

    /// <summary>  各バフカードが生成されたとき </summary>
    public Subject<BuffCard> CardCreated = new Subject<BuffCard>();

    /// <summary>  バフカードが選択されおわったとき </summary>
    public Subject<Unit> CardSelectedEnd = new Subject<Unit>();



    [SerializeField] private Transform _posRoot;

    [SerializeField] private BuffCard _buffCard;

    [SerializeField] private EnemyCreater _enemyCreater;

    private int _maxSelectCardValue = 3;

    private List<BuffCard> _activeCardsList = new List<BuffCard>();

    /// <summary>  敵カウントがいくつごとにカードイベントが起きるか </summary>
    private int _cardEventCount = 5;

    private void Awake()
    {
        //var buffCards = Resources.LoadAll<BuffCard>("Prefabs/Cards");
        //_buffCardsStock.AddRange(buffCards);

        _enemyCreater.EnemyKillCount.Subscribe(value => 
        {

            if (value != 0 && value % _cardEventCount == 0)
            {
                //カードインスタンス生成（仮）
                
                SetBuffCardManagerState(BuffCardManagerState.CardCreate);
            }
          
        
        }).AddTo(this);         
       
       
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
                CardCreateStart.OnNext(Unit.Default);
                _activeCardsList?.Clear();
                _activeCardsList = new List<BuffCard>();

                for (int i = 0; i < _maxSelectCardValue; i++)
                {
                    //アクティブになっているカードをすべて取得しておく
                    var card = CreateCard();
                    _activeCardsList.Add(card);
                    card.tweenBuffCard.PlayInAnim(i * 0.1f);

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

                CardSelectedEnd.OnNext(Unit.Default);

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
