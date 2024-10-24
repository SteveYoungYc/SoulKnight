using UnityEngine;

public class GunWeaponIdleState : State
{
    private readonly BasicGunWeapon weapon;

    public GunWeaponIdleState(BasicGunWeapon w)
    {
        weapon = w;
    }

    public override void Update()
    {
        if (weapon.isShooting)
        {
            weapon.fsm.ChangeState(weapon.coolState);
        }
    }
}

public class GunWeaponCoolState : State
{
    private readonly BasicGunWeapon weapon;
    private float currentCoolDownTime;
    public float coolDownTime;

    public GunWeaponCoolState(BasicGunWeapon w)
    {
        weapon = w;
    }

    public override void Enter()
    {
        currentCoolDownTime = coolDownTime;
        weapon.ShootOneBullet();
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

public class BasicGunWeapon : Weapon
{
    public StateMachine fsm;
    public GunWeaponIdleState idleState;
    public GunWeaponCoolState coolState;
    
    public override void StartShoot()
    {
        base.StartShoot();
        if (fsm.currentState == idleState)
        {
            fsm.ChangeState(coolState);
        }
    }

    public override void StopShoot()
    {
        base.StopShoot();
    }

    public void ShootOneBullet()
    {
        Bullet bullet = BulletFactory.Instance.CreateBullet(bulletTypes[0], transform);
        Rigidbody2D bulletRb = bullet.gameObject.GetComponent<Rigidbody2D>();
        bulletRb.AddForce(transform.right * bulletSpeed, ForceMode2D.Impulse);
        bullet.damage = 50;
    }

    public void Start()
    {
        fireRate = 0.1f;
        bulletSpeed = 20;
        
        fsm = new StateMachine();
        idleState = new GunWeaponIdleState(this);
        coolState = new GunWeaponCoolState(this)
        {
            coolDownTime = fireRate
        };
        fsm.ChangeState(idleState);
    }

    public void Update()
    {
        fsm.Update();
    }
}
