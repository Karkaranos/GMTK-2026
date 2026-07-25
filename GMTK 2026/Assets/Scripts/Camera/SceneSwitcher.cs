using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SceneSwitcher : MonoBehaviour
{
    [SerializeField] private SceneCamera leftCam;
    [SerializeField] private SceneCamera rightCam;
    [SerializeField, Range(0f, 1f), Tooltip("Controls how far the mouse must be on either side of the " +
        "screen to preview the other scene.")] 
    private float previewWindow;

    private InputAction mousePosAction;
    private InputAction clickAction;

    private bool onRightSide;
    private SceneCamera currentCam;
    private bool isPreviewing;

    public static bool CanSwitchScenes { get; set; }

    private SceneCamera CurrentCam
    {
        get => currentCam;
        set
        {
            if (currentCam != null)
            {
                currentCam.OnCameraDeactivate();
            }

            currentCam = value;

            if (currentCam != null)
            {
                currentCam.OnCameraActivate();
            }
        }
    }

    private void Awake()
    {
        CanSwitchScenes = true;
        CurrentCam = leftCam;
        mousePosAction = InputSystem.actions.FindAction("MousePos");
        mousePosAction.Enable();
        clickAction = InputSystem.actions.FindAction("Click");
        clickAction.Enable();

        mousePosAction.performed += HandleMousePos;
        clickAction.started += HandleClick;
    }

    private void OnDestroy()
    {
        mousePosAction.performed -= HandleMousePos;
        clickAction.started -= HandleClick;
    }

    private void HandleMousePos(InputAction.CallbackContext obj)
    {
        if (CanSwitchScenes && !MenuBehavior.GamePaused)
        {
            Vector2 mousePos = obj.ReadValue<Vector2>();
            float normalizedMouseX = mousePos.x / Screen.width;
            if (onRightSide)
            {
                // Mouse needs to be to the left to preview on the right side.
                if (normalizedMouseX < previewWindow)
                {
                    if (!isPreviewing)
                    {
                        isPreviewing = true;
                        rightCam.Preview();
                    }
                }
                else
                {
                    if (isPreviewing)
                    {
                        isPreviewing = false;
                        rightCam.ClearPreview();
                    }
                }
            }
            else
            {
                if (normalizedMouseX > 1 - previewWindow)
                {
                    if (!isPreviewing)
                    {
                        isPreviewing = true;
                        leftCam.Preview();
                    }
                }
                else
                {
                    if (isPreviewing)
                    {
                        isPreviewing = false;
                        leftCam.ClearPreview();
                    }
                }
            }
        }
        else if (isPreviewing)
        {
            ClearAllPreviews();
        }
    }

    private void HandleClick(InputAction.CallbackContext obj)
    {
        if (isPreviewing && CanSwitchScenes)
        {
            ToggleCamera(!onRightSide);
            
        }
    }

    private void ToggleCamera(bool isRight)
    {
        onRightSide = isRight;
        if (isRight)
        {
            CurrentCam = rightCam;
        }
        else
        {
            CurrentCam = leftCam;
        }
        // Remove any previewing.
        ClearAllPreviews();
    }

    private void ClearAllPreviews()
    {
        leftCam.ClearPreview();
        rightCam.ClearPreview();
        isPreviewing = false;
    }
}
