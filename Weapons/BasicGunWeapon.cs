using UnityEngine;

public class GunWeaponIdleState : IdleState
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
        weapon.ShootOneBullet(0, 100, 20, Vector2.zero);
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
    public GunWeaponCoolState coolState;
    
    public new void Awake()
    {
        base.Awake();
        fireRate = 0.1f;

        idleState = new GunWeaponIdleState(this);
        coolState = new GunWeaponCoolState(this)
        {
            coolDownTime = fireRate
        };
        fsm.ChangeState(idleState);
    }
    
    public override void StartShoot()
    {
        base.StartShoot();
        if (fsm.currentState == idleState)
        {
            fsm.ChangeState(coolState);
        }
    }
}
