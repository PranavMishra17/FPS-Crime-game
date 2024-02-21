using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerPolice : MonoBehaviour
{
    public Animator npcAnimator;

    // The name of the animation parameter that controls the state
    public string interactionParameterName = "IsInteracting";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Setdancetrigger()
    {
        npcAnimator.SetTrigger("dancenow"); 
    }
}
