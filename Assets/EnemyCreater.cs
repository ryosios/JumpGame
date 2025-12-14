using UnityEngine;
using System.Collections.Generic;
using UniRx;

public class EnemyCreater : MonoBehaviour
{
    private enum EnemyCreaterState
    {
        Wait,
        Create
    }

    [SerializeField] private List<EnemyBase> _activeEnemyBase = new ();

    [SerializeField] private Transform _enemyRoot;

    private int _maxEnemyValue = 5;

    private void Awake()
    {
        for (int i = 0; i < _maxEnemyValue; i++)
        {

            SetEnemyCreaterState(EnemyCreaterState.Create);

        }
    }

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

    private void CreateEnemy()
    {
        if(_activeEnemyBase.Count < _maxEnemyValue)
        {
            //あとでリソースはAwakeで1回だけロードするようにする
            var prefab = Resources.Load<GameObject>("Prefabs/Enemy/EnemyBase");
            EnemyBase enemyBase = Instantiate(prefab, _enemyRoot)
                         .GetComponent<EnemyBase>();
            enemyBase.transform.parent = _enemyRoot;
            _activeEnemyBase.Add(enemyBase);
            enemyBase.Killed.Subscribe(_ =>
            {
                _activeEnemyBase.Remove(enemyBase);
                SetEnemyCreaterState(EnemyCreaterState.Create);
            }).AddTo(this);
            SetEnemyCreaterState(EnemyCreaterState.Wait);
        }
       
    }

    private Vector3 enemyRandomPosReturn()
    {
        //XYのランダムPosを返す予定
        Vector3 pos = Vector3.zero;

        return pos;
    }

}
