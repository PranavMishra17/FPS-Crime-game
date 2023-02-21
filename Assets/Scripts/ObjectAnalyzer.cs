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
            StartCoroutine(MoveObjectToPosition(pickedObject, objectOriginalPosition, objectOriginalRotation, 0.5f));
            player.gameObject.GetComponent<FirstPersonController>().Toggle();
            pickedObject = null;

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

    IEnumerator MoveObjectToPosition(GameObject obj, Vector3 targetPos, Quaternion targetRot, float time)
    {
        Vector3 startPosition = obj.transform.position;
        Quaternion startRotation = obj.transform.rotation;
        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            obj.transform.position = Vector3.Lerp(startPosition, targetPos, (elapsedTime / time));
            obj.transform.rotation = Quaternion.Slerp(startRotation, targetRot, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        obj.transform.position = targetPos;
        obj.transform.rotation = targetRot;
    }

    // player.gameObject.GetComponent<FirstPersonController>().Toggle();

}

