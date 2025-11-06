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

    protected override void SendMoving(Transform transform)
    {
        GameManager.Instance.SendPlayerMoving(id);
    }
}
