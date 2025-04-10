using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 3f;
    private CharacterController controller;
    private Camera cam;
    PlayerAnimation playerAnimation;
    PlayerState playerState;
    private void Awake()
    {
        playerAnimation = GetComponent<PlayerAnimation>();
        playerState = GetComponent<PlayerState>();
        controller = GetComponent<CharacterController>();
    }
    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        // if (playerState.CanMove() == false)
        // {
        //     return;
        // }
        if (playerState.IsDizzy || playerState.IsAttacking || playerState.IsHit)
            moveSpeed = 1.2f;
        else
            if(moveSpeed != 3f)  moveSpeed = 3f;

        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 camForward = cam.transform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = cam.transform.right;
        camRight.y = 0;
        camRight.Normalize();

        Vector3 move = input.z * camForward + input.x * camRight;

        controller.SimpleMove(move * moveSpeed);
  

        if (move != Vector3.zero)
        {
            transform.forward = move;
            playerAnimation.SetMove();
        }
        else
        {
            playerAnimation.SetIdle();
        }
    }
}
