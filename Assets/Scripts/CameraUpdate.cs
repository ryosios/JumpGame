using UnityEngine;
using UniRx;
using DG.Tweening;
using System.Collections;

public class CameraUpdate : MonoBehaviour
{
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
        var updateCameraPos = new Vector3(playerPos.x, playerPos.y, -20f);
        this.transform.position = updateCameraPos;
    }

}
