using UnityEngine;

public class CrateSynchronizationHandler : SynchronizationHandler
{
    [SerializeField] private int index;
    public int Index
    {
        get { return index; }
        set
        {
            if (index == value) return;
            index = value;
        }
    }

    protected override void SendMoving(Transform transform)
    {
        GameManager.Instance.SendCrateMoving(Index);
    }
}
