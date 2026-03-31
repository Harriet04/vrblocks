using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AddObject : MonoBehaviour
{
    [SerializeField] private GameObject? blockPrefab; // Assign corresponding block in inspector
    [SerializeField] private Transform spawnParent; // Assign "MoveableEntities" as spawn position in hierarchy if that's how we're moving with it.
    [SerializeField] private Vector3 spawnOffset = new(0, 0.1f, 0); // Offset to avoid overlap
    public ExecutionDirector executionDirector;
    public CodingModeSettings CodingModeSettings;
    public Transform CodingWindow;
    public Transform startBlock;
    private Vector3 scaleValue = new Vector3(0.25f, 0.125f, 0.25f);
    private void Awake()
    {
        if (TryGetComponent<Button>(out var button))
        {
            button.onClick.AddListener(SpawnBlock);
        }
        else
        {
            Debug.LogError($"{gameObject.name}: AddObject script must be attached to a UI Button.");
        }
    }

    private void SpawnBlock()
    {
        if(CodingModeSettings.CodingMode == 0)//if coding is in normal mode
        {
            if (blockPrefab == null)
            {
                Debug.LogError("No blockPrefab assigned to " + gameObject.name);
                return;
            }

            Transform buttonTransform = GetComponent<Transform>();
            GameObject newBlock = Instantiate(
                blockPrefab,
                buttonTransform.position + spawnOffset, // Spawning based on button position
                buttonTransform.rotation,
                spawnParent
            );

            newBlock.name = blockPrefab.name; // Because I use strings for block queue.

            Debug.Log("Spawned: " + newBlock.name + " at " + newBlock.transform.position);
        }else if (CodingModeSettings.CodingMode == 1)// when coding is in simple mode
        {
            if (blockPrefab == null)
            {
                Debug.LogError("No blockPrefab assigned to " + gameObject.name);
                return;
            }


            
            GameObject newBlock = Instantiate(
                blockPrefab,
                CodingModeSettings.heightOffset + (CodingModeSettings.simpleOffset * (CodingModeSettings.spawnCounter%8)), // Spawning based on start block position
                startBlock.rotation,
                spawnParent
            );
            //delete snapping
            Destroy(newBlock.GetComponent<BlockSnapping>());

            CodingModeSettings.spawnCounter++;
            newBlock.GetComponent<Rigidbody>().useGravity = false;  //turn off block gravity
            newBlock.transform.eulerAngles = new Vector3(CodingWindow.eulerAngles.x, CodingWindow.eulerAngles.y, CodingWindow.eulerAngles.z);
            newBlock.transform.LeanScale(scaleValue,0.0f);
            Destroy(newBlock.GetComponent<BlockGrabInteractable>());
            Destroy(newBlock.GetComponent<Rigidbody>());  //turn off physics of all blocks
            
            
            newBlock.name = blockPrefab.name; // Because I use strings for block queue.
            executionDirector.mainBlockList.Add(newBlock);
            
            //update spawn offset in case of block overflow
            if (CodingModeSettings.spawnCounter % 8 == 0)
            {
                Vector3 temp = new Vector3(0.175f,0.0f,-0.225f);
                CodingModeSettings.heightOffset += temp;
            }
        }
    }

    /*public void LinkTest()
    {
        Debug.Log("LINK WORKS");
    }*/
}
