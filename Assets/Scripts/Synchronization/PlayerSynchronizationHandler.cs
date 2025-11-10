using UnityEngine;

public class PlayerSynchronizationHandler : SynchronizationHandler
{
    [SerializeField] private string id;
    public string Id
    {
        get { return id; }
        set
        {
            if (id == value) return;
            id = value;
        }
    }
    private PlayerMovement playerMovement;

    protected override void Start()
    {
        base.Start();
        playerMovement = GetComponent<PlayerMovement>();
    }

    public void UpdateMovement(Vector3 movement)
    {
        if (playerMovement)
        {
            playerMovement.movement = movement;
        }
    }

    protected override void SendMoving()
    {
        GameManager.Instance.SendPlayerLocation(id);
    }
}
