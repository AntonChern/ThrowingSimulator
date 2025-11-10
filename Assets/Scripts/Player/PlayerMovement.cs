using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 6;
    private Rigidbody rb;
    public Vector3 movement = Vector3.zero;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);

        if (movement.magnitude <= Vector3.kEpsilon)
            return;

        var moveDirection = movement * speed + new Vector3(0f, rb.linearVelocity.y, 0f); 
        rb.linearVelocity = moveDirection;
        Quaternion targetRotation = Quaternion.LookRotation(movement);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 15f);
    }
}
