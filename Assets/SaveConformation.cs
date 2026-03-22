using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveConformation : MonoBehaviour
{
    public TextMeshProUGUI ConformationText;
    public Button ConformationButton;

    public GameObject SaveAsset;
    public GameObject SandboxSelectorMenu;

    public float AnimSpeed = 0.3f;

    void Start()
    {
        ConformationText.text = "Successfully Saved";
        ConformationButton.onClick.AddListener(Confirm);

    }
    public void Confirm()
    {
        SandboxSelectorMenu.LeanScale(Vector3.one, AnimSpeed).setEaseInOutCubic();
        SaveAsset.SetActive(false);
        DisableMenu();
    }


    private void SetRelativeScale(Vector3 Scale)
    {
        //Scale the game object this script is applied to
        gameObject.LeanScale(Scale, AnimSpeed).setEaseInOutCubic();
    }

    public void DisableMenu()
    {
        SetRelativeScale(Vector3.zero);
    }
}
