using UnityEngine;
using UnityEngine.AI;
using System;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyAnimation))]
public class EnemyState : MonoBehaviour
{
    public float chaseDistance = 10f;
    public float attackDistance = 2f;
    public float attackCooldown = 2f;
    public int maxHP = 100;

    private int currentHP;
    private Vector3 startPosition;
    private NavMeshAgent agent;
    private EnemyAnimation enemyAnim;
    private Transform player;
    private PlayerState playerState;

    private bool isDead = false;
    private float lastAttackTime = 0f;

    private enum State { Idle, Chase, Attack, Return }
    private State currentState = State.Idle;

    void Start()
    {
        currentHP = maxHP;
        agent = GetComponent<NavMeshAgent>();
        enemyAnim = GetComponent<EnemyAnimation>();
        startPosition = transform.position;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerState = playerObj.GetComponent<PlayerState>();
        }

        enemyAnim.PlayIdle();
    }

    void Update()
    {
        if (isDead || player == null) return;

        float distToPlayer = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case State.Idle:
                if (distToPlayer <= chaseDistance)
                {
                    currentState = State.Chase;
                    enemyAnim.PlayChase();
                }
                break;

            case State.Chase:
                if (distToPlayer > chaseDistance)
                {
                    currentState = State.Return;
                    agent.SetDestination(startPosition);
                    enemyAnim.PlayIdle();
                }
                else if (distToPlayer <= attackDistance)
                {
                    currentState = State.Attack;
                    agent.ResetPath();
                    enemyAnim.PlayAttack();
                }
                else
                {
                    agent.SetDestination(player.position);
                }
                break;

            case State.Attack:
                transform.LookAt(player);

                if (Time.time - lastAttackTime >= attackCooldown)
                {
                    lastAttackTime = Time.time;
                    enemyAnim.PlayAttack();
                    playerState?.TakeDamage(10); // 플레이어에게 데미지 줌
                }

                if (distToPlayer > attackDistance)
                {
                    currentState = State.Chase;
                    enemyAnim.PlayChase();
                }
                break;

            case State.Return:
                float distToStart = Vector3.Distance(transform.position, startPosition);
                if (distToStart <= 0.5f)
                {
                    currentState = State.Idle;
                    enemyAnim.PlayIdle();
                    agent.ResetPath();
                }
                break;
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHP -= damage;
        enemyAnim.PlayHit();

        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        agent.enabled = false;
        enemyAnim.PlayDeath();
        Invoke(nameof(DisableEnemy), 3f); // 3초 후 비활성화 (풀링용)
    }

    private void DisableEnemy()
    {
        gameObject.SetActive(false); // 나중에 풀에서 재사용 예정
    }
}