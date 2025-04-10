using UnityEngine;
using System;

[RequireComponent(typeof(PlayerAnimation))]
public class PlayerState : MonoBehaviour
{
    [Header("Health")]
    public int maxHP = 100;
    public int currentHP { get; private set; }

    public bool IsDead;
    public bool IsDizzy;
    public bool IsAttacking;
    public bool IsHit;

    public event Action OnDie;
    public event Action OnHitStart;
    public event Action OnHitEnd;
    public event Action OnAttackStart;
    public event Action OnAttackEnd;
    public event Action OnDizzyStart;
    public event Action OnDizzyEnd;
    public event Action<int> OnHPChanged;

    private void Start()
    {
        currentHP = maxHP;

        // delegate 연결
        OnHPChanged += HpChange;
        OnHitEnd += () => IsHit = false;
        OnDizzyEnd += () => IsDizzy = false;
        OnAttackEnd += () => IsAttacking = false;
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Alpha0)) TakeDamage(10);
        if (Input.GetKeyDown(KeyCode.Alpha1)) Heal(10);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SetDizzy(true);
#endif
    }

    public void Heal(int amount)
    {
        currentHP = Mathf.Min(currentHP + amount, maxHP);
        OnHPChanged?.Invoke(currentHP);
    }

    public void Die()
    {
        IsDead = true;
        OnDie?.Invoke();
    }

    public void TakeDamage(int amount, bool setHit = true)
    {
        if (IsDead) return;

        if (setHit)
        {
            currentHP -= amount;
            OnHitStart?.Invoke();
            OnHPChanged?.Invoke(currentHP);
            IsHit = true;

            if (currentHP <= 0) Die();
        }
        else
        {
            IsHit = false;
        }
    }

    public void SetDizzy(bool value)
    {
        if (value && !IsDizzy)
        {
            IsDizzy = true;
            OnDizzyStart?.Invoke();
        }
        else if (!value)
        {
            IsDizzy = false;
        }
    }

    public void SetAttacking(bool value)
    {
        if (value && !IsAttacking)
        {
            IsAttacking = true;
            OnAttackStart?.Invoke();
        }
        else if (!value)
        {
            IsAttacking = false;
        }
    }

    private void HpChange(int hp)
    {
        Debug.Log($"HP Changed: {hp}");
    }

    public bool CanMove() => !IsDead && !IsDizzy;
    public bool CanAttack() => !IsDead && !IsDizzy && !IsAttacking;

    public void InvokeAttackEnd() => OnAttackEnd?.Invoke();
    public void InvokeHitEnd() => OnHitEnd?.Invoke();
    public void InvokeDizzyEnd() => OnDizzyEnd?.Invoke();
}