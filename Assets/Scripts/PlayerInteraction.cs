using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private LayerMask interactMask;
    private GameObject takenObject;
    private Vector3 objectPosition = new Vector3(0f, -0.5f, 1f);
    private Vector3 objectRotation = new Vector3(0f, 0f, 0f);

    private void Update()
    {
        if (takenObject)
        {
            takenObject.transform.localPosition = objectPosition;
            takenObject.transform.localEulerAngles = objectRotation;
        }
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (takenObject)
            {
                Throw();
            }
            else
            {
                Take();
            }
        }
    }

    private void Take()
    {
        RaycastHit hit;
        if (Physics.CapsuleCast(transform.position - transform.up * 0.5f, transform.position + transform.up * 0.5f, 0.499f, transform.forward, out hit, 1.2f, interactMask))
        {
            takenObject = hit.collider.gameObject;
            takenObject.transform.SetParent(transform);
        }
    }

    private void Throw()
    {
        takenObject.transform.SetParent(null);
        takenObject.GetComponent<Rigidbody>().AddForce(transform.forward * 15f, ForceMode.Impulse);
        takenObject = null;
    }
}
