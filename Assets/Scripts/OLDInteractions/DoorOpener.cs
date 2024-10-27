using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("DoorOpener start");
        Animator animator = GetComponent<Animator>();
        animator.SetBool("isOpen", true);
    }
}
