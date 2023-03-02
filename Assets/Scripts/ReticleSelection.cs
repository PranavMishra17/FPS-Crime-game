using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReticleSelection : MonoBehaviour
{

    [SerializeField] public Image reticleImage;
    [SerializeField] public Sprite defaultReticle, pickableReticle, interactReticle;

    public InventoryManager invM;
    public bool objpicked = false;


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
            //Debug.Log(hitTag);
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

        if (Input.GetKeyDown(KeyCode.I) && !objpicked)
        {
            invM.ToggleInventoryUI();
            
        }

        if (Input.GetKeyDown(KeyCode.P) && !objpicked)
        {
            //invM.SaveInventory();
            DateTime currentDateTime = DateTime.Now;
            SaveLoadManager.SaveGame(currentDateTime.ToString());

        }

        if (Input.GetKeyDown(KeyCode.L) && !objpicked)
        {
            //invM.LoadInventory();
            SaveLoadManager.LoadGame();
        }

    }
}
