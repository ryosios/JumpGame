using UnityEngine;
using UniRx;
using DG.Tweening;
using System.Collections;
using UnityEngine.UI;

public class CardShaderSetUnscaledTime : MonoBehaviour
{
    private enum ThisState
    {
        Default,
        Update
    }

    //public Subject<Unit> Default = new Subject<Unit>();

    [SerializeField] private Image _thisImage;

    private Material _thisMaterial;

    private void Awake()
    {
        Material mat = _thisImage.material;
        _thisMaterial = Instantiate(mat);
        _thisImage.material = _thisMaterial;

      
    }

    private void Update()
    {
        SetThisState(ThisState.Update);
    }
    /// <summary>
    /// ステート
    /// </summary>
    private void SetThisState(ThisState thisState)
    {
        var state = thisState;

        switch (state)
        {
            case ThisState.Default:         
                
                break;

            case ThisState.Update:
                _thisMaterial.SetFloat("_UnScaledTime", Time.unscaledTime);
               

                break;

        }
    }

}
