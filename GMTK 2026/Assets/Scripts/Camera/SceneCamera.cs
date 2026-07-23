using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Controls the main virtual cameras for each of the two scenes.
/// </summary>
public class SceneCamera : MonoBehaviour
{
    [SerializeField] private CinemachineCamera cam;
    [SerializeField] private float previewOffset;
    [SerializeField] private CoroutineTimeline timeline;
    [SerializeField] private UnityEvent<bool> OnCameraToggleEvent;

    private Vector3 defaultPos;

    private void Awake()
    {
        defaultPos = transform.position;
        timeline.OnUpdateEvent += SetCamPosition;
    }
    private void OnDestroy()
    {
        timeline.OnUpdateEvent -= SetCamPosition;
    }

    public void OnCameraActivate()
    {
        cam.Prioritize();
        OnCameraToggleEvent?.Invoke(true);
    }

    public void OnCameraDeactivate()
    {
        OnCameraToggleEvent?.Invoke(false);
    }

    private void SetCamPosition(float progress)
    {
        transform.position = Vector3.Lerp(defaultPos, defaultPos + Vector3.right * previewOffset, progress);
    }

    public void Preview()
    {
        timeline.Play();
    }

    public void ClearPreview()
    {
        timeline.Reverse();
    }
}
