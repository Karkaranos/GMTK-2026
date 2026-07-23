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

    public void SpawnPopupBubble(PopupBubbleData data, Vector3 position)
    {
        GameObject instance = Instantiate(BubblePrefab, transform);
        PopupBubbleController bubbleController = instance.GetComponent<PopupBubbleController>();

        instance.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(position);
        bubbleController.bubbleData = data;
        bubbleController.IconImage.sprite = data.iconSprite;
        bubbleController.FadeIconImage.sprite = data.iconSprite;
    }
}
