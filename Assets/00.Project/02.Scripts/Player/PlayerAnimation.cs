using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerState))]
public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private PlayerState playerState;
    private float[] layerTemp;
    private Dictionary<string, float> animationClipLengths = new();

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        playerState = GetComponent<PlayerState>();
        layerTemp = new float[3];
    }

    void Start()
    {
        // delegate 기반 상태 이벤트 연결
        playerState.OnDie += PlayDie;
        playerState.OnAttackStart += SetAttackLayer;
        playerState.OnHitStart += SetHitLayer;
        playerState.OnDizzyStart += SetDizzyLayer;
    }

    private void Update()
    {
        for (int i = 1; i <= 3; i++)
        {
            if (animator.GetLayerWeight(i) > 0)
            {
                layerTemp[i - 1] -= Time.deltaTime;
                animator.SetLayerWeight(i, layerTemp[i - 1]);

                if (layerTemp[i - 1] <= 0f)
                {
                    animator.SetLayerWeight(i, 0f);
                    switch (i)
                    {
                        case 1:
                            playerState.InvokeAttackEnd();
                            break;
                        case 2:
                            playerState.InvokeHitEnd();
                            break;
                        case 3:
                            playerState.InvokeDizzyEnd();
                            break;
                    }
                }
            }
        }
    }

    public void SetMove()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("WalkForward")) return;
        animator.SetTrigger("Walk");
    }

    public void SetIdle()
    {
        if (playerState.IsDead || animator.GetCurrentAnimatorStateInfo(0).IsName("Idle01")) return;
        animator.SetTrigger("Idle");
    }

    public void PlayDie()
    {
        animator.SetTrigger("Die");
    }

    public void SetAttackLayer()
    {
        SetLayer(1, "Attack01");
    }

    public void SetHitLayer()
    {
        SetLayer(2, "GetHit");
    }

    public void SetDizzyLayer()
    {
        SetLayer(3, "Dizzy");
    }

    public void SetLayer(int layerIndex, string animationName, bool enabled = true)
    {
        if (enabled)
        {
            // 애니메이션 클립 길이를 안전하게 가져오기 위해 ClipInfo 사용
            AnimatorClipInfo[] clipInfos = animator.GetCurrentAnimatorClipInfo(layerIndex);
            if (clipInfos.Length > 0)
            {
                layerTemp[layerIndex - 1] = clipInfos[0].clip.length;
            }
            else
            {
                layerTemp[layerIndex - 1] = 1f; // 기본값
            }

            animator.SetLayerWeight(layerIndex, 1f);
            animator.Play(animationName, layerIndex, 0f);
        }
        else
        {
            animator.SetLayerWeight(layerIndex, 0f);
            layerTemp[layerIndex - 1] = 0f;
        }
    }
}
