using System;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private LayerMask interactMask;
    private GameObject takenObject;
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

    private void Update()
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
        Action<Collider> take = (collider) =>
        {
            takenObject = collider.gameObject;
            takenObject.transform.SetParent(transform);
        };

        RaycastHit hit;
        Collider[] colliders = Physics.OverlapCapsule(transform.position - transform.up * 0.5f, transform.position + transform.up * 0.5f, 0.5f, interactMask);
        if (colliders.Length != 0)
        {
            take(colliders[0]);
        }
        else
        {
            if (Physics.CapsuleCast(transform.position - transform.up * 0.5f, transform.position + transform.up * 0.5f, 0.5f, transform.forward, out hit, 1f, interactMask))
            {
                take(hit.collider);
            }
        }
    }

    private void Throw()
    {
        takenObject.transform.SetParent(null);
        takenObject.GetComponent<Rigidbody>().AddForce(transform.forward * 15f, ForceMode.Impulse);
        takenObject = null;
    }
}
