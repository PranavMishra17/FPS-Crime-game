using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class ObjectAnalyzer : MonoBehaviour
{

    private GameObject pickedObject;
    public GameObject player;
    public GameObject inventoryCanvas;
    private Vector3 objectOriginalPosition;
    private Quaternion objectOriginalRotation;
    private Vector3 offset;

    [SerializeField]
    public float pickDistance = 5f;
    //private bool viewstate = false;
    private bool isObjectPicked = false;

    private bool takeSS = false;
    private bool isDetailAdded = false;
    private bool isScreenshotAdded = false;
    private bool cluemenuactive = false;
    public string[] clues = new string[] { "Sword is rather rusty", "Sword doesn't belong here i feel" };
    //public string Clue1 = "None";
    //public string Clue2 = "None";
    //public string Clue3 = "None";
    public string pickedClue = "None";
    public Button clueButtonPrefab1;
    public Button clueButtonPrefab2;
    public Button clueButtonPrefab3;
    public GameObject cluePrompt;
    public GameObject clueMenu;
    public GameObject screenshotIcon;
    //private Sprite screenshotSprite;
    public GameObject levelInventoryObject;
    public InventoryManager invM;
    public InventoryItem invI;
    public ReticleSelection retS;
    public LevelInventory levelInventory;

    public int resWidth = 2550;
    public int resHeight = 3300;
    

    public Image screenshotImage;
   // public XEntity.InventoryItemSystem.InstantHarvest intH;

    private void Start()
    {
        player = GameObject.Find("Player");
        retS = player.gameObject.GetComponent<ReticleSelection>();

        inventoryCanvas = GameObject.Find("InventoryCanvas");
        invM = inventoryCanvas.gameObject.GetComponent<InventoryManager>();
        invI = this.gameObject.GetComponent<InventoryItem>();

        levelInventoryObject = GameObject.Find("LvlInventory");
        levelInventory = levelInventoryObject.GetComponent<LevelInventory>();

    }
    void Update()
    {
       // ToggleFocusState();
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 10f))
            {
                if (hit.collider.gameObject.tag == "Pickable")
                {
                    if (!isObjectPicked) // if the object is not currently picked up
                    {
                        isObjectPicked = true;
                        retS.objpicked = true;
                        pickedObject = hit.collider.gameObject;
                        float distance = hit.distance;
                        pickDistance = distance;
                        player.gameObject.GetComponent<FirstPersonController>().Toggle();

                        offset = pickedObject.transform.position - ray.GetPoint(hit.distance);
                        objectOriginalPosition = pickedObject.transform.position;
                        objectOriginalRotation = pickedObject.transform.rotation;
                        clueMenu.SetActive(true);
                        //focusStateActive = true;

                    }
                }
            }
        }

        if (Input.GetKey(KeyCode.E) && isObjectPicked && pickedObject != null && !isDetailAdded)
        {
            cluemenuactive = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            cluePrompt.SetActive(false);
            StartCoroutine(showCluePrompt(clues));
        }
        else if (isDetailAdded)
        {
            cluePrompt.GetComponent<TextMeshProUGUI>().text = "Clue added in files";
            cluePrompt.SetActive(true);
        }

       /* if (Input.GetKey(KeyCode.Space) && isObjectPicked && pickedObject != null && !isDetailAdded && !isScreenshotAdded)
        {
            GetScreenshot();
        }*/

        if (Input.GetKey(KeyCode.Q) && isObjectPicked && pickedObject != null)
        {
            StartCoroutine(MoveObjectToPosition(pickedObject, objectOriginalPosition, objectOriginalRotation, 0.5f));
            player.gameObject.GetComponent<FirstPersonController>().Toggle();
            pickedObject = null;
            isObjectPicked = false;
            retS.objpicked = false;
            cluePrompt.GetComponent<TextMeshProUGUI>().text = "";
            cluePrompt.SetActive(false);
            clueMenu.SetActive(false);
            clueButtonPrefab1.gameObject.SetActive(false);
            clueButtonPrefab2.gameObject.SetActive(false);
            clueButtonPrefab3.gameObject.SetActive(false);
            cluemenuactive = false;
            Cursor.visible = false;
            //focusStateActive = false ;
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (isObjectPicked && !cluemenuactive)
        {
            Ray ray1 = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 newPosition = ray1.GetPoint(pickDistance) + offset;
            pickedObject.transform.position = newPosition;

            float rotationSpeed = 10.0f;
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

            pickedObject.transform.Rotate(Vector3.up, -mouseX, Space.World);
            pickedObject.transform.Rotate(Vector3.right, mouseY, Space.World);
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

    private void LateUpdate()
    {
        if (Input.GetKey(KeyCode.Space) && isObjectPicked && pickedObject != null && isDetailAdded && !isScreenshotAdded)
        {
            GetScreenshot();
        }
    }
    /* 
     private void ToggleFocusState()
     {
         if (focusStateActive)
         {
             // Set the focus distance and aperture of the DepthOfField effect based on the position of the objectToFocusOn
             depthOfField.focusDistance.value = Vector3.Distance(Camera.main.transform.position, objectToFocusOn.transform.position);
             depthOfField.aperture.value = focusAperture;
         }
         else
         {
             // Reset the focus distance and aperture to default values
             depthOfField.focusDistance.value = 10f;
             depthOfField.aperture.value = normalAperture;
         }
     }
    */
    private IEnumerator showCluePrompt(string[] clues)
    {
        clueButtonPrefab1.gameObject.SetActive(true);
        clueButtonPrefab2.gameObject.SetActive(true);
        clueButtonPrefab3.gameObject.SetActive(true);

        clueButtonPrefab1.GetComponentInChildren<TextMeshProUGUI>().text = clues[0];
        clueButtonPrefab2.GetComponentInChildren<TextMeshProUGUI>().text = clues[1];
        clueButtonPrefab3.GetComponentInChildren<TextMeshProUGUI>().text = clues[2];

        clueButtonPrefab1.onClick.AddListener(delegate { addClue(clues[0]); });
        clueButtonPrefab2.onClick.AddListener(delegate { addClue(clues[1]); });
        clueButtonPrefab3.onClick.AddListener(delegate { addClue(clues[2]); });
        
        // Wait until the player chooses a clue
        yield return new WaitUntil(() => pickedClue != "");
            
    }

    void addClue(string clue)
    {
        pickedClue = clue;
        isDetailAdded = true;
        //intr.InitInteraction();
        clueButtonPrefab1.gameObject.SetActive(false);
        clueButtonPrefab2.gameObject.SetActive(false);
        clueButtonPrefab3.gameObject.SetActive(false);
        //GetScreenshot();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        cluemenuactive = false;

        cluePrompt.GetComponent<TextMeshProUGUI>().text = "Click space to capture. Position object accordingly.";
        cluePrompt.SetActive(true);

        screenshotIcon.SetActive(true);
    }

    private void GetScreenshot()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            // Capture the screenshot and update the UI image with the new sprite
            StartCoroutine(captureScreenshot());

            //screenshotImage.sprite = screenshotSprite;
        }
        isScreenshotAdded = true;
        isDetailAdded = true;
        //AddItemToInventory();
        //intH.AttemptHarvest(pickedObject);
    }
    IEnumerator captureScreenshot()
    {
        yield return new WaitForEndOfFrame();

        string path = "Assets/Screenshots/" + "_" + Screen.width + "X" + Screen.height + ".png";

        Texture2D screenImage = new Texture2D(Screen.width, Screen.height);
        //Get Image from screen
        screenImage.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenImage.Apply();

        // Calculate the cropping parameters
        int width = screenImage.width;
        int height = screenImage.height;
        int size = Mathf.Min(width, height);
        int x = (width - size) / 2;
        int y = (height - size) / 2;
        int offsetX = (width - size) % 2;
        int offsetY = (height - size) % 2;

        // Crop the texture from the central line
        Color[] pixels = screenImage.GetPixels(x + offsetX / 2, y + offsetY / 2, size, size);
        Texture2D croppedTexture = new Texture2D(size, size, TextureFormat.RGB24, false);
        croppedTexture.SetPixels(pixels);
        croppedTexture.Apply();


        //Convert to png
        //byte[] imageBytes = croppedTexture.EncodeToPNG();

        //Save image to file
        //System.IO.File.WriteAllBytes(path, imageBytes);

        // Refresh the Asset Database
        //AssetDatabase.Refresh();

        // Load the image from file
       // Texture2D texture = new Texture2D(Screen.width, Screen.height);
        //byte[] fileData = System.IO.File.ReadAllBytes(path);
       // texture.LoadImage(fileData);
        
        // Create a sprite from the texture
        //screenshotSprite = Sprite.Create(croppedTexture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
        //screenshotSprite = Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
        //screenshotImage.sprite = screenshotSprite;

        // Create a new inventory item and set its sprite and info properties
        InventoryItem newItem = new InventoryItem(Sprite.Create(croppedTexture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f)), pickedClue);
        newItem.sprite = Sprite.Create(croppedTexture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
        newItem.itemInfo = pickedClue;
        //invM.AddingtoInv();

        // Find the appropriate level inventory game object and get its LevelInventory component
        //GameObject levelInventoryObject = GameObject.Find("LevelInventory");
        //LevelInventory levelInventory = levelInventoryObject.GetComponent<LevelInventory>();

        // Add the new item to the level inventory
        levelInventory.AddItem(newItem);
        Debug.Log("Item added to inventory: " + newItem.itemInfo);
        //invM.ToggleInventoryUI();
    }

}


