using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimation : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayIdle()
    {
        animator.SetTrigger("Idle");
    }

    public void PlayChase()
    {
        animator.SetTrigger("Chase");
    }

    public void PlayAttack()
    {
        animator.SetTrigger("Attack");
    }

    public void PlayHit()
    {
        animator.SetTrigger("GetHit");
    }

    public void PlayDeath()
    {
        animator.SetTrigger("Death");
    }
}