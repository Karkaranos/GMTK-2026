using System.Collections.Generic;
using UnityEngine;

public class PopupIndicatorHandler : MonoBehaviour
{
    public static PopupIndicatorHandler Instance;

    [SerializeField] private GameObject prefab;
    [SerializeField] private Camera camera;
    [SerializeField] private Canvas canvas;
    [SerializeField] private float distanceFromScreenEdge;
    private Dictionary<Transform, RectTransform> popups = new();

    private void Start()
    {
        Instance = this;
    }

    // I'm aware that this is not particularly efficient
    // But that shouldn't really matter
    void Update()
    {
        Bounds bounds = GetBounds();
        foreach (var popup in popups)
        {
            if (bounds.Contains(popup.Key.position))
            {
                popup.Value.gameObject.SetActive(false);
            }
            else
            {
                popup.Value.gameObject.SetActive(true);

                //Debug.DrawLine(popup.Key.position, bounds.ClosestPoint(popup.Key.position));
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), 
                    camera.WorldToScreenPoint(bounds.ClosestPoint(popup.Key.position)), camera, out var pos);
                popup.Value.anchoredPosition = pos;
            }
        }
    }

    public void AddPopup(Transform t)
    {
        popups.Add(t, Instantiate(prefab, canvas.transform).GetComponent<RectTransform>());
    }

    public void RemovePopup(Transform t)
    {
        Destroy(popups[t].gameObject);
        popups.Remove(t);
    }

    private Bounds GetBounds()
    {
        float cameraHeight = camera.orthographicSize * 2;
        return new Bounds(camera.transform.position, 
            new Vector3(cameraHeight * ((float)Screen.width / Screen.height) - (distanceFromScreenEdge * camera.orthographicSize),
            cameraHeight - (distanceFromScreenEdge * camera.orthographicSize), 100));
    }
}
