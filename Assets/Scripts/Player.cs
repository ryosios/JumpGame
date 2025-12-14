using UnityEngine;
using UniRx;
using DG.Tweening;
using System.Collections;

public class Player : MonoBehaviour
{
   public enum PlayerState
    {
        Default,
        Direction,
        Jump,
        Attack,
    }

    public Subject<Unit> EnemyCollisionEnter = new Subject<Unit>();

    public Subject<Unit> EnemyCollisionExit = new Subject<Unit>();

    /// <summary>  スタミナ変動したときのサブジェクト </summary>
    public Subject<float> SutaminaChange = new Subject<float>();

    /// <summary> _rotationArrowRootTransのTransform </summary>
    [SerializeField] Transform _rotationArrowRootTrans;

    /// <summary> _playerRigidのRigidbody2D </summary>
    [SerializeField] Rigidbody2D _playerRigid;

    /// <summary> _attackCollisionのCircleCollider2D </summary>
    [SerializeField] CircleCollider2D _attackCollision;

    /// <summary> _sutaminaSliderのSutaminaSlider </summary>
    [SerializeField] SutaminaSlider _sutaminaSlider;

    /// <summary> 回転用のシーケンス </summary>
    private Sequence _rotateSequence;

    /// <summary> ジャンプ力 </summary>
    private float _jumpPower = 20f;

    /// <summary>  ジャンプ角度 </summary>
    private float _jumpAngle = 60f;

    /// <summary> 　矢印出てる時間 </summary>
    private float _moveAngleTime = 2f;

    /// <summary> Attackコリジョンがオン状態の時間 </summary>
    private float _attackCollOffTime = 0.1f;

    /// <summary> Attackのリキャストタイム </summary>
    private float _attackRecastTime = 0.5f;

    private bool _isAttack = false;

    /// <summary> スタミナの値 </summary>
    private float _sutaminaValue = 1f;

    private void Start()
    {
        SetPlayerState(PlayerState.Default);

        InputController.Instance.LeftClickEnter.Subscribe(_ =>
        {
            SetPlayerState(PlayerState.Direction);

        }).AddTo(this);

        InputController.Instance.LeftClickExit.Where(_=>_sutaminaValue>0).Subscribe(_ =>
        {
            SetPlayerState(PlayerState.Jump);

        }).AddTo(this);

        InputController.Instance.RightClickEnter.Subscribe(_ =>
        {
            SetPlayerState(PlayerState.Attack);

        }).AddTo(this);

        InputController.Instance.RightClickExit.Subscribe(_ =>
        {
           

        }).AddTo(this);
       
    }

    /// <summary>
    /// ステート
    /// </summary>
    public void SetPlayerState(PlayerState playerState)
    {
        var state = playerState;

        switch (state)
        {
            case PlayerState.Default:
                
                Initialized();

                break;

            case PlayerState.Direction:

                _playerRigid.linearVelocity = Vector2.zero;
                _playerRigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;

                _rotationArrowRootTrans.gameObject.SetActive(true);
                _rotationArrowRootTrans.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                _rotateSequence?.Kill();
                _rotateSequence = DOTween.Sequence();                
                _rotateSequence.Append(_rotationArrowRootTrans.DOLocalRotate(new Vector3(0, 0, _jumpAngle),_moveAngleTime/4*1).SetEase(Ease.Linear).SetLink(gameObject));
                _rotateSequence.Append(_rotationArrowRootTrans.DOLocalRotate(new Vector3(0, 0, -_jumpAngle), _moveAngleTime / 4 * 2).SetEase(Ease.Linear).SetLink(gameObject));
                _rotateSequence.Append(_rotationArrowRootTrans.DOLocalRotate(new Vector3(0, 0, 0), _moveAngleTime / 4 * 1).SetEase(Ease.Linear).SetLink(gameObject));
                _rotateSequence.OnComplete(()=>
                {         
                    _rotateSequence?.Kill();
                    
                });

                break;

            case PlayerState.Jump:
                _rotateSequence?.Kill();
                _rotationArrowRootTrans.gameObject.SetActive(false);
                _playerRigid.constraints = RigidbodyConstraints2D.None;
                _playerRigid.constraints = RigidbodyConstraints2D.FreezeRotation;

                _sutaminaValue -= 0.2f;
                SutaminaChange.OnNext(-0.2f);

                SetJump();
                break;

            case PlayerState.Attack:

                if (_isAttack == false)
                {
                    _isAttack = true; //アタックのリキャスト用
                    StartCoroutine(SetAttack());
                }           

                break;

        }
    }

    /// <summary>
    /// 初期化
    /// </summary>
    private void Initialized()
    {
        _rotationArrowRootTrans.gameObject.SetActive(false);
        _playerRigid.linearVelocity = Vector2.zero;
        _playerRigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;

        _attackCollision.gameObject.SetActive(false);
    }

    /// <summary>
    /// Jump
    /// </summary>
    private void SetJump()
    {
        _playerRigid.linearVelocity = Vector2.zero;

        float angle = _rotationArrowRootTrans.eulerAngles.z;
        if (angle > 180f) 
        {
            angle -= 360f;
        } 

        Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad),Mathf.Sin(angle * Mathf.Deg2Rad));
        _playerRigid.AddForce(dir * _jumpPower, ForceMode2D.Impulse);

        Debug.Log("");
    }

    /// <summary>
    /// Attack
    /// </summary>
    private IEnumerator SetAttack()
    {
        _attackCollision.gameObject.SetActive(true);
        yield return new WaitForSeconds(_attackCollOffTime);
        _attackCollision.gameObject.SetActive(false);
        yield return new WaitForSeconds(_attackRecastTime - _attackCollOffTime);
        _isAttack = false;//アタックのリキャストフラグを戻す
    }
}
