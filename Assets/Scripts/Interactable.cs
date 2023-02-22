using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Animator animator;

    void Start()
    {
       // animator = GetComponent<Animator>();
    }

    void OnMouseDown()
    {
        if (gameObject.CompareTag("Interact"))
        {
            if (animator.GetBool("open") == false)
            {
                animator.SetBool("open", true);
            }
            else
            {
                animator.SetBool("open", false);
            }
            

        }
    }
}
