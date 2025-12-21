using UnityEngine;
using UniRx;
using DG.Tweening;
using System.Collections;
using UnityEngine.UI;

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
    public Subject<BuffBase> CardSelected = new Subject<BuffBase>();

    /// <summary> BuffAddTimeカードが選択されたとき </summary>
    public Subject<BuffAddTime> CardSelectedBuffAddTime = new Subject<BuffAddTime>();

    [SerializeField] private Button _thisButton;
    public Button ThisButton  => _thisButton;

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
                //ランダムでBuffBaseの中から能力をゲットしてカードにつっこむ
                //とりあえず能力1個
                _buffBasesActive = new();
                int index_A = Random.Range(0, _buffBasesStock.Count);
                _buffBasesActive.Add(_buffBasesStock[index_A]);


                break;

            case BuffCardState.CardSelect:
                //カードにセット予定の能力（_buffBasesActive）を子クラスの種類で振り分けて発火

                foreach (var buffBaseActive in _buffBasesActive)
                {
                    CardSelected.OnNext(buffBaseActive);

                    if (buffBaseActive is BuffAddTime buffAddTime)
                    {
                        CardSelectedBuffAddTime.OnNext(buffAddTime);
                        
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
