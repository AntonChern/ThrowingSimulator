using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float gravity;
    private CharacterController controller;
    private Vector3 velocity;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Vector3 moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = 0f; // Reset vertical velocity if grounded
        }

        velocity.y -= gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (moveDirection.magnitude <= Vector3.kEpsilon)
            return;

        //if (!controller.isGrounded)
        //{
        //    moveDirection.y -= gravity * Time.deltaTime;
        //}
        controller.Move(moveDirection * speed * Time.deltaTime);
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
    }
}
