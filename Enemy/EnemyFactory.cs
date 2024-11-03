using System;
using UnityEngine;
using Object = UnityEngine.Object;

public enum EnemyType
{
    Enemy03,
}

public class EnemyFactory
{
    private static EnemyFactory _instance;
    
    private EnemyFactory()
    {
    }
    
    public static EnemyFactory Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new EnemyFactory();
            }
            return _instance;
        }
    }
    
    private GameObject CreateEnemyGameObject(string path)
    {
        GameObject prefab = Resources.Load<GameObject>(path);
        GameObject gameObject = Object.Instantiate(prefab);
        return gameObject;
    }

    public Enemy CreateEnemy(EnemyType type)
    {
        Enemy enemy = CreateEnemyGameObject("Prefabs/Enemy").GetComponent<Enemy>();
        switch (type)
        {
            case EnemyType.Enemy03:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
        return enemy;
    }
}
