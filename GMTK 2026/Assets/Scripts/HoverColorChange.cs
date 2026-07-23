using UnityEngine;

public class HoverColorChange : MonoBehaviour
{
    [SerializeField] private Color hoverColor;

    private SpriteRenderer rend;
    private Color baseColor;

    private void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
        baseColor = rend.color;
    }

    private void OnMouseEnter()
    {
        rend.color = hoverColor;
    }
    private void OnMouseExit()
    {
        rend.color = baseColor;
    }
}
