using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public float spawnInterval = 5f;
    public Transform spawnQuad; // 스폰 범위를 정의할 Quad

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            Vector3 spawnPos = GetRandomPointOnQuad(spawnQuad);
            EnemyPoolManager.Instance.GetEnemy(spawnPos);
        }
    }

    Vector3 GetRandomPointOnQuad(Transform quad)
    {
        Vector3 center = quad.position;
        Vector3 scale = quad.localScale;

        // 전체 영역의 40%만 사용 (중심 기준)
        float rangeFactor = 0.4f;

        float halfWidth = scale.x * rangeFactor;
        float halfHeight = scale.z * rangeFactor;

        float randX = Random.Range(-halfWidth, halfWidth);
        float randZ = Random.Range(-halfHeight, halfHeight);

        return new Vector3(center.x + randX, center.y, center.z + randZ);
    }
}
