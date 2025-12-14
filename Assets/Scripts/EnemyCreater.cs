using UnityEngine;
using System.Collections.Generic;
using UniRx;
using System.Linq;

public class EnemyCreater : MonoBehaviour
{
    private enum EnemyCreaterState
    {
        Wait,
        Create
    }

    /// <summary> インスタンス化済みのEnemyBaseリスト </summary>
    [SerializeField] private List<EnemyBase> _activeEnemyBase = new ();

    /// <summary> Enemyぶら下げるRoot </summary>
    [SerializeField] private Transform _enemyRoot;

    /// <summary> EnemyBaseプレファブのリソース </summary>
    private List<EnemyBase> _enemyPrefabs;

    /// <summary> Enemyが存在できる上限数 </summary>
    private int _maxEnemyValue = 10;

    /// <summary> Enemy生成範囲の半径 </summary>
    private float _enemyNewRadius = 12f;

    private void Awake()
    {
        //enemy読み込み
        _enemyPrefabs = Resources.LoadAll<EnemyBase>("Prefabs/Enemys").ToList();

        //開始時生成
        for (int i = 0; i < _maxEnemyValue; i++)
        {

            SetEnemyCreaterState(EnemyCreaterState.Create);

        }
    }

    /// <summary>
    /// ステート
    /// </summary>
    private void SetEnemyCreaterState(EnemyCreaterState enemyCreaterState)
    {
        var state = enemyCreaterState;

        switch (state)
        {
            case EnemyCreaterState.Wait:


                break;

            case EnemyCreaterState.Create:
                CreateEnemy();   
                

                break;
        }

    }

    /// <summary>
    /// 敵生成
    /// </summary>
    private void CreateEnemy()
    {
        if(_activeEnemyBase.Count < _maxEnemyValue)
        {           
            EnemyBase enemyBase = Instantiate(_enemyPrefabs[0], _enemyRoot).GetComponent<EnemyBase>();
            enemyBase.transform.parent = _enemyRoot;
            enemyBase.transform.localPosition = enemyRandomPosReturn(Vector2.zero, _enemyNewRadius);
            _activeEnemyBase.Add(enemyBase);
            enemyBase.Killed.Subscribe(_ =>
            {
                _activeEnemyBase.Remove(enemyBase);
                SetEnemyCreaterState(EnemyCreaterState.Create);
            }).AddTo(this);
            SetEnemyCreaterState(EnemyCreaterState.Wait);
        }
       
    }

    /// <summary>
    /// ランダム位置を返す
    /// </summary>
    /// <param name="center">中心座標</param>
    /// <param name="radius">半径</param>
    private Vector3 enemyRandomPosReturn(Vector2 center, float radius)
    {
        Vector2 random = Random.insideUnitCircle * radius;
        return center + random;
    }

}
