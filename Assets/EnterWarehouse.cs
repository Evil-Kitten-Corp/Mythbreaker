using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterWarehouse : MonoBehaviour
{
    [SerializeField] private Animator MyAnimationController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        { 
            MyAnimationController.SetBool("Enter", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MyAnimationController.SetBool("Enter", false);
        }
    }
}
