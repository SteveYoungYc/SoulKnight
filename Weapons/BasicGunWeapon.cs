using UnityEngine;

public class GunWeaponIdleState : State
{
    private readonly BasicGunWeapon weapon;

    public GunWeaponIdleState(BasicGunWeapon w)
    {
        weapon = w;
    }

    public override void Enter()
    {
    }

    public override void Exit()
    {
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

    public override void Exit()
    {
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
        var transform1 = transform;
        GameObject bulletPrefab = Resources.Load<GameObject>("Prefabs/Bullet");
        GameObject bullet = Instantiate(bulletPrefab, transform1.position, transform1.rotation);
        bullet.transform.localScale = new Vector3(1, 1, 1);
        SpriteRenderer spriteRenderer = bullet.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = bulletSprite;
        spriteRenderer.sortingOrder = 6;
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.AddForce(transform.right * bulletSpeed, ForceMode2D.Impulse);
    }

    public void Start()
    {
        fireRate = 1f;
        bulletSpeed = 20;
        
        fsm = new StateMachine();
        idleState = new GunWeaponIdleState(this);
        coolState = new GunWeaponCoolState(this);
        coolState.coolDownTime = 1f;
        fsm.ChangeState(idleState);
    }

    public void Update()
    {
        fsm.Update();
    }
}
