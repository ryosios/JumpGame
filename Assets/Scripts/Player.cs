using UnityEngine;
using UniRx;
using DG.Tweening;
using System.Collections;
using Spine.Unity;
using Spine;
using System.Linq;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    //プレイヤー用クラス

    public enum PlayerState
    {
        Default,
        Direction,
        Jump,
        Attack,
        SutaminaRecovery,
        Bounce,
        BuffLevelUp
    }

    public Subject<Unit> EnemyCollisionEnter = new Subject<Unit>();

    public Subject<Unit> EnemyCollisionExit = new Subject<Unit>();

    /// <summary>  スタミナ変動したときのサブジェクト </summary>
    public Subject<float> SutaminaChange = new Subject<float>();

    /// <summary>  最初の一回目のジャンプ </summary>
    public Subject<Unit> JumpOneTime = new Subject<Unit>();

    /// <summary> _rotationArrowRootTransのTransform </summary>
    [SerializeField] Transform _rotationArrowRootTrans;

    /// <summary> _playerRigidのRigidbody2D </summary>
    [SerializeField] Rigidbody2D _playerRigid;

    /// <summary> _attackCollisionのCircleCollider2D </summary>
    [SerializeField] CircleCollider2D _attackCollision;

    /// <summary> _arrowのTransform </summary>
    [SerializeField] private Transform _arrowTrans;

    /// <summary> 回転用のシーケンス </summary>
    private DG.Tweening.Sequence _rotateSequence;

    /// <summary> ジャンプ力 </summary>
    private float _jumpPower = 20f;

    /// <summary>  ジャンプ角度 </summary>
    private float _jumpAngle = 360f;

    /// <summary>  ジャンプに消費するスタミナ </summary>
    private float _jumpSutamina = 0.2f;

    /// <summary> スタミナの値 </summary>
    private float _sutaminaValue = 1f;

    /// <summary> スタミナ回復スピード </summary>
    private float _sutaminaRecoverySpeed = 0.005f;

    /// <summary> 　矢印出てる時間 </summary>
    private float _moveAngleTime = 1f;

    /// <summary> Attackコリジョンがオン状態の時間 </summary>
    private float _attackCollOffTime = 0.1f;

    /// <summary> Attackのリキャストタイム </summary>
    private float _attackRecastTime = 0.5f;

    private bool _isAttack = false;    

    /// <summary> 最初の一回目のジャンプフラグ </summary>
    private bool _isJumpOneTime = false;

    /// <summary> 子のコリジョン取得用 </summary>
    [SerializeField] PlayerCollisionPresenter _playerCollision;


    //バフ関連
    /// <summary> プレイヤーのレベル（強化段階） </summary>
    private int _playerBuffLevel = 1;

    /// <summary> BuffCardManager </summary>
    [SerializeField] BuffCardManager _buffCardManager;

    /// <summary> バフレベルが上がった時 </summary>
    public Subject<Unit> BuffLevelUpEnd = new Subject<Unit>();


    //Spine
    /// <summary> SkeletonAnimation </summary>
    [SerializeField] private SkeletonAnimation _skeletonAnimation;

    /// <summary> ジャンプで再生したいアニメーションリスト </summary>
    private string[] _jumpAnimNameTargetStringList = { "jump_1", "jump_2", "jump_3", "jump_4" };

    /// <summary> 取得したアニメリスト </summary>
    private List<Spine.Animation> _jumpCandidates;

    /// <summary> ジャンプで切り替えたいアタッチメントのあるスロット </summary>
    private string _slotName = "face_1";

    /// <summary> ジャンプで切り替えたいアタッチメントのリスト </summary>
    private string[] _attachmentNames = { "face_1", "face_2" };

    private List<Attachment> _attachments;

    private Collision2D _bounceCollision;

    private void Awake()
    {
        InitializeSpineAnim();

        //バウンス時
        _playerCollision.CollisionExit.Subscribe(collision =>
        {
            GetBounceCollision(collision);
            SetPlayerState(PlayerState.Bounce);

        }).AddTo(this);

        _buffCardManager.CardSelectedEnd.Subscribe(_=> 
        {
            SetPlayerState(PlayerState.BuffLevelUp);

        }).AddTo(this);
    }

    private void Start()
    {
        SetPlayerState(PlayerState.Default);

        InputController.Instance.CenterAreaButtonEnter.Where(_ => _sutaminaValue > _jumpSutamina).Subscribe(_ =>
        {
            SetPlayerState(PlayerState.Direction);

        }).AddTo(this);

        InputController.Instance.CenterAreaButtonExit.Where(_ => _sutaminaValue > _jumpSutamina).Subscribe(_ =>
        {
            SetPlayerState(PlayerState.Jump);

        }).AddTo(this);

 


        //スタミナ自動回復いったんオフ
        /*
        Observable.EveryUpdate().Subscribe(_ =>
        {
            SetPlayerState(PlayerState.SutaminaRecovery);

        }).AddTo(this);
        */
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
                _arrowTrans.localScale = new Vector3(0f,1f,1f);

                _rotateSequence?.Kill();
                _rotateSequence = DOTween.Sequence();

                _rotateSequence.Append(_rotationArrowRootTrans.DOLocalRotate(new Vector3(0, 0, -_jumpAngle), _moveAngleTime,DG.Tweening.RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLink(gameObject));
                _rotateSequence.Join(_arrowTrans.DOScaleX(1f,1f).SetEase(Ease.OutElastic));

                /*
                _rotateSequence.Append(_rotationArrowRootTrans.DOLocalRotate(new Vector3(0, 0, _jumpAngle),_moveAngleTime/4*1).SetEase(Ease.Linear).SetLink(gameObject));
                _rotateSequence.Append(_rotationArrowRootTrans.DOLocalRotate(new Vector3(0, 0, -_jumpAngle), _moveAngleTime / 4 * 2).SetEase(Ease.Linear).SetLink(gameObject));
                _rotateSequence.Append(_rotationArrowRootTrans.DOLocalRotate(new Vector3(0, 0, 0), _moveAngleTime / 4 * 1).SetEase(Ease.Linear).SetLink(gameObject));
                */
                _rotateSequence.OnComplete(()=>
                {         
                    _rotateSequence?.Kill();
                    
                });

                break;

            case PlayerState.Jump:
                _rotateSequence?.Kill();
                _rotationArrowRootTrans.gameObject.SetActive(false);
                _playerRigid.constraints = RigidbodyConstraints2D.None;
                //_playerRigid.constraints = RigidbodyConstraints2D.FreezeRotation;

                _sutaminaValue -= _jumpSutamina;
                SutaminaChange.OnNext(_sutaminaValue);

                SetJump();

                if(_isJumpOneTime == false)
                {
                    _isJumpOneTime = true;
                    JumpOneTime.OnNext(Unit.Default);
                }

                //アニメーションとアタッチメントをランダム切り替え
                PlayRandomSpineAnim(_skeletonAnimation, _jumpCandidates, false);
                ApplyRandomAttachment(_skeletonAnimation, _slotName, _attachments);


                break;

            case PlayerState.Attack:

                /*
                if (_isAttack == false)
                {
                    _isAttack = true; //アタックのリキャスト用
                    StartCoroutine(SetAttack());
                }           
                */
                break;

            case PlayerState.SutaminaRecovery:
              
                if(_sutaminaValue < 1f)
                {
                    _sutaminaValue += Time.deltaTime * _sutaminaRecoverySpeed;
                    SutaminaChange.OnNext(_sutaminaValue);

                }                

                break;

            case PlayerState.Bounce:

                if (_bounceCollision.gameObject.layer == 8)
                {
                    PlayRandomSpineAnim(_skeletonAnimation, _jumpCandidates, false);
                    ApplyRandomAttachment(_skeletonAnimation, _slotName, _attachments);
                }            
               
                break;

            case PlayerState.BuffLevelUp:
                _playerBuffLevel += 1;
                BuffLevelUpEnd.OnNext(Unit.Default);

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


    //Spine
    /// <summary>
    /// Spineのアニメーション関連の初期設定
    /// </summary>
    void InitializeSpineAnim()
    {
        //Spineアニメーション初期設定       
        var skeletonData = _skeletonAnimation.Skeleton.Data;

        _jumpCandidates = skeletonData.Animations
           .Where(a => _jumpAnimNameTargetStringList.Contains(a.Name))
           .ToList();

        //Spineアタッチメント名初期設定
        var slotIndex = skeletonData.FindSlot(_slotName).Index;
        _attachments = new List<Attachment>();
        foreach (var name in _attachmentNames)
        {
            var attachment = skeletonData.DefaultSkin.GetAttachment(slotIndex, name);
            if (attachment != null)
                _attachments.Add(attachment);
        }
    }

    /// <summary>
    /// Spineのアニメーションをランダム再生
    /// </summary>
    /// <param name="skeletonAnimation">対象のskeletonAnimation</param>
    /// <param name="candidates">取得したアニメーション一覧</param>
    /// <param name="candidates">再生するアニメの名前のリスト</param>
    void PlayRandomSpineAnim(SkeletonAnimation skeletonAnimation, List<Spine.Animation> candidates ,bool isLoop)
    {
        if (candidates == null || candidates.Count == 0)
            return;

        var anim = candidates[Random.Range(0, candidates.Count)];
        skeletonAnimation.AnimationState.SetAnimation(0, anim,isLoop);
    }

    /// <summary>
    /// Spineのアタッチメントをランダム切り替え
    /// </summary>
    public void ApplyRandomAttachment(SkeletonAnimation skeletonAnimation,string slotName, List<Attachment> attachments)
    {
        if (attachments.Count == 0)
            return;

        var at = attachments[Random.Range(0, _attachments.Count)];
        skeletonAnimation.Skeleton.SetAttachment(slotName, at.Name);
    }


    /// <summary>
    /// Collision受け渡し用
    /// </summary>
    public void GetBounceCollision(Collision2D collision)
    {
        _bounceCollision = collision;
    }

    /// <summary>
    /// バフレベルを取得
    /// </summary>
    public int GetBuffLevel()
    {
        return _playerBuffLevel;
    }
}
