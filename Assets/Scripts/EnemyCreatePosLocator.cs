using UnityEngine;
using UniRx;
public class EnemyCreatePosLocator : MonoBehaviour
{
    private enum EnemyCreatePosLocatorState
    {
        Default,
        Update
    }
    private Player _player;


    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        Observable.EveryUpdate().Subscribe(_=> 
        {
            SetEnemyCreatePosLocatorState(EnemyCreatePosLocatorState.Update);

        }).AddTo(this);
    }

    /// <summary>
    /// ステート
    /// </summary>
    private void SetEnemyCreatePosLocatorState(EnemyCreatePosLocatorState enemyCreatePosLocatorState)
    {
        var state = enemyCreatePosLocatorState;

        switch (state)
        {
            case EnemyCreatePosLocatorState.Default:
                
                break;
            case EnemyCreatePosLocatorState.Update:
                this.transform.position = _player.transform.position;

                break;

        }
    }
}
