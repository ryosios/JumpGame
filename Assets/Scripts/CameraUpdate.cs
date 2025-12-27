using UnityEngine;
using UniRx;
using DG.Tweening;
using System.Collections;

public class CameraUpdate : MonoBehaviour
{
    //カメラ全般用クラス

    public enum CameraState
    {
        Default,
        Update,

    }

    [SerializeField] Transform _playerTrans;

    private void Awake()
    {
        Observable.EveryUpdate().Subscribe(_ =>
        {
            SetCameraState(CameraState.Update);          

        }).AddTo(this);

    }
    /// <summary>
    /// ステート
    /// </summary>
    public void SetCameraState(CameraState cameraState)
    {
        var state = cameraState;

        switch (state)
        {
            case CameraState.Default:         
                
                break;

            case CameraState.Update:
                UpdateCameraPos();

                break;

        }
    }

    /// <summary>
    /// カメラ位置を更新
    /// </summary>
    private void UpdateCameraPos()
    {
        float dt = Time.deltaTime;
        // 毎フレームの処理
        Vector3 playerPos = _playerTrans.position;

        float clampedX = Mathf.Clamp(playerPos.x, -8f, 8f);
        float clampedY = Mathf.Clamp(playerPos.y, -12f, 12f);

        transform.position = new Vector3(
            clampedX,
            clampedY,
            -20f
        );


    }

}
