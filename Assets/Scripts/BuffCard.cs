using UnityEngine;
using UniRx;
using DG.Tweening;
using System.Collections;
using UnityEngine.UI;
using TMPro;

using System.Collections.Generic;

public class BuffCard : MonoBehaviour
{
    //バフカードのプレファブ用クラス

    private enum BuffCardState
    {
        Default,
        Initialize,
        CardSelect,
        Destroy
    }  

    /// <summary> カードが選択されたとき </summary>
    public Subject<BuffCard> CardSelected = new Subject<BuffCard>();

    /// <summary> BuffAddTimeカードが選択されたとき </summary>
    public Subject<BuffAddTime> CardSelectedBuffAddTime = new Subject<BuffAddTime>();

    /// <summary> BuffPlayerSizeカードが選択されたとき </summary>
    public Subject<BuffPlayerSize> CardSelectedBuffPlayerSize = new Subject<BuffPlayerSize>();

    [SerializeField] private Button _thisButton;
    public Button ThisButton  => _thisButton;


    [SerializeField] private TextMeshProUGUI _buffTextText;

    /// <summary> カードのアニメーションクラス </summary>
    [SerializeField] private TweenBuffCard _tweenBuffCard;

    public TweenBuffCard tweenBuffCard => _tweenBuffCard;

    /// <summary> ロードした全BaffBaseのスクリプタブルオブジェクト </summary>
    [Header("※ 注意：このリストは実行時に自動で代入されます")]
    [SerializeField] private List<BuffBase> _buffBasesStock;

    /// <summary> このカードにつけるバフ能力のリスト </summary>
    [Header("※ 注意：このリストは実行時に自動で代入されます")]
    [SerializeField] private List<BuffBase> _buffBasesActive;



    private void Awake()
    {
       

        SetBuffCardState(BuffCardState.Default);
    }

    /// <summary>
    /// ステート
    /// </summary>
    private void SetBuffCardState(BuffCardState buffCardState)
    {
        var state = buffCardState;

        switch (state)
        {
            case BuffCardState.Default:
                //インスタンス化時の最初の一回
                //全バフ用スクリプタるオブジェクトを読み込んでおく
                _buffBasesStock = new();
                var buffBasesStock = Resources.LoadAll<BuffBase>("ScriptableObjects/BuffBases");                
                _buffBasesStock.AddRange(buffBasesStock);
                SetBuffCardState(BuffCardState.Initialize);

                break;

            case BuffCardState.Initialize:
                //インスタンス化時、続けてランダムでBuffBaseの中から能力をゲットしてカードにつっこむ
                //とりあえず能力1個
                _buffBasesActive = new();
                int index_A = Random.Range(0, _buffBasesStock.Count);
                _buffBasesActive.Add(_buffBasesStock[index_A]);
                _buffTextText.text = _buffBasesActive[0].buffName;
               


                break;

            case BuffCardState.CardSelect:
                //カードにセット予定の能力（_buffBasesActive）を子クラスの種類で振り分けて発火
                CardSelected.OnNext(this);

                foreach (var buffBaseActive in _buffBasesActive)
                {
                    

                    if (buffBaseActive is BuffAddTime buffAddTime)
                    {
                        CardSelectedBuffAddTime.OnNext(buffAddTime);
                        
                    }
                    if (buffBaseActive is BuffPlayerSize buffPlayerSize)
                    {
                        CardSelectedBuffPlayerSize.OnNext(buffPlayerSize);

                    }



                }
                

                break;

            case BuffCardState.Destroy:
                //リストクリア
                _buffBasesStock?.Clear();
                _buffBasesActive?.Clear();

                break;


        }
    }

    private void OnDestroy()
    {
        SetBuffCardState(BuffCardState.Destroy);
 
    }

    /// <summary>
    /// ボタンから呼ぶ用
    /// </summary>
    public void SetStateCardSelect()
    {
        SetBuffCardState(BuffCardState.CardSelect);
    }

}
