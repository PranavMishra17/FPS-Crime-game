using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

public class ConvStarter : MonoBehaviour
{
    [SerializeField] private NPCConversation thisconv;

    public FirstPersonController fps;
    public GameObject pressEgo;

    private bool convstarted;
    // Start is called before the first frame update
    void Start()
    {
        fps = FindObjectOfType<FirstPersonController>();
        convstarted = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to the player
        if (other.CompareTag("Player"))
        {

            pressEgo.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Check if the collider belongs to the player
        if (other.CompareTag("Player"))
        {

            if (Input.GetKey(KeyCode.T) && !convstarted)
            {
                if(!convstarted)
                {
                    convstarted = true;
                    ConversationManager.Instance.StartConversation(thisconv);
                    fps.Toggle();
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    pressEgo.SetActive(false);
                    Debug.Log("000 if");
                }

            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the collider belongs to the player
        if (other.CompareTag("Player"))
        {
            pressEgo.SetActive(false);
        }
    }

    public void ExitDialogue()
    {
        fps.Toggle();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        convstarted = false;
        Debug.Log("000 else");
    }
}
