using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenUIManager : MonoBehaviour
{
    [SerializeField, BoxGroup("Screens"), Tooltip("Parent of penguin UI.")]
    private Transform _perSecUIHolder;
    [SerializeField, BoxGroup("Screens")]
    private TMP_Text _distanceTravelledText;

    [SerializeField, BoxGroup("Screens")]
    private TMP_Text _totalPartQuality;
    [SerializeField, BoxGroup("Screens")]
    private RocketUI _topPart;
    [SerializeField, BoxGroup("Screens")]
    private RocketUI _wingPart;
    [SerializeField, BoxGroup("Screens")]
    private RocketUI _enginePart;

    #region GS
    public TMP_Text DistanceTravelledText { get => _distanceTravelledText; }
    public Transform PerSecUIHolder { get => _perSecUIHolder; }
    public RocketUI TopPart { get => _topPart; set => _topPart = value; }
    public RocketUI WingPart { get => _wingPart; set => _wingPart = value; }
    public RocketUI EnginePart { get => _enginePart; set => _enginePart = value; }
    public TMP_Text TotalPartQuality { get => _totalPartQuality; }
    #endregion

    [System.Serializable]
    public struct RocketUI
    {
        [SerializeField]
        private Image _partImage;

        [SerializeField]
        private TMP_Text _qualityText;
        #region GS

        public Image PartImage { get => _partImage; }
        public TMP_Text QualityText { get => _qualityText; }
        #endregion
    }
}
