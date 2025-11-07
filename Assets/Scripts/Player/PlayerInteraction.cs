using UnityEngine;

[RequireComponent(typeof(Grabber))]
public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private LayerMask interactMask;
    private Grabber grabber;
    private PlayerSynchronizationHandler synchronizer;

    private void Start()
    {
        interactMask = (1 << LayerMask.NameToLayer("Interactable"));
        grabber = GetComponent<Grabber>();
        synchronizer = GetComponent<PlayerSynchronizationHandler>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        var collisionSynchronizer = collision.gameObject.GetComponent<CrateSynchronizationHandler>();
        if (synchronizer.IsAuthor &&
            collisionSynchronizer != null &&
            !collisionSynchronizer.IsAuthor)
        {
            GameManager.Instance.SendCrateInteracting(
                synchronizer.Id,
                collisionSynchronizer.Index,
                false
            );
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (grabber.TakenObject)
            {
                grabber.Throw(true);
            }
            else
            {
                Take();
            }
        }
    }

    public void Take()
    {
        RaycastHit hit;
        Collider[] colliders = Physics.OverlapCapsule(transform.position - transform.up * 0.5f, transform.position + transform.up * 0.5f, 0.5f, interactMask);
        if (colliders.Length != 0)
        {
            grabber.Take(colliders[0].gameObject, true);
        }
        else
        {
            if (Physics.CapsuleCast(transform.position - transform.up * 0.5f, transform.position + transform.up * 0.5f, 0.5f, transform.forward, out hit, 1f, interactMask))
            {
                grabber.Take(hit.collider.gameObject, true);
            }
        }
    }
}
