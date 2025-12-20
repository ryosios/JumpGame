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
    private List<EnemyBase> _activeEnemyBase = new ();

    /// <summary> 撃破した敵の数 </summary>
    public ReactiveProperty<int> EnemyKillCount { get; } = new ReactiveProperty<int>(0);


    /// <summary> Enemyぶら下げるRoot </summary>
    [SerializeField] private Transform _enemyRoot;

    /// <summary> Enemy生成用の起点オブジェクトのTransform </summary>
    [SerializeField] private Transform _enemyCreatePosLocatorTrans;

    /// <summary> EnemyBaseプレファブのリソース </summary>
    private List<EnemyBase> _enemyPrefabs;

    /// <summary> Enemyが存在できる上限数 </summary>
    private int _maxEnemyValue = 20;

    /// <summary> Enemy生成範囲の内半径 </summary>
    private float _enemyNewInnerRadius = 1f;

    /// <summary> Enemy生成範囲の外半径 </summary>
    private float _enemyNewOuterRadius = 14f;

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

            //ステージ中心座標を起点にして敵を生成
            enemyBase.transform.localPosition = GetRandomPosInRectDonut(Vector2.zero, new Vector2(_enemyNewInnerRadius, _enemyNewInnerRadius), new Vector2(_enemyNewOuterRadius, _enemyNewOuterRadius));
            _activeEnemyBase.Add(enemyBase);
            enemyBase.Killed.Subscribe(_ =>
            {
                _activeEnemyBase.Remove(enemyBase);
                EnemyKillCount.Value += 1;
                SetEnemyCreaterState(EnemyCreaterState.Create);
            }).AddTo(this);
            SetEnemyCreaterState(EnemyCreaterState.Wait);
        }
       
    }

    /// <summary>
    /// 中心座標からドーナツ型範囲内のランダムな位置を返す
    /// </summary>
    /// <param name="center">中心座標</param>
    /// <param name="minRadius">内半径</param>
    /// <param name="maxRadius">外半径</param>
    private Vector3 GetRandomPosInDonut(Vector2 center, float minRadius, float maxRadius)
    {
        // 半径は sqrt を使って面積的に均等にする
        float radius = Mathf.Sqrt(
            Random.Range(minRadius * minRadius, maxRadius * maxRadius)
        );

        float angle = Random.Range(0f, Mathf.PI * 2f);

        Vector2 offset = new Vector2(
            Mathf.Cos(angle),
            Mathf.Sin(angle)
        ) * radius;

        return center + offset;
    }

    /// <summary>
    /// 中心座標から長方形ドーナツ範囲内のランダムな位置を返す
    /// </summary>
    /// <param name="center">中心座標</param>
    /// <param name="innerHalf">内半径</param>
    /// <param name="outerHalf">外半径</param>
    private Vector2 GetRandomPosInRectDonut( Vector2 center, Vector2 innerHalf, Vector2 outerHalf)
    {
        float topArea = (outerHalf.x * 2) * (outerHalf.y - innerHalf.y);
        float bottomArea = topArea;
        float leftArea = innerHalf.y * 2 * (outerHalf.x - innerHalf.x);
        float rightArea = leftArea;

        float total = topArea + bottomArea + leftArea + rightArea;
        float r = Random.Range(0f, total);

        if (r < topArea)
        {
            return center + new Vector2(
                Random.Range(-outerHalf.x, outerHalf.x),
                Random.Range(innerHalf.y, outerHalf.y)
            );
        }
        r -= topArea;

        if (r < bottomArea)
        {
            return center + new Vector2(
                Random.Range(-outerHalf.x, outerHalf.x),
                Random.Range(-outerHalf.y, -innerHalf.y)
            );
        }
        r -= bottomArea;

        if (r < leftArea)
        {
            return center + new Vector2(
                Random.Range(-outerHalf.x, -innerHalf.x),
                Random.Range(-innerHalf.y, innerHalf.y)
            );
        }

        // right
        return center + new Vector2(
            Random.Range(innerHalf.x, outerHalf.x),
            Random.Range(-innerHalf.y, innerHalf.y)
        );
    }


}
