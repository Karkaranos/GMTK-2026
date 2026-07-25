using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ControlRoomEventBubbleSpawner : MonoBehaviour
{
    public GameObject bubblePrefab;

    [Header("Events")]
    public List<PopupBubbleData> singlePenguinEvents;
    public List<PopupBubbleData> multiPenguinEvents;
    public List<PopupBubbleData> globalEvents;


    [Header("Penguins")]
    public List<Penguin> penguins;

    [Header("Spawn Timing")]
    public Vector2 spawnTimeRange;
    public int maxEventsActive = 3;

    private Coroutine spawnTimer;

    private class EventInstance
    {
        public PopupBubbleController bubble;
        public List<Penguin> affectedPenguins;
    }

    private List<EventInstance> activeEvents = new();

    private void Update()
    {
        if (activeEvents.Count < maxEventsActive)
        {
            spawnTimer ??= StartCoroutine(SpawnAfterTime(Random.Range(spawnTimeRange.x, spawnTimeRange.y)));
        }
    }

    private IEnumerator SpawnAfterTime(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        TriggerRandomEvent();

        spawnTimer = null;
    }

    private void TriggerRandomEvent()
    {
        int roll = Random.Range(0, 100);

        if (roll < 60)
            SpawnSinglePenguinEvent();
        else if (roll < 90)
            SpawnMultiPenguinEvent();
        else
            SpawnGlobalEvent();
    }

    private void SpawnSinglePenguinEvent()
    {
        Penguin target = penguins[Random.Range(0, penguins.Count)];
        List<Penguin> affected = new() { target };

        Vector3 pos = target.transform.position + Vector3.up * 1.5f;

        PopupBubbleData chosenData = singlePenguinEvents[Random.Range(0, singlePenguinEvents.Count)];
        SpawnBubbleEvent(pos, affected, chosenData);

    }

    private void SpawnMultiPenguinEvent()
    {
        Penguin anchor = penguins[Random.Range(0, penguins.Count)];
        int count = Random.Range(2, Mathf.Min(4, penguins.Count));

        List<Penguin> sortedByDistance = penguins.OrderBy(p => Vector2.Distance(p.transform.position, anchor.transform.position)).ToList();
        List<Penguin> affected = sortedByDistance.Take(count).ToList();

        Vector3 center = ComputeCenterPoint(affected) + Vector3.up * 1.5f;

        PopupBubbleData chosenData = multiPenguinEvents[Random.Range(0, multiPenguinEvents.Count)];

        SpawnBubbleEvent(center, affected, chosenData);
    }


    private void SpawnGlobalEvent()
    {
        List<Penguin> affected = new(penguins);

        Vector3 center = ComputeCenterPoint(penguins);

        PopupBubbleData chosenData = globalEvents[Random.Range(0, globalEvents.Count)];
        SpawnBubbleEvent(center, affected, chosenData);
    }

    private Vector3 ComputeCenterPoint(IEnumerable<Penguin> list)
    {
        Vector3 sum = Vector3.zero;
        int count = 0;

        foreach (var p in list)
        {
            sum += p.transform.position;
            count++;
        }

        return sum / count;
    }

    private void SpawnBubbleEvent(Vector3 worldPos, List<Penguin> affectedPenguins, PopupBubbleData data)
    {
        GameObject instance = Instantiate(bubblePrefab, transform);
        PopupBubbleController controller = instance.GetComponent<PopupBubbleController>();

        instance.transform.position = worldPos;

        foreach (var penguin in affectedPenguins)
            penguin.AddDistraction();

        controller.bubbleData = data;
        controller.IconImage.sprite = data.iconSprite;
        controller.FadeIconImage.sprite = data.iconSprite;

        EventInstance evt = new EventInstance
        {
            bubble = controller,
            affectedPenguins = affectedPenguins
        };

        activeEvents.Add(evt);

        controller.bubbleData.onComplete = () =>
        {
            foreach (var penguin in evt.affectedPenguins)
                penguin.RemoveDistraction();

            activeEvents.Remove(evt);
        };
    }



}
