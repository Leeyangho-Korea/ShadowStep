using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Slider slider;
    public Transform target; // 따라갈 대상 (몬스터 상단)
    public Vector3 offset = new Vector3(0, 2f, 0); // 몬스터 위에 표시




    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
            transform.LookAt(Camera.main.transform); // 항상 카메라를 향하도록
        }
    }

    public void SetHealth(float normalizedValue)
    {
        slider.value = normalizedValue;
    }
}
