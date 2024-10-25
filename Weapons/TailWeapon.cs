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
        weapon.ShootOneBullet();
        weapon.transform.localScale = Vector3.one;
    }

    public override void Update()
    {
        if (weapon.isShooting)
        {
            weapon.chargingPercentage += 0.002f;
            var transform = weapon.transform;
            transform.localScale += new Vector3(0.003f, 0.0015f, 0);
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
    
    public void ShootOneBullet()
    {
        Bullet bullet = BulletFactory.Instance.CreateBullet(bulletTypes[0], transform);
        Rigidbody2D bulletRb = bullet.gameObject.GetComponent<Rigidbody2D>();
        bulletRb.AddForce((isFacingLeft ? -transform.right : transform.right) * bulletSpeed, ForceMode2D.Impulse);
        bullet.damage = (int)(100 * (1 + 4 * chargingPercentage));
    }
}
