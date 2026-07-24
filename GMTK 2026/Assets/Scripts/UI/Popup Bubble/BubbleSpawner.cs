using NaughtyAttributes;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BubbleSpawner : MonoBehaviour
{
    public GameObject BubblePrefab;
    public PopupBubbleData bubbleData;

    public Canvas targetCanvas;

    public bool randomizeSpawnPosition;

    [ShowIf("randomizeSpawnPosition")]
    public Rect spawnRect;

    private bool singleSpawnPoint => !randomizeSpawnPosition;
    [ShowIf("singleSpawnPoint")]
    public List<Transform> spawnPositions;

    public int maxBubblesActive;
    private List<PopupBubbleController> activeBubbles = new();
    public Vector2 spawnTimeRange;
    private Coroutine spawnTimer;

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(255, 0, 255, 0.5f);
        if (randomizeSpawnPosition) Gizmos.DrawCube((Vector3)((Vector2)transform.position + spawnRect.center), spawnRect.size);
    }

    private void Update()
    {
        if (activeBubbles.Count < maxBubblesActive)
        {
            spawnTimer ??= StartCoroutine(SpawnAfterTime(Random.Range(spawnTimeRange.x, spawnTimeRange.y)));
        }
    }

    private IEnumerator SpawnAfterTime(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        activeBubbles.Add(SpawnPopupBubble(bubbleData, (Vector3)RandomLocalPosition()));
        spawnTimer = null;
    }

    private Vector2 RandomLocalPosition()
    {
        if (randomizeSpawnPosition)
        {
            float randomX = Random.Range(0, spawnRect.width);
            float randomY = Random.Range(0, spawnRect.height);

            return new Vector2(randomX, randomY);
        }
        else
        {
            return spawnPositions[Random.Range(0, spawnPositions.Count)].localPosition;
        }
    }

    public PopupBubbleController SpawnPopupBubble(PopupBubbleData data, Vector3 position)
    {
        GameObject instance = Instantiate(BubblePrefab, transform);
        PopupBubbleController bubbleController = instance.transform.GetComponent<PopupBubbleController>();

        instance.transform.localPosition = RandomLocalPosition();

        bubbleController.bubbleData = data;
        bubbleController.IconImage.sprite = data.iconSprite;
        bubbleController.FadeIconImage.sprite = data.iconSprite;

        bubbleController.bubbleData.onComplete = () => { activeBubbles.Remove(bubbleController); };

        return bubbleController;
    }
}
