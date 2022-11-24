using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewMeters : Interactable
{
      public bool openMenu = false;
      Animator anim;

    public Canvas menuUI;
     public override void OnInteract()
    {
        //Debug.Log("Changing UI");
        ChangeUI();
        //play animation
        anim.Play("Armature|Press");
        anim.SetTrigger("press");

    }
    public void ChangeUI()
    {
        menuUI.enabled = !menuUI.enabled;
        openMenu = !openMenu;
        //for this one, we don't need to see the cursor
        /* if (Cursor.lockState == CursorLockMode.Locked)
        {
             Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true; 
        }  
        else
        {
             Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false; 
        } */
    }
    // Start is called before the first frame update
    void Start()
    {
        //disable at start
        menuUI.enabled = false;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
