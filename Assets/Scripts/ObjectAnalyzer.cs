using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class ObjectAnalyzer : MonoBehaviour
{

    public GameObject pickedObject;
    public GameObject player;
    public GameObject inventoryCanvas;
    private Vector3 objectOriginalPosition;
    private Quaternion objectOriginalRotation;
    private Vector3 offset;

    [SerializeField]
    public float pickDistance = 5f;
    //private bool viewstate = false;
    public bool isObjectPicked = false;

    //private bool takeSS = false;
    public bool isDetailAdded = false;
    public bool isScreenshotAdded = false;
    public bool cluemenuactive = false;
    public string[] clues = new string[] { "Sword is rather rusty", "Sword doesn't belong here i feel" };
    //public string Clue1 = "None";
    //public string Clue2 = "None";
    //public string Clue3 = "None";
    public string pickedClue = null;
    public GameObject buttonHolder;
    public Button clueButtonPrefab1;
    public Button clueButtonPrefab2;
    public Button clueButtonPrefab3;
    Button clueButton1;
    Button clueButton2;
    Button clueButton3;
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
    bool mycou = false;
    int i = 1;
    public Image screenshotImage;
    public string screenshotPath;
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
               //     if (hit.collider.gameObject.tag == "Pickable" && hit.collider.gameObject == gameObject)
                {
                    if (!isObjectPicked) // if the object is not currently picked up
                    {
                        this.isObjectPicked = true;
                        retS.objpicked = true;
                        pickedObject = hit.collider.gameObject;
                        clues = pickedObject.GetComponent<InventoryItem>().clues;
                        float distance = hit.distance;
                        this.pickDistance = distance;
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

        if (Input.GetKeyDown(KeyCode.E) && isObjectPicked && pickedObject != null && !pickedObject.GetComponent<InventoryItem>().addedtoInv)
          //  if (Input.GetKeyDown(KeyCode.E) && isObjectPicked && pickedObject != null && !isDetailAdded)
        {
            Debug.Log("first E called");
            cluemenuactive = true;
            buttonHolder.SetActive(true);

           // Cursor.lockState = CursorLockMode.None;
            //Cursor.visible = true;

            cluePrompt.GetComponent<TextMeshProUGUI>().text = " Press the number to pick the clue ";
            cluePrompt.SetActive(false);
            pickedClue = null;
            clues = pickedObject.GetComponent<InventoryItem>().clues;
            StartCoroutine(showCluePrompt(clues));
            //StartCoroutine(showCluePrompt(clues));
        }
        else if (Input.GetKeyDown(KeyCode.E) && pickedObject.GetComponent<InventoryItem>().addedtoInv && isObjectPicked)
        {
            Debug.Log("second E called");
            cluePrompt.GetComponent<TextMeshProUGUI>().text = "Clue added in files";
            cluePrompt.SetActive(true);
        }

        if ((Input.GetKeyDown(KeyCode.Q) && isObjectPicked && pickedObject != null) || (Input.GetKey(KeyCode.Space) && isObjectPicked && pickedObject != null && pickedObject.GetComponent<InventoryItem>().addedtoInv && pickedObject.GetComponent<InventoryItem>().ssAdded))
        {
            Debug.Log("first Q called");
            cluePrompt.GetComponent<TextMeshProUGUI>().text = "  ";
            cluePrompt.SetActive(false);

            StartCoroutine(MoveObjectToPosition(pickedObject, objectOriginalPosition, objectOriginalRotation, 0.5f));
            player.gameObject.GetComponent<FirstPersonController>().Toggle();
            pickedObject = null;
            isObjectPicked = false;
            retS.objpicked = false;
            
            
            clueMenu.SetActive(false);
            //buttonHolder.SetActive(false);

            cluemenuactive = false;

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            if(clueButton1 != null)
            {
                Destroy(clueButton1.gameObject);
                Destroy(clueButton2.gameObject);
                Destroy(clueButton3.gameObject);
            }
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
            pickedObject = null;
        }

        if (mycou)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) { addClue(clues[0]); }
            if (Input.GetKeyDown(KeyCode.Alpha2)) { addClue(clues[1]); }
            if (Input.GetKeyDown(KeyCode.Alpha3)) { addClue(clues[2]); }
        }
    }

    private void LateUpdate()
    {
        if (Input.GetKey(KeyCode.Space) && isObjectPicked && pickedObject != null && pickedObject.GetComponent<InventoryItem>().addedtoInv && !pickedObject.GetComponent<InventoryItem>().ssAdded)
        {
            GetScreenshot();


        }
    }
    private IEnumerator showCluePrompt(string[] clues)
    {
        mycou = true;
        Debug.Log("show clue prompt called");

        buttonHolder.SetActive(true);

        // Remove listeners from any existing buttons
        clueButtonPrefab1.onClick.RemoveAllListeners();
        clueButtonPrefab2.onClick.RemoveAllListeners();
        clueButtonPrefab3.onClick.RemoveAllListeners();

        // Instantiate the three clue buttons
        clueButton1 = Instantiate(clueButtonPrefab1, buttonHolder.transform);
        clueButton2 = Instantiate(clueButtonPrefab2, buttonHolder.transform);
        clueButton3 = Instantiate(clueButtonPrefab3, buttonHolder.transform);

        clueButtonPrefab1.onClick.RemoveAllListeners();
        clueButtonPrefab2.onClick.RemoveAllListeners();
        clueButtonPrefab3.onClick.RemoveAllListeners();

        // Set the text and listeners for each button
        clueButton1.GetComponentInChildren<TextMeshProUGUI>().text = clues[0];
        clueButton1.onClick.AddListener(delegate { addClue(clues[0]); });
        if (Input.GetKeyDown(KeyCode.Alpha1)){ addClue(clues[0]); }

        clueButton2.GetComponentInChildren<TextMeshProUGUI>().text = clues[1];
        clueButton2.onClick.AddListener(delegate { addClue(clues[1]); });
        if (Input.GetKeyDown(KeyCode.Alpha2)) { addClue(clues[1]); }

        clueButton3.GetComponentInChildren<TextMeshProUGUI>().text = clues[2];
        clueButton3.onClick.AddListener(delegate { addClue(clues[2]); });
        if (Input.GetKeyDown(KeyCode.Alpha3)) { addClue(clues[2]); }

       // Wait until the player chooses a clue
        yield return new WaitUntil(() => pickedClue != null);
        mycou = false;

        // Destroy the clue buttons
        Destroy(clueButton1.gameObject);
        Destroy(clueButton2.gameObject);
        Destroy(clueButton3.gameObject);

    }

    void addClue(string clue)
    {
        pickedClue = clue;
        //this.isDetailAdded = true;
        pickedObject.GetComponent<InventoryItem>().addedtoInv = true;

        //buttonHolder.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        cluemenuactive = false;
        Debug.Log("Add clue called");
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

        pickedObject.GetComponent<InventoryItem>().ssAdded = true;
        //this.isScreenshotAdded = true;
        //this.isDetailAdded = true;
        pickedObject.GetComponent<InventoryItem>().addedtoInv = true;
        //AddItemToInventory();
        //intH.AttemptHarvest(pickedObject);
    }
    IEnumerator captureScreenshot()
    {
        yield return new WaitForEndOfFrame();
        i++;
        string path = "Assets/Screenshots/" + "_"  + i + pickedObject.GetComponent<InventoryItem>().itemInfo + ".png";

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
        byte[] imageBytes = croppedTexture.EncodeToPNG();

        //Save image to file
        System.IO.File.WriteAllBytes(path, imageBytes);
        screenshotPath = path;

        // Refresh the Asset Database
        AssetDatabase.Refresh();

        // Load the image from file
       // Texture2D texture = new Texture2D(Screen.width, Screen.height);
        //byte[] fileData = System.IO.File.ReadAllBytes(path);
       // texture.LoadImage(fileData);
        
        // Create a sprite from the texture
        //screenshotSprite = Sprite.Create(croppedTexture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
        //screenshotSprite = Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
        //screenshotImage.sprite = screenshotSprite;

        // Create a new inventory item and set its sprite and info properties
        InventoryItem newItem = new InventoryItem(Sprite.Create(croppedTexture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f)), pickedClue, path);
        newItem.sprite = Sprite.Create(croppedTexture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
        newItem.itemInfo = pickedClue;
        newItem.spritePath = path;
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


