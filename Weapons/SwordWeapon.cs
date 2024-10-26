﻿using System.Collections;
using UnityEngine;

public class SwordWeaponIdleState : IdleState
{
    private readonly SwordWeapon weapon;

    public SwordWeaponIdleState(SwordWeapon w)
    {
        weapon = w;
    }
    
    public override void Enter()
    {
        weapon.StopAttack();
    }

    public override void Update()
    {
        if (weapon.isShooting)
        {
            weapon.fsm.ChangeState(weapon.coolState);
        }
    }
}

public class SwordWeaponCoolState : State
{
    private readonly SwordWeapon weapon;
    private float currentCoolDownTime;
    public float coolDownTime;

    public SwordWeaponCoolState(SwordWeapon w)
    {
        weapon = w;
    }

    public override void Enter()
    {
        currentCoolDownTime = coolDownTime;
        weapon.AttackOnce();
    }

    public override void Update()
    {
        if (weapon.isActive && currentCoolDownTime > 0)
        {
            currentCoolDownTime -= Time.deltaTime;
        }
        else
        {
            weapon.fsm.ChangeState(weapon.idleState);
        }
    }
}

public class SwordWeapon : Weapon
{
    public SwordWeaponCoolState coolState;

    private bool isAttacking;
    private readonly float quickUpDuration = 0.05f; // Quick upward motion
    private readonly float swingDownDuration = 0.1f; // Normal speed downward swing (double angle)
    private readonly float quickBackUpDuration = 0.05f; // Quick return to start position
    private readonly Vector2 angles = new(-90, 90);
    
    private Coroutine attackCoroutine;
    
    public new void Awake()
    {
        base.Awake();
        fireRate = 0.5f;
        damage = 250;
        isTakeControl = true;
        idleState = new SwordWeaponIdleState(this);
        coolState = new SwordWeaponCoolState(this)
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
    
    public void AttackOnce()
    {
        attackCoroutine = StartCoroutine(SwingSword());
    }
    
    public void StopAttack()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }
    }

    private IEnumerator SwingSword()
    {
        // Capture the initial rotation
        Quaternion startRotation = transform.rotation;

        // Calculate intermediate rotations
        Quaternion quickUpRotation = Quaternion.Euler(0, 0, angles.y); // Quick upward
        Quaternion swingDownRotation = Quaternion.Euler(0, 0, angles.x); // Swing down (past start)
        Quaternion backToStartRotation = startRotation; // Return to original position
        
        float elapsedTime = 0f;

        // 1. Quick upward rotation by swingAngle
        while (elapsedTime < quickUpDuration)
        {
            transform.rotation = Quaternion.Lerp(startRotation, quickUpRotation, elapsedTime / quickUpDuration);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }
        // Ensure it reaches the exact target
        transform.rotation = quickUpRotation;

        // 2. Normal speed downward swing by swingAngle * 2 (relative to current position)
        elapsedTime = 0f;
        isAttacking = true;
        while (elapsedTime < swingDownDuration)
        {
            transform.rotation = Quaternion.Lerp(quickUpRotation, swingDownRotation, elapsedTime / swingDownDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // Ensure it reaches the exact target
        transform.rotation = swingDownRotation;
        isAttacking = false;

        // 3. Quick return to starting position by swingAngle
        elapsedTime = 0f;
        while (elapsedTime < quickBackUpDuration)
        {
            transform.rotation = Quaternion.Lerp(swingDownRotation, backToStartRotation, elapsedTime / quickBackUpDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // Ensure it reaches the exact target
        transform.rotation = backToStartRotation;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isAttacking)
        {
            return;
        }
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}
