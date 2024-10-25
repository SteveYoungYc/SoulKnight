using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public bool isActive;
    public bool isShooting;
    public bool isTakeControl;
    public float bulletSpeed = 10f;
    public float fireRate = 1f;
    public int damage;
    public WeaponType type;
    public BulletType[] bulletTypes;
    public Transform gunPoint;
    public BoxCollider2D boxCollider2D;
    public StateMachine fsm;
    public IdleState idleState;

    public void Awake()
    {
        fsm = new StateMachine();
    }

    public void Update()
    {
        if (isActive)
        {
            fsm.Update();
        }
    }

    public virtual void StartShoot()
    {
        isShooting = true;
    }

    public virtual void StopShoot()
    {
        isShooting = false;
    }
}
