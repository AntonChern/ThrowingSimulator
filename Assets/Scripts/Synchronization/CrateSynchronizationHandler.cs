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
    public Vector3 velocity;
    private Rigidbody rb;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!isAuthor && !rb.isKinematic)
        {
            rb.linearVelocity = velocity;
        }
    }

    protected override void Update()
    {
        base.Update();
        if (isAuthor)
        {
            GameManager.Instance.SendCrateVelocity(index);
        }
    }

    public void UpdateVelocity(Vector3 velocity)
    {
        if (rb)
        {
            this.velocity = velocity;
        }
    }

    protected override void SendMoving()
    {
        GameManager.Instance.SendCrateMoving(Index);
    }
}
