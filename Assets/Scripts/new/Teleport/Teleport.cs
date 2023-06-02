using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform point;
    public UnityEngine.Object plane;
    private bool isTeleported;

    private CharacterController characterController;

    public bool isTouchTeleport()
    {
        if(isTeleported) { return true; }
        return false;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            characterController = collision.gameObject.GetComponent<CharacterController>();
            characterController.enabled = false;
            //DeletePlane();
            collision.transform.position = point.transform.position;
            characterController.enabled = true;
            Debug.Log("Телепорт сработал!" + collision.transform.position);

        }
    }

    private void DeletePlane()
    {
        Destroy(plane);
    }

}
