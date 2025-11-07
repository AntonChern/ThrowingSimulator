using UnityEngine;

public class Grabber : MonoBehaviour
{
    [SerializeField] private GameObject takenObject;
    private int takenObjectIndex;
    [SerializeField] private GameObject crateCollider;

    public GameObject TakenObject
    {
        get { return takenObject; }
        set
        {
            if (takenObject == value) return;
            takenObject = value;
        }
    }

    private Vector3 objectPosition = new Vector3(0f, -0.5f, 1f);
    private Vector3 objectRotation = new Vector3(0f, 0f, 0f);

    private void LateUpdate()
    {
        if (takenObject)
        {
            takenObject.transform.localPosition = objectPosition;
            takenObject.transform.localEulerAngles = objectRotation;
        }
    }

    public void Take(GameObject gameObject, bool needToSend)
    {
        takenObject = gameObject;
        var synchronizer = takenObject.GetComponent<CrateSynchronizationHandler>();
        takenObjectIndex = synchronizer.Index;
        crateCollider.SetActive(true);
        takenObject.transform.SetParent(transform);
        takenObject.GetComponent<Rigidbody>().isKinematic = true;
        takenObject.GetComponent<BoxCollider>().enabled = false;
        synchronizer.changable = false;
        if (needToSend)
        {
            GameManager.Instance.SendCrateInteracting(
                GetComponent<PlayerSynchronizationHandler>().Id,
                takenObjectIndex,
                true
            );
        }
    }

    public void Throw(bool needToSend)
    {
        if (!takenObject) return;
        crateCollider.SetActive(false);
        takenObject.GetComponent<Rigidbody>().isKinematic = false;
        takenObject.GetComponent<BoxCollider>().enabled = true;
        var synchronizer = takenObject.GetComponent<CrateSynchronizationHandler>();
        synchronizer.changable = true;
        takenObject.transform.SetParent(null);
        if (synchronizer.IsAuthor)
        {
            Rigidbody rb = takenObject.GetComponent<Rigidbody>();
            rb.linearVelocity = GetComponent<Rigidbody>().linearVelocity + (transform.forward * 15f + Vector3.up * 5f);
        }
        if (needToSend)
        {
            GameManager.Instance.SendCrateInteracting(
                GetComponent<PlayerSynchronizationHandler>().Id,
                takenObjectIndex,
                false
            );
        }
        takenObject = null;
        takenObjectIndex = -1;
    }
}
