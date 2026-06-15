using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    private FacilDoor door;

    void Start()
    {
        door = GetComponentInParent<FacilDoor>();
    }

    void OnTriggerEnter(Collider c) {

        door.openDoor(c);

    }

    void OnTriggerExit(Collider c) {
        door.closeDoor(c);
    }
}
