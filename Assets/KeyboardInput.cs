using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class keyboardinput : MonoBehaviour
{

    public InputActionReference secondaryButtonRight;
    public InputActionReference secondaryButtonLeft;
    public XRRayInteractor RrayInteractor;
    public XRRayInteractor LrayInteractor;
    public string UI_InputName;

    private bool isHovered = false;
    private bool RightHovered = false;
    private bool LeftHovered = false;
    public string LevelName = "";
    //private TextMeshProUGUI textMesh;
    public UnityEvent<string> OnNameChanged = new UnityEvent<string>();

    public TMP_InputField textMesh;

    //public int DefaultName;

    // Start is called before the first frame update
    void Start()
    {
        secondaryButtonRight.action.started += OnSecondaryButton;
        secondaryButtonLeft.action.started += OnSecondaryButton;
    }

    // Update is called once per frame
    void Update()
    {

        // Detect UI Element Hits
        if (RrayInteractor.TryGetCurrentUIRaycastResult(out RaycastResult uiHit))
        {
            //Debug.Log($"Hovering over UI: {uiHit.gameObject.name}");
            if (uiHit.gameObject.name == UI_InputName)
            {
                //Debug.Log("CORRECT UI DETECTED"); //RIGHT HAND HOVER DETECTED
                RightHovered =(true);//activates keyboard group
            }
            else
            {
                //Debug.Log("NO UI DETECTED"); //RIGHT HAND HOVER DETECTED
                RightHovered = (false);//deactivates keyboard group
            }

        }
        if (LrayInteractor.TryGetCurrentUIRaycastResult(out RaycastResult uiHit2))
        {
            //Debug.Log($"Hovering over UI: {uiHit.gameObject.name}");
            if (uiHit2.gameObject.name == UI_InputName)
            {
                //Debug.Log("CORRECT UI DETECTED"); //LEFT HAND HOVER DETECTED
                LeftHovered = (true);//activates keyboard group
            }
            else
            {
                //Debug.Log("NO UI DETECTED"); //RIGHT HAND HOVER DETECTED
                LeftHovered = (false);//deactivates keyboard group
            }
         
        }
        if (RightHovered | LeftHovered)
        {
            isHovered = true;
            //Debug.Log("hovered");
        }
        else
        {
            isHovered = false;
            //Debug.Log("not hovered");
        }
    }


    /*
public void OnHoverEntered(HoverEnterEventArgs hoverEnter)
    {
        isHovered = true;
        Debug.Log("levelName: Hovered");
    }

    public void OnHoverExited(HoverExitEventArgs hoverExit)
    {
        isHovered = false;
        Debug.Log("LevelName: Hovered Exited");
    }
    */

    public void OnSecondaryButton(InputAction.CallbackContext ctx)
    {
        if (isHovered)
        {
            VRKeys.Keyboard keyboard = FindObjectOfType<VRKeys.Keyboard>();
            keyboard.Enable();
            keyboard.SetText(LevelName);
            ControllerModels cm = FindObjectOfType<ControllerModels>();
            cm.EnableControllerModel(true, true);
            cm.EnableControllerModel(true, false);

            void disable_keyboard()
            {
                cm.EnableControllerModel(false, true);
                cm.EnableControllerModel(false, false);
                keyboard.Disable();
                keyboard.OnCancel.RemoveAllListeners();
                keyboard.OnSubmit.RemoveAllListeners();
            }
            ;

            keyboard.OnCancel.AddListener(() => {
                disable_keyboard();
            });

            keyboard.OnSubmit.AddListener((submitText) => {
                LevelName = submitText;
                textMesh.text = LevelName;
                //if (LevelName == "") { textMesh.text = DefaultName.ToString(); }
                OnNameChanged.Invoke(submitText);
                disable_keyboard();
            });
        }
    }
}
