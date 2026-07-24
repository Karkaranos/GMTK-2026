using NaughtyAttributes;
using System;
using UnityEngine;
using UnityEngine.UI;

public class SectionBuilder : MonoBehaviour
{
    [SerializeField] private RocketPartDatabase partDB;
    [SerializeField] private BuildingSection section;
    [SerializeField] private Image partDisplay;
    public ProgressBar buildBar;

    [SerializeField, ReadOnly] private CanvasGroup group;

    private RocketPart[] parts;
    private int selectedPart;

    private int SelectedPart
    {
        get => selectedPart;
        set
        {
            selectedPart = ((value % parts.Length) + parts.Length) % parts.Length;
            partDisplay.sprite = parts[selectedPart].Sprite;
        }
    }

    [ContextMenu("Get Components")]
    private void Reset()
    {
        group = GetComponent<CanvasGroup>();
    }

    private void Awake()
    {
        parts = Array.Find(partDB.Database, x => x.Section == section.Section).Parts;
        SelectedPart = selectedPart;
        buildBar.OnBarFinish += OnBarFinish;
        if (section.BuildingBar == null)
        {
            section.BuildingBar = buildBar;
        }
    }

    public void ScrollPart(int direction)
    {
        SelectedPart += direction;
    }

    public void Build()
    {
        buildBar.gameObject.SetActive(true);
        group.interactable = false;
        section.OnBeginBuild();
    }

    private void OnBarFinish()
    {
        buildBar.gameObject.SetActive(false);
        group.interactable = true;

        section.SetPart(parts[selectedPart]);
        section.OnEndBuild();
        
    }
}
