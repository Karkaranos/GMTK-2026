using System.Collections;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField]
    private float SCROLL_WIDTH = 8f;

    [SerializeField]
    private float scrollSpeed;
    public void FixedUpdate()
    {
        Vector3 pos = transform.position;
        pos.x -= scrollSpeed * Time.deltaTime;
        if (transform.position.x < -SCROLL_WIDTH)
        {
            Offscreen(ref pos);
        }
        transform.position = pos;
    }
    public virtual void Offscreen(ref Vector3 pos)
    {
        pos.x += (2 * SCROLL_WIDTH);

    }
}
