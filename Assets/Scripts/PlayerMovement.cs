using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;
        rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);

        if (moveDirection.magnitude <= Vector3.kEpsilon)
            return;

        rb.linearVelocity = moveDirection * speed + new Vector3(0f, rb.linearVelocity.y, 0f);
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 15f);
    }
}
