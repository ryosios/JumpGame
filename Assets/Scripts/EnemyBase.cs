using UnityEngine;
using UniRx;
using System.Collections;

public class EnemyBase : MonoBehaviour
{
    //敵クラス

    private enum EnemyBaseState 
    {
        EnemyCollisionEnter,
        EnemyCollisionExit,
        Default,
        Killed,

    }

    /// <summary> EnemyCollisionPresenter </summary>
    [SerializeField] private EnemyCollisionPresenter _collision;

    /// <summary>  _enemyImageのSpriteRenderer </summary>
    [SerializeField] private SpriteRenderer _enemyImage;

    /// <summary>  _enemyEffect1のParticleSystem </summary>
    [SerializeField] private ParticleSystem _enemyEffect1;

    /// <summary>  _enemyEffect1のParticleSystem </summary>
    [SerializeField] private ParticleSystem _enemyEffect2in;

    /// <summary>  EnemyCollisionEnterのSubject </summary>
    public Subject<Unit> EnemyCollisionEnter = new Subject<Unit>();

    /// <summary>  EnemyCollisionExitのSubject </summary>
    public Subject<Unit> EnemyCollisionExit = new Subject<Unit>();

    /// <summary>  KilledのSubject </summary>
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

    /// <summary>
    /// ステート
    /// </summary>
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
                //エフェクト再生
                _enemyEffect2in.Play();

                break;

            case EnemyBaseState.Killed:
                Killed.OnNext(Unit.Default);
                
                StartCoroutine(DestroyEnemy(this, 1f));

                break;

        }
    }

    /// <summary>
    /// 敵削除用
    /// </summary>
    private IEnumerator DestroyEnemy(EnemyBase enemyBase,float delayTime)
    {
        //透明にする
        _enemyImage.color = new Color(1, 1, 1, 0);
        //エフェクト再生
        _enemyEffect1.Play();
        //当たり判定だけオフ
        _collision.SetCircleColActive(false);
        yield return new WaitForSeconds(delayTime);
        Destroy(enemyBase.gameObject);
    }
}
