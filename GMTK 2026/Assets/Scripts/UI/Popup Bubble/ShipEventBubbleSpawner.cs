using NaughtyAttributes;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using NUnit.Framework;
using System.Linq;
using JetBrains.Annotations;

public class ShipEventBubbleSpawner : MonoBehaviour
{
    public GameObject BubblePrefab;
    public PopupBubbleData bubbleData;

    public Canvas targetCanvas;

    public bool randomizeSpawnPosition;

    [ShowIf("randomizeSpawnPosition")]
    public Rect spawnRect;

    [HideIf("randomizeSpawnPosition"), InfoBox("Number of spawn positions should be >= maxBubblesActive")]
    public List<Transform> spawnPositions;
    private bool[] positionsInUse;

    public int maxBubblesActive;
    private List<PopupBubbleController> activeBubbles = new();
    public Vector2 spawnTimeRange;
    private Coroutine spawnTimer;

    public SectionBuilder sectionToSlow;
    public float fillRateDecrementPerBubble;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(255, 0, 255, 0.5f);
        if (randomizeSpawnPosition) Gizmos.DrawCube((Vector3)((Vector2)transform.position + spawnRect.center), spawnRect.size);
    }

    private void Start()
    {
        positionsInUse = new bool[spawnPositions.Count];
    }

    private void Update()
    {
        if (activeBubbles.Count < maxBubblesActive)
        {
            spawnTimer ??= StartCoroutine(SpawnAfterTime(Random.Range(spawnTimeRange.x, spawnTimeRange.y)));
        }
    }

    public IEnumerator SpawnAfterTime(float seconds)
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

            return new Vector2(randomX + spawnRect.x, randomY + spawnRect.y);
        }
        else
        {
            return spawnPositions[Random.Range(0, spawnPositions.Count)].localPosition;
        }
    }

    private int NewUnusedPositionIndex()
    {
        //grabs all positions that arent marked as in use by positionsInUse[]
        // and returns the index of a random one

        var indexedPairs = spawnPositions.Select((item, index) => (item, index));

        var availablePositions = indexedPairs.Where(pos => !positionsInUse[pos.index]).ToList();

        Debug.Log(availablePositions.Count());
        if (availablePositions.Count() > 0)
        {
            return availablePositions[Random.Range(0, availablePositions.Count())].index;
        }
        else { return -1; }
    }


    public PopupBubbleController SpawnPopupBubble(PopupBubbleData data, Vector3 position)
    {
        GameObject instance = Instantiate(BubblePrefab, targetCanvas.transform);
        PopupBubbleController bubbleController = instance.transform.GetComponent<PopupBubbleController>();

        int posIndex = -1;

        if(randomizeSpawnPosition)
        {
            instance.transform.position = transform.TransformPoint(RandomLocalPosition());
        }
        else
        {
            posIndex = NewUnusedPositionIndex();

            if (posIndex >= 0)
            {
                instance.transform.position = spawnPositions[posIndex].position;
                positionsInUse[posIndex] = true;
            }
            else
            {
                Debug.Log("Spawned bubble has no available unused positions");
            }
        }

        bubbleController.bubbleData = data;
        bubbleController.IconImage.sprite = data.iconSprite;
        bubbleController.FadeIconImage.sprite = data.iconSprite;

        bubbleController.bubbleData.onComplete = () => { BubblePopped(bubbleController); if (!randomizeSpawnPosition) positionsInUse[posIndex] = false; };

        sectionToSlow.buildBar.FillRate -= fillRateDecrementPerBubble;

        return bubbleController;
    }

    private void BubblePopped(PopupBubbleController bubbleController)
    {
        sectionToSlow.buildBar.FillRate += fillRateDecrementPerBubble;
        activeBubbles.Remove(bubbleController);
    }
}
