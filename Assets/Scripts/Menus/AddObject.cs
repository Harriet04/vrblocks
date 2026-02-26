using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AddObject : MonoBehaviour
{
    [SerializeField] private GameObject? blockPrefab; // Assign corresponding block in inspector
    [SerializeField] private Transform spawnParent; // Assign "MoveableEntities" as spawn position in hierarchy if that's how we're moving with it.
    [SerializeField] private Vector3 spawnOffset = new(0, 0.5f, 0); // Offset to avoid overlap
    public CodingModeSettings CodingModeSettings;
    public Transform CodingWindow;

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
                CodingWindow.position + spawnOffset, // Spawning based on button position
                CodingWindow.rotation,
                spawnParent
            );
            
            newBlock.GetComponent<Rigidbody>().useGravity = false;  //turn off block gravity
            newBlock.name = blockPrefab.name; // Because I use strings for block queue.
        }
    }

    /*public void LinkTest()
    {
        Debug.Log("LINK WORKS");
    }*/
}
