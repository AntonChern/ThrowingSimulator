using UnityEngine;

public abstract class SynchronizationHandler : MonoBehaviour
{
    [SerializeField] protected bool isAuthor = false;
    public bool IsAuthor
    {
        get { return isAuthor; }
        set
        {
            if (isAuthor == value) return;
            isAuthor = value;
        }
    }

    public bool changable = true;

    private Vector3 actualPosition;
    private Quaternion actualRotation;

    private Vector3 startPosition;
    private Vector3 targetPosition;

    private Quaternion startRotation;
    private Quaternion targetRotation;

    private float timer = 0f;
    private float synchronizationStep = 0.1f;

    private void Start()
    {
        actualPosition = transform.position;
        actualRotation = transform.rotation;

        SetInterpolatingPoints();
    }

    public void UpdateActualState(Vector3 position, Quaternion rotation)
    {
        actualPosition = position;
        actualRotation = rotation;
        SetInterpolatingPoints();
    }

    protected abstract void SendMoving(Transform transform);

    protected void Update()
    {
        if (isAuthor)
        {
            SendMoving(transform);
            if (transform.position.y < -1f)
            {
                transform.position = Vector3.zero + Vector3.up;
            }
        }
    }

    protected void SetInterpolatingPoints()
    {
        targetPosition = actualPosition;
        targetRotation = actualRotation;

        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    protected void LateUpdate()
    {
        if (!isAuthor)
        {
            if (changable)
            {
                timer += Time.deltaTime;
                transform.position = Vector3.Lerp(startPosition, targetPosition, timer / synchronizationStep);
                transform.rotation = Quaternion.Lerp(startRotation, targetRotation, timer / synchronizationStep);
                if (timer >= synchronizationStep)
                {
                    timer = 0f;
                    SetInterpolatingPoints();
                }
            }
            else
            {
                SetInterpolatingPoints();
            }
        }
    }
}
