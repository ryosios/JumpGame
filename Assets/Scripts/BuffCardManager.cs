using UnityEngine;
using UniRx;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BuffCardManager : MonoBehaviour
{
    //バフカードプレファブ管理用

    public enum BuffCardManagerState
    {
        Default,
        CardCreate,
        CardSelected,
        UpdateBuffMaxSelectCard,
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

    [SerializeField] private Image _blackBoardImage;

    [SerializeField] private CanvasGroup _blackBoardGroups;

    private int _maxSelectCardValue = 3;

    private List<BuffCard> _activeCardsList = new List<BuffCard>();

    /// <summary>  敵カウントがいくつごとにカードイベントが起きるか </summary>
    private int _cardEventCount = 5;


    private BuffCard _selectedBuffCard;

    private void Awake()
    {
        //var buffCards = Resources.LoadAll<BuffCard>("Prefabs/Cards");
        //_buffCardsStock.AddRange(buffCards);

        _enemyCreater.EnemyKillCount.Subscribe(value => 
        {
            Debug.Log("koko-1");

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
    public void SetBuffCardManagerState(BuffCardManagerState baffCardManagerState, BuffMaxSelectCard buffMaxSelectCard = null)
    {
        var state = baffCardManagerState;

        switch (state)
        {
            case BuffCardManagerState.Default:
                _blackBoardImage.raycastTarget = false;
                _blackBoardGroups.alpha = 0f;
                _blackBoardGroups.gameObject.SetActive(false);
               

                break;

            case BuffCardManagerState.CardCreate:
                
                CardCreateStart.OnNext(Unit.Default);
                _activeCardsList?.Clear();
                _activeCardsList = new List<BuffCard>();

                _blackBoardGroups.gameObject.SetActive(true);
                _blackBoardImage.raycastTarget = true;
                _blackBoardGroups.alpha = 1f;

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
                    if(activeCardsList == _selectedBuffCard)
                    {
                        activeCardsList.tweenBuffCard.PlaySelectedAnim(0);
                        activeCardsList.tweenBuffCard.OutEnd.Subscribe(_=> 
                        {
                            CardSelectedEnd.OnNext(Unit.Default);

                            _blackBoardGroups.alpha = 0f;
                            _blackBoardGroups.gameObject.SetActive(false);
                            _blackBoardImage.raycastTarget = false;

                            //先に消しちゃうとHorizontalLayoutでレイアウトが崩れるのでまとめて消す
                            foreach (var card in _activeCardsList)
                            {
                                Destroy(card.gameObject);
                            }
                            

                        }).AddTo(this);
                    }
                    else
                    {
                        activeCardsList.tweenBuffCard.PlayOutAnim(0f);
                        //先に消しちゃうとHorizontalLayoutでレイアウトが崩れるのでまとめて消す
                        
                    }
                    
                }               

                break;

            case BuffCardManagerState.UpdateBuffMaxSelectCard:
                if (_maxSelectCardValue < 5) 
                {
                    //いったん上限5枚まで。2段目作っていいかも
                    _maxSelectCardValue += buffMaxSelectCard._addBuffMaxSelectCardValue;
                }
                


                break;
        }
    }

    private BuffCard CreateCard()
    {
        BuffCard buffCardInstance = Instantiate(_buffCard, _posRoot) as BuffCard;
        
        buffCardInstance.gameObject.SetActive(true);
        buffCardInstance.CardSelected.Subscribe( buffCard =>
        {//カードが選択された時のサブジェクト
            _selectedBuffCard = buffCard;
            SetBuffCardManagerState(BuffCardManagerState.CardSelected);
            

        }).AddTo(this);
        buffCardInstance.CardSelectedBuffMaxSelectCard.Subscribe(buffMaxSelectCard =>
        {//バフでカード枚数が増える判定のサブジェクト
            SetBuffCardManagerState(BuffCardManagerState.UpdateBuffMaxSelectCard, buffMaxSelectCard);            

        }).AddTo(this);
        CardCreated.OnNext(buffCardInstance);
        return buffCardInstance;
    }

}
