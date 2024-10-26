using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public bool isActive;
    public bool isShooting;
    public bool isTakeControl;
    public bool isFacingLeft;
    public float fireRate = 1f;
    public int damage;
    public WeaponType type;
    public BulletType[] bulletTypes;
    public Transform gunPoint;
    public BoxCollider2D boxCollider2D;
    public SpriteRenderer SpriteRenderer;
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

    public virtual Bullet ShootOneBullet(int bulletIdx, int bulletDamage, int bulletSpeed, Vector2 direction)
    {
        if (direction == Vector2.zero)
        {
            direction = isFacingLeft ? -transform.right : transform.right;
        }
        Bullet bullet = BulletFactory.Instance.CreateBullet(bulletTypes[bulletIdx], transform);
        bullet.transform.position = gunPoint.transform.position;
        bullet.rb2d.AddForce(direction * bulletSpeed, ForceMode2D.Impulse);
        bullet.damage = bulletDamage;
        return bullet;
    }
}
