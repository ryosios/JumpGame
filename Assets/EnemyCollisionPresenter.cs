using UnityEngine;
using UniRx;

public class EnemyCollisionPresenter : MonoBehaviour
{    

    public Subject<Unit> CollisionEnter = new Subject<Unit>();

    public Subject<Unit> CollisionExit = new Subject<Unit>();

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CollisionEnter.OnNext(Unit.Default);
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        CollisionExit.OnNext(Unit.Default);
        
    }

}
