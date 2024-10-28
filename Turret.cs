using System;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public Weapon weapon;
    public EnemySpawner enemySpawner;

    public void Start()
    {
        GameObject enemySpawnerObj = GameObject.Find("EnemySpawner");
        enemySpawner = enemySpawnerObj.GetComponent<EnemySpawner>();
        if (enemySpawner == null)
        {
            Debug.LogError("enemySpawner is null!");
        }

        weapon = WeaponFactory.Instance.CreateWeapon(WeaponType.Tail, transform);
        weapon.transform.localPosition = Vector3.zero;
        weapon.isActive = true;
    }

    public void Update()
    {
        float minDistance = float.MaxValue;
        GameObject selectedEnemyObj = null;
        foreach (GameObject enemyObj in enemySpawner.enemies)
        {
            float distance = Vector2.Distance(transform.position, enemyObj.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                selectedEnemyObj = enemyObj;
            }
        }

        if (selectedEnemyObj != null)
        {
            HandleWeaponAim(selectedEnemyObj.transform.position);
            weapon.StartShoot();
        }
        else
        {
            weapon.StopShoot();
        }
    }
    
    void HandleWeaponAim(Vector3 targetPosition)
    {
        bool isFacingLeft = targetPosition.x < transform.position.x;
        weapon.isFacingLeft = isFacingLeft;
        transform.localScale = new Vector3(isFacingLeft ? -1 : 1, 1, 1);
        
        if (!weapon.isTakeControl)
        {
            Vector2 lookDir = (Vector2)targetPosition - (Vector2)weapon.transform.position;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
            weapon.transform.rotation = Quaternion.Euler(0f, 0f, isFacingLeft ? angle - 180 : angle);
        }
    }
}
