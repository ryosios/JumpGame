using UnityEngine;
using UniRx;
using System.Collections;

public class EnemyBase : MonoBehaviour
{
    private enum EnemyBaseState 
    {
        EnemyCollisionEnter,
        EnemyCollisionExit,
        Default,
        Killed,

    }

    [SerializeField] private EnemyCollisionPresenter _collision;

    [SerializeField] private SpriteRenderer _enemyImage;

    [SerializeField] private ParticleSystem _enemyEffect1;

    public Subject<Unit> EnemyCollisionEnter = new Subject<Unit>();

    public Subject<Unit> EnemyCollisionExit = new Subject<Unit>();

    public Subject<Unit> Killed = new Subject<Unit>();

    private void Awake()
    {
        SetEnemyBaseState(EnemyBaseState.Default);

        _collision.CollisionEnter.Subscribe(_=> 
        {
            Debug.Log("コリジョン開始");
            SetEnemyBaseState(EnemyBaseState.EnemyCollisionEnter);
            SetEnemyBaseState(EnemyBaseState.Killed);

        }).AddTo(this);

        _collision.CollisionExit.Subscribe(_ =>
        {
            Debug.Log("コリジョン終了");
            SetEnemyBaseState(EnemyBaseState.EnemyCollisionExit);            

        }).AddTo(this);

        //画面外にいったら
    }

    private void SetEnemyBaseState(EnemyBaseState enemyBaseState)
    {
        var state = enemyBaseState;

        switch (state)
        {
            case EnemyBaseState.EnemyCollisionEnter:
                EnemyCollisionEnter.OnNext(Unit.Default);
                break;

            case EnemyBaseState.EnemyCollisionExit:
                EnemyCollisionExit.OnNext(Unit.Default);
                break;

            case EnemyBaseState.Default:
               
                break;

            case EnemyBaseState.Killed:
                Killed.OnNext(Unit.Default);
                _enemyImage.color = new Color(1, 1, 1, 0);
                _enemyEffect1.Play();
                DestroyEnemy(this, 1f);

                break;

        }
    }

    private IEnumerator DestroyEnemy(EnemyBase enemyBase,float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Destroy(enemyBase);
    }
}
