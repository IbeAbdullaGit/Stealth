using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Reciever

public class Door : Observer
{
    public bool isOpen = false;
    public Vector3 openPosition;
    public Vector3 closedPosition;

    public override void OnNotify()
    {
        OpenDoor();
    }
    public void ToggleDoor()
    {
        if (isOpen == false)
        {
            transform.position = openPosition;
            isOpen = true;
        }
        else if (isOpen == true)
        {
            transform.position = closedPosition;
            isOpen = false;
        }
    }

    public void OpenDoor()
    {
        if (isOpen == false)
        {
            transform.position = openPosition;
            isOpen = true;
        }
    }

    public void CloseDoor()
    {
        if (isOpen == true)
        {
            transform.position = closedPosition;
            isOpen = false;
        }
    }

}
