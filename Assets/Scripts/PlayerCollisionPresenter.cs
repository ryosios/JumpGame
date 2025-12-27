using UnityEngine;
using UniRx;

public class PlayerCollisionPresenter : MonoBehaviour
{    

    public Subject<Collision2D> CollisionEnter = new Subject<Collision2D>();

    public Subject<Collision2D> CollisionExit = new Subject<Collision2D>();

    [SerializeField]private CircleCollider2D _thisCircleCol;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CollisionEnter.OnNext(collision);
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        CollisionExit.OnNext(collision);
        
        
    }

    public void SetCircleColActive(bool isActive)
    {
        _thisCircleCol.enabled = isActive;
    }

}
