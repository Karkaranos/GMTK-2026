using NaughtyAttributes;
using UnityEngine;

public class AlertEnvironmentManager : MonoBehaviour
{
    [SerializeField, BoxGroup("Fire Alarm")]
    private Animator _fireAlarmAnimator;
    [SerializeField, BoxGroup("Fire Alarm")]
    private string _animFireAlarmOnID = "B_ON";
    public void StartFireAlarm()
    {
        Debug.Log("Started");
        _fireAlarmAnimator.SetBool(_animFireAlarmOnID, true);
    }

    public void EndFireAlarm()
    {
        Debug.Log("End");
        _fireAlarmAnimator.SetBool(_animFireAlarmOnID, false);
    }
}
