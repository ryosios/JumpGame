using UnityEngine;
using UniRx;

[CreateAssetMenu(fileName = "BuffBase", menuName = "Scriptable Objects/BuffBase")]
public class BuffBase : ScriptableObject
{
    //バフ能力ベースクラス

    [Header("Card Info")]
    public string buffName;
    [TextArea]
    public string description;
    public Sprite icon;

  

 

}
