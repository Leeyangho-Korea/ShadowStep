using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackRange = 1.5f;
    public float attackRate = 1f;
    public int damage = 10;
    public LayerMask enemyLayer;

    //private float nextAttackTime = 0f;

    PlayerState playerState;

    private void Awake()
    {
        playerState = GetComponent<PlayerState>();
    }
    void Update()
    {
        if (playerState.CanAttack())
        {
            if (Input.GetMouseButtonDown(0)) // 좌클릭으로 공격
            {
                Attack();
                //nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    void Attack()
    {
        // 공격상태로 전환
        playerState.SetAttacking(true);

        // 공격 판정 (전방 원형 범위)
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position + transform.forward * attackRange * 0.5f, attackRange, enemyLayer);

        foreach (Collider enemy in hitEnemies)
        {
                Debug.Log("Attack: " + enemy.name);
                enemy.GetComponent<EnemyHealth>()?.TakeDamage(damage);
        }
    }

    // 공격 범위 시각화 (에디터에서만 보임)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward * attackRange * 0.5f, attackRange);
    }
}
