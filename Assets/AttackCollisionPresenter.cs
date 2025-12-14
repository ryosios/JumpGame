using UnityEngine;
using UniRx;

public class AttackCollisionPresenter : MonoBehaviour
{    

    public Subject<Unit> AttackCollisionEnter = new Subject<Unit>();

    public Subject<Unit> AttackCollisionExit = new Subject<Unit>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AttackCollisionEnter.OnNext(Unit.Default);
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        AttackCollisionExit.OnNext(Unit.Default);
        
    }

}
