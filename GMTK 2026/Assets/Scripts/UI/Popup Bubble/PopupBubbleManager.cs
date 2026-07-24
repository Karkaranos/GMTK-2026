using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class PopupBubbleManager : MonoBehaviour
{
    public static PopupBubbleManager Instance;
    public GameObject BubblePrefab;

    private void Start()
    {
        if (Instance != null) Destroy(gameObject);
        Instance = this;
    }

    public PopupBubbleController SpawnPopupBubble(PopupBubbleData data, Vector3 position)
    {
        GameObject instance = Instantiate(BubblePrefab, transform);
        PopupBubbleController bubbleController = instance.GetComponent<PopupBubbleController>();


        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, position);
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle( GetComponent<RectTransform>(), screenPoint, null, out localPoint );

        instance.GetComponent<RectTransform>().position = localPoint;

        bubbleController.bubbleData = data;
        bubbleController.IconImage.sprite = data.iconSprite;
        bubbleController.FadeIconImage.sprite = data.iconSprite;

        return bubbleController;
    }
}
