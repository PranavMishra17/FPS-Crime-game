using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TylerCode.SoundSystem;

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
                gameObject.GetComponentInParent<S4SoundSource>().PlaySound("DoorOpen");
            }
            else
            {
                animator.SetBool("open", false);
                gameObject.GetComponentInParent<S4SoundSource>().PlaySound("DoorClose");
            }
            

        }
    }
}
