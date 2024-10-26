using System.Collections;
using UnityEngine;

public class TailWeaponIdleState : IdleState
{
    private readonly TailWeapon weapon;

    public TailWeaponIdleState(TailWeapon w)
    {
        weapon = w;
    }

    public override void Enter()
    {
        weapon.transform.localScale = Vector3.one;
    }

    public override void Update()
    {
        if (weapon.isShooting)
        {
            weapon.fsm.ChangeState(weapon.chargingState);
        }
    }
}

public class TailWeaponChargingState : State
{
    private readonly TailWeapon weapon;
    private float coolTime = 0;

    public TailWeaponChargingState(TailWeapon w)
    {
        weapon = w;
    }

    public override void Enter()
    {
        weapon.chargingPercentage = 0;
    }

    public override void Exit()
    {
        weapon.GetHigh();
        weapon.transform.localScale = Vector3.one;
    }

    public override void Update()
    {
        if (weapon.isShooting)
        {
            weapon.chargingPercentage += 0.002f;
            var transform = weapon.transform;
            
            transform.localScale += new Vector3(0.003f, 0.0015f, 0);
            
            float shakeAmount = 0.01f;
            float shakeFrequency = 10f;
            float shakeOffsetX = Mathf.Sin(Time.time * shakeFrequency * Mathf.PI * 2) * shakeAmount;
            float shakeOffsetY = Mathf.Sin(Time.time * shakeFrequency * Mathf.PI * 2 + Mathf.PI / 2) * shakeAmount;
            transform.localPosition += new Vector3(shakeOffsetX, shakeOffsetY, 0);
            
            coolTime += Time.deltaTime;
            if (coolTime >= 0.1f)
            {
                weapon.ShootOneBullet1();
                coolTime = 0;
            }
            
            if (transform.localScale.x > 3)
            {
                weapon.fsm.ChangeState(weapon.coolState);
            }
        }
        else
        {
            weapon.fsm.ChangeState(weapon.coolState);
        }
    }

}

public class TailWeaponCoolState : IdleState
{
    private readonly TailWeapon weapon;
    private float currentCoolDownTime;
    public float coolDownTime;  // Minimal cool down time

    public TailWeaponCoolState(TailWeapon w)
    {
        weapon = w;
    }

    public override void Enter()
    {
        currentCoolDownTime = coolDownTime * (1 + 5 * weapon.chargingPercentage);
    }

    public override void Update()
    {
        if (currentCoolDownTime > 0)
        {
            currentCoolDownTime -= Time.deltaTime;
        }
        else
        {
            weapon.fsm.ChangeState(weapon.idleState);
        }
    }
}

public class TailWeapon : Weapon
{
    public TailWeaponChargingState chargingState;
    public TailWeaponCoolState coolState;
    public float chargingPercentage = 0;

    public new void Awake()
    {
        base.Awake();
        idleState = new TailWeaponIdleState(this);
        chargingState = new TailWeaponChargingState(this);
        coolState = new TailWeaponCoolState(this)
        {
            coolDownTime = 0.2f
        };
        fsm.ChangeState(idleState);
    }
    
    public override void StartShoot()
    {
        base.StartShoot();
        if (fsm.currentState == idleState)
        {
            fsm.ChangeState(chargingState);
        }
    }
    
    public void ShootOneBullet0(Vector2 direction)
    {
        Bullet bullet = BulletFactory.Instance.CreateBullet(bulletTypes[0], transform);
        bullet.transform.localScale = new Vector3(0.5f, 0.5f, 0);
        Rigidbody2D bulletRb = bullet.gameObject.GetComponent<Rigidbody2D>();
        bulletRb.AddForce(direction * bulletSpeed / 3, ForceMode2D.Impulse);
        bullet.damage = (int)(100 * (1 + 4 * chargingPercentage));
    }
    
    public void ShootOneBullet1()
    {
        Bullet bullet = BulletFactory.Instance.CreateBullet(bulletTypes[1], transform);
        bullet.transform.localScale = new Vector3(0.2f, 0.2f, 0);
        Rigidbody2D bulletRb = bullet.gameObject.GetComponent<Rigidbody2D>();
        bulletRb.AddForce((isFacingLeft ? -transform.right : transform.right) * bulletSpeed, ForceMode2D.Impulse);
        bullet.damage = 80;
    }
    
    private IEnumerator ShootMultipleTimes(int shootCount)
    {
        for (int i = 0; i < shootCount; i++)
        {
            StartShootMultipleBullets();
            yield return new WaitForSeconds(0.1f);
        }
    }
    
    public void StartShootMultipleBullets()
    {
        int bulletNum = (int)(1 + 4 * chargingPercentage);
        Vector2 direction = isFacingLeft ? -transform.right : transform.right;
        
        float startAngle = -(bulletNum - 1) / 2f * 5f;

        for (int i = 0; i < bulletNum; i++)
        {
            float angleOffset = startAngle + i * 5f;
            Vector2 offsetDirection = Quaternion.Euler(0, 0, angleOffset) * direction;
            ShootOneBullet0(offsetDirection);
        }
    }

    public void GetHigh()
    {
        StartCoroutine(ShootMultipleTimes(3));
    }
}
