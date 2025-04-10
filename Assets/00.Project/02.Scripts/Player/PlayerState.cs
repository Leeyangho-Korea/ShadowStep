using UnityEngine;
using UnityEngine.Events;

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

    public UnityEvent OnDie;
    public UnityEvent OnHitStart;
    public UnityEvent OnHitEnd;
    public UnityEvent OnAttackStart;
    public UnityEvent OnAttackEnd;
    public UnityEvent OnDizzyStart;
    public UnityEvent OnDizzyEnd;
    public UnityEvent<int> OnHPChanged;

    private void Start()
    {
        currentHP = maxHP;
        OnHPChanged.AddListener(HpChange);
        OnHitEnd.AddListener(() => IsHit = false);
        OnDizzyEnd.AddListener(() => IsDizzy = false);
        OnAttackEnd.AddListener(() => IsAttacking = false);
    }

    private void Update()
    {
#if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            TakeDamage(10);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Heal(10);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetDizzy(true);
        }
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

    // 이동중에 다른 애니메이션이 가능할수 있도록 애니메이션을 마스크 레이어로 구성.
    public bool CanMove() => !IsDead && !IsDizzy;
    public bool CanAttack() => !IsDead && !IsDizzy && !IsAttacking;
}