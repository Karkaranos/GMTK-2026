using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ProgressManager : Manager
{
    public static ProgressManager INST;

    [SerializeField, MinValue(0)]
    private float _tick = 1;

    [SerializeField]
    private float _distanceModifier = 1;

    [SerializeField, ReadOnly]
    private float totalProgress;
    [SerializeField, ReadOnly]
    private float perSecondProgress;
    [SerializeField, ReadOnly]
    private float distanceFlown; //max 1620
    [SerializeField, ReadOnly]
    private float shipQuality = 0; //max 9

    public static float DistanceFlown { get; private set; }

    //MANAGERS
    private BuildingManager buildMan;
    private PenguinManager penguinMan;
    private ScreenUIManager screenUIMan;

    public override void Initialize()
    {
        if (INST == null)
            INST = this;
        else if (INST != this)
            Debug.LogError("There are multiple instances of ProgressManager. There can only be one.");

        buildMan = FindAnyObjectByType<BuildingManager>();
        penguinMan = FindAnyObjectByType<PenguinManager>();
        screenUIMan = penguinMan.GetComponent<ScreenUIManager>();

        CountdownManager.OnCountdownFinished += SaveDistanceFlown;
    }

    private void OnDestroy()
    {
        CountdownManager.OnCountdownFinished -= SaveDistanceFlown;
    }

    private void SaveDistanceFlown()
    {
        DistanceFlown = distanceFlown;
    }

    private void Start()
    {
        StartCoroutine(Tick());
    }

    private IEnumerator Tick()
    {
        while (true)
        {
            yield return new WaitForSeconds(_tick);

            CalculatePerSecondProgress();
            AddProgress(perSecondProgress);

            CalculateShipQuality();

            CalculateDistanceFlown();
        }
    }

    private void AddProgress(float amount)
    {
        totalProgress += amount;
        Mathf.Clamp(totalProgress, 0, 100);
    }

    #region Calculate
    private void CalculatePerSecondProgress()
    {
        perSecondProgress = 1 - penguinMan.GetDistractedPercentage();
    }

    //Calculate total of ship quality. If a part isn't built, 0 quality score for it.
    private void CalculateShipQuality()
    {
        Dictionary<RocketSection, RocketPart> parts = buildMan.GetParts();

        float topScore = (parts[RocketSection.Top] == null)? 0 : parts[RocketSection.Top].Quality;
        float wingScore = (parts[RocketSection.Wings] == null) ? 0 : parts[RocketSection.Wings].Quality;
        float engineScore = (parts[RocketSection.Engine] == null) ? 0 : parts[RocketSection.Engine].Quality;

        shipQuality = topScore + wingScore + engineScore;
        if (shipQuality < 0)
            shipQuality = 0;

        //UI
        //screenUIMan.TopPart.PartImage.sprite = 
        screenUIMan.TotalPartQuality.text = shipQuality.ToString();

        screenUIMan.TopPart.QualityText.text = topScore.ToString();
         if (parts[RocketSection.Top] != null) 
            screenUIMan.TopPart.PartImage.sprite = parts[RocketSection.Top].Sprite;

        screenUIMan.WingPart.QualityText.text = wingScore.ToString();
        if (parts[RocketSection.Wings] != null)
            screenUIMan.WingPart.PartImage.sprite = parts[RocketSection.Wings].Sprite;

        screenUIMan.EnginePart.QualityText.text = engineScore.ToString();
        if (parts[RocketSection.Engine] != null)
            screenUIMan.EnginePart.PartImage.sprite = parts[RocketSection.Engine].Sprite;
    }

    private void CalculateDistanceFlown()
    {
        distanceFlown = shipQuality * totalProgress * _distanceModifier;

        //UI
        screenUIMan.DistanceTravelledText.text = distanceFlown.ToString();
    }
    #endregion
}
