// ✅ PlayerController.cs (혼란 상태에서 랜덤 이동 적용)
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float rotationSmoothness = 10f;

    private CharacterController controller;
    private Camera cam;
    private Vector3 lastMoveDirection = Vector3.zero;
    private Vector3 rotateDirection = Vector3.forward;
    private Vector2 scrambleDir = Vector2.zero;

    private PlayerAnimation playerAnimation;
    private PlayerState playerState;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerAnimation = GetComponent<PlayerAnimation>();
        playerState = GetComponent<PlayerState>();
    }

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (cam == null)
        {
            cam = Camera.main;
            if (cam == null) return;
        }

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 input = new Vector3(h, 0, v).normalized;

        // 혼란 상태일 때 랜덤 방향 고정
        if (playerState.IsDizzy)
        {
            if (input.magnitude > 0.1f && scrambleDir == Vector2.zero)
            {
                scrambleDir = Random.insideUnitCircle.normalized;
            }

            if (input.magnitude < 0.1f)
            {
                scrambleDir = Vector2.zero;
            }
        }

        Vector3 camForward = cam.transform.forward;
        Vector3 camRight = cam.transform.right;

        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 move;

        if (playerState.IsDizzy && scrambleDir != Vector2.zero)
        {
            move = (v * scrambleDir.y * camForward + h * scrambleDir.x * camRight).normalized;
        }
        else
        {
            move = (v * camForward + h * camRight).normalized;
        }

        if (move != Vector3.zero)
        {
            lastMoveDirection = move;
            controller.Move(move * moveSpeed * Time.deltaTime);

            float angleToNew = Vector3.Angle(rotateDirection, move);
            if (angleToNew > 1f)
            {
                float angle = Vector3.Angle(transform.forward, move);
                float dampFactor = Mathf.Clamp01(1f - (angle / 180f));
                rotateDirection = Vector3.Slerp(rotateDirection, move, rotationSmoothness * dampFactor * Time.deltaTime);
            }

            Quaternion toRotation = Quaternion.LookRotation(rotateDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);

            playerAnimation.SetMove();
        }
        else
        {
            playerAnimation.SetIdle();
        }
    }
}