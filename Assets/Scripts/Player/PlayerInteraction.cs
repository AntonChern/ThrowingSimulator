using UnityEngine;

[RequireComponent(typeof(Grabber))]
public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private LayerMask interactMask;
    private Grabber grabber;
    private PlayerSynchronizationHandler synchronizer;
    private int takenObjectIndex = -1;

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
            !collisionSynchronizer.IsAuthor &&
            collisionSynchronizer.velocity.magnitude < 0.1f)
        {
            collisionSynchronizer.IsAuthor = true;
            GameManager.Instance.SendCrateAuthority(
                synchronizer.Id,
                collisionSynchronizer.Index
            );
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (grabber.TakenObject)
            {
                Throw();
            }
            else
            {
                Take();
            }
        }
    }

    private void Throw()
    {
        GameManager.Instance.SendCrateInteracting(
            GetComponent<PlayerSynchronizationHandler>().Id,
            takenObjectIndex,
            false
        );
        takenObjectIndex = -1;
    }

    private GameObject FindInteractableObject()
    {
        RaycastHit hit;
        Collider[] colliders = Physics.OverlapCapsule(transform.position - transform.up * 0.5f, transform.position + transform.up * 0.5f, 0.5f, interactMask);
        if (colliders.Length != 0)
        {
            return colliders[0].gameObject;
        }
        if (Physics.CapsuleCast(transform.position - transform.up * 0.5f, transform.position + transform.up * 0.5f, 0.5f, transform.forward, out hit, 1f, interactMask))
        {
            return hit.collider.gameObject;
        }
        return null;
    }

    private void Take()
    {
        GameObject takenObject = FindInteractableObject();

        if (takenObject)
        {
            var synchronizer = takenObject.GetComponent<CrateSynchronizationHandler>();
            synchronizer.IsAuthor = true;
            takenObjectIndex = synchronizer.Index;
            GameManager.Instance.SendCrateInteracting(
                GetComponent<PlayerSynchronizationHandler>().Id,
                takenObjectIndex,
                true
            );
        }
    }
}
