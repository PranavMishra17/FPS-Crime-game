using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReserseTrigger : MonoBehaviour
{
    private bool isTriggered = false;
    public float backwardsDistance = 5f;
    public float rotationDegrees = 180f;
    public float liftHeight = 2f;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !isTriggered)
        {
            isTriggered = true;
            Vector3 backwardsVector = -other.gameObject.transform.forward * backwardsDistance;
            other.gameObject.transform.position += backwardsVector;
            other.gameObject.transform.Rotate(0f, rotationDegrees, 0f);
            Debug.Log("Trigger Enter");

            // Lift the player up
            other.gameObject.transform.position += new Vector3(0f, liftHeight, 0f);


            isTriggered = false;
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player" && !isTriggered)
        {
            Debug.Log("Collision Enter");
            isTriggered = true;
            Vector3 backwardsVector = -other.gameObject.transform.forward * backwardsDistance;
            other.gameObject.transform.position += backwardsVector;
            other.gameObject.transform.Rotate(0f, rotationDegrees, 0f);

            // Lift the player up
            other.gameObject.transform.position += new Vector3(0f, liftHeight, 0f);


            isTriggered = false;
        }
    }

}
