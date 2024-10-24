using UnityEngine;

public class TailWeaponIdleState : State
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

    public override void Exit()
    {
    }

    public override void Update()
    {
        if (weapon.isShooting)
        {
            weapon.fsm.ChangeState(weapon.biggerState);
        }
    }
}

public class TailWeaponBiggerState : State
{
    private readonly TailWeapon weapon;
    private int count;

    public TailWeaponBiggerState(TailWeapon w)
    {
        weapon = w;
    }

    public override void Enter()
    {
        count = 0;
    }

    public override void Exit()
    {
    }

    public override void Update()
    {
        if (weapon.isShooting)
        {
            count++;
            var transform = weapon.transform;
            transform.localScale += new Vector3(0.5f, 0.02f, 0);
            weapon.ShootOneBullet();
            if (count == 10)
            {
                weapon.fsm.ChangeState(weapon.idleState);
            }
            weapon.isShooting = false;
        }
    }
}

public class TailWeapon : Weapon
{
    public StateMachine fsm;
    public TailWeaponIdleState idleState;
    public TailWeaponBiggerState biggerState;

    public void Start()
    {
        fsm = new StateMachine();
        idleState = new TailWeaponIdleState(this);
        biggerState = new TailWeaponBiggerState(this);
        fsm.ChangeState(idleState);
    }
    
    public void Update()
    {
        fsm.Update();
    }
    
    public override void StartShoot()
    {
        base.StartShoot();
    }
    
    public void ShootOneBullet()
    {
        Bullet bullet = BulletFactory.Instance.CreateBullet(bulletTypes[0], transform);
        Rigidbody2D bulletRb = bullet.gameObject.GetComponent<Rigidbody2D>();
        bulletRb.AddForce(transform.right * bulletSpeed, ForceMode2D.Impulse);
        bullet.damage = 500;
    }

    public void AttackOnce()
    {
        
    }
}
