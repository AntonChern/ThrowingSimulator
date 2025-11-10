using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerInput : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerSynchronizationHandler synchronizer;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        synchronizer = GetComponent<PlayerSynchronizationHandler>();
    }

    private void Update()
    {
        Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;
        GameManager.Instance.SendPlayerMovement(synchronizer.Id, movement);
    }
}
