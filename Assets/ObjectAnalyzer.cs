using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAnalyzer : MonoBehaviour
{  
    private GameObject pickedObject;
    public GameObject player;
    private Vector3 objectOriginalPosition;
    private Quaternion objectOriginalRotation;
    private Vector3 offset;

    [SerializeField]
    public float pickDistance = 3f;

    private void Start()
    {
        player = GameObject.Find("Player");
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, pickDistance))
            {
                if (hit.collider.gameObject.tag == "Pickable")
                {
                    pickedObject = hit.collider.gameObject;
                    player.gameObject.GetComponent<FirstPersonController>().Toggle();

                    offset = pickedObject.transform.position - ray.GetPoint(hit.distance);
                    objectOriginalPosition = pickedObject.transform.position;
                    objectOriginalRotation = pickedObject.transform.rotation;
                }
            }
        }

        if (Input.GetMouseButton(0) && pickedObject != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 newPosition = ray.GetPoint(pickDistance) + offset;
            pickedObject.transform.position = newPosition;
        }

        if (Input.GetMouseButtonUp(0) && pickedObject != null)
        {
            pickedObject.transform.position = objectOriginalPosition;
            pickedObject.transform.rotation = objectOriginalRotation;
            pickedObject = null;

            player.gameObject.GetComponent<FirstPersonController>().Toggle();

        }

        if (pickedObject != null)
        {
            float rotationSpeed = 10.0f;
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

            pickedObject.transform.Rotate(Vector3.up, -mouseX, Space.World);
            pickedObject.transform.Rotate(Vector3.right, mouseY, Space.World);
        }
    }
    
    // player.gameObject.GetComponent<FirstPersonController>().Toggle();
 
}

