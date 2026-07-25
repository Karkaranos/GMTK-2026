using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenUIManager : MonoBehaviour
{
    //PROGRESS
    [SerializeField, BoxGroup("Progress"), Tooltip("Parent of penguin UI.")]
    private Transform _perSecUIHolder;

    //TRAVEL DISTANCE
    [SerializeField, BoxGroup("Travel Distance")]
    private TMP_Text _distanceTravelledText;

    //SHIP PARTS
    [SerializeField, BoxGroup("Ship Parts")]
    private TMP_Text _totalPartQuality;
    [SerializeField, BoxGroup("Ship Parts")]
    private RocketUI _topPart;
    [SerializeField, BoxGroup("Ship Parts")]
    private RocketUI _wingPart;
    [SerializeField, BoxGroup("Ship Parts")]
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
        private Image _warningImage;
        #region GS

        public Image PartImage { get => _partImage; }
        public Image WarningImage { get => _warningImage; }
        #endregion
    }


}
