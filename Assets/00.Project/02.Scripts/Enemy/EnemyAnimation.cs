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
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("IDLE"))
        return;
        animator.ResetTrigger("Idle");
        animator.SetTrigger("Idle");
    }

    public void PlayChase()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("CHASE"))
        return;
        animator.ResetTrigger("Chase");
        animator.SetTrigger("Chase");
    }

    public void PlayAttack()
    {
      //  if(animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
      //  return;
       // animator.ResetTrigger("Attack");
       // animator.SetTrigger("Attack");
        animator.Play("ATTACK", 0, 0f);
    }

    public void PlayHit()
    {
        //if(animator.GetCurrentAnimatorStateInfo(0).IsName("GetHit"))
        //return;
        //animator.ResetTrigger("GetHit");
        //animator.SetTrigger("GetHit");
        animator.Play("GETHIT", 0, 0f);
    }

    public void PlayWalk()
    {
        animator.SetTrigger("Walk");
    }

    public void PlayDeath()
    {
        animator.SetTrigger("Death");
    }
}