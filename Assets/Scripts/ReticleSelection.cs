using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReticleSelection : MonoBehaviour
{

    [SerializeField] public Image reticleImage;
    [SerializeField] public Sprite defaultReticle, pickableReticle, interactReticle;


    // Start is called before the first frame update
    void Start()
    {
        //reticleImage = GetComponentInChildren<Image>();
    }

    // Update is called once per frame
    void Update()
    {

        #region Reticle selection

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 20f))
        {
            string hitTag = hit.collider.gameObject.tag;
            Debug.Log(hitTag);
            if (hitTag == "Pickable")
            {
                reticleImage.sprite = pickableReticle;
            }
            else if (hitTag == "Interact")
            {
                reticleImage.sprite = interactReticle;
            }
            else 
            {
                reticleImage.sprite = defaultReticle;
            }
        }
        else
        {
            reticleImage.sprite = defaultReticle;
        }

        #endregion
    }
}
