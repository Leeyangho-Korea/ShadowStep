using UnityEngine;
using UnityEngine.AI;
using System;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyAnimation))]
public class EnemyState : MonoBehaviour
{
    public EnemyHealthBar healthBar;
    public float chaseDistance = 10f;
    public float attackDistance = 2f;
    public float attackCooldown = 10f;
    public int maxHP = 100;

    private int currentHP;
    private Vector3 startPosition;
    private NavMeshAgent agent;
    private EnemyAnimation enemyAnim;
    private Transform player;
    private PlayerState playerState;

    private bool isDead = false;
    private float lastAttackTime = 0f;

    private enum State { Idle, Chase, Attack, Return}
    [SerializeField] private State currentState = State.Idle;

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
                    enemyAnim.PlayChase();
                    currentState = State.Chase;
                }
                break;

            case State.Chase:
            enemyAnim.PlayChase();
                if (distToPlayer > chaseDistance)
                {
                    enemyAnim.PlayWalk();
                    currentState = State.Return;
                    agent.SetDestination(startPosition);
                }
                else if (distToPlayer <= attackDistance)
                {
                    //if(CanAttack())
                    //{
                          enemyAnim.PlayIdle();
                        currentState = State.Attack;
                       // if(CanAttack() == false)
                       // {
                          
                       // }
                    //}
                    //enemyAnim.PlayAttack();
                    
                    agent.ResetPath();
                }
                else
                {
                    agent.SetDestination(player.position);
                }
                break;

            case State.Attack:
                transform.LookAt(player);

            

                if (distToPlayer > attackDistance)
                {
                    enemyAnim.PlayChase();
                    currentState = State.Chase;
                }
                else
                {
                    if (CanAttack())
                    {
                    lastAttackTime = Time.time;
                    enemyAnim.PlayAttack();
                    playerState?.TakeDamage(10); // 플레이어에게 데미지 줌
                    }
                }
                break;

            case State.Return:

                   if (distToPlayer <= chaseDistance)
                {
                    enemyAnim.PlayChase();
                    currentState = State.Chase;
                }
                else
                {
                    float distToStart = Vector3.Distance(transform.position, startPosition);
                    if (distToStart <= 0.5f)
                    {
                        enemyAnim.PlayIdle();
                        currentState = State.Idle;
                        agent.ResetPath();
                    }
                }
              
                break;
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;
        //공격 쿨 늘려서 자연스럽게
        lastAttackTime = Time.time;
        currentHP -= damage;

        enemyAnim.PlayHit();

        currentHP = currentHP < 0 ? 0 : currentHP;

        healthBar.slider.value = currentHP;
        if (currentHP <= 0)
        {
            Die();
        }
    }

    private bool CanAttack()
    {
        return Time.time - lastAttackTime >= attackCooldown;
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