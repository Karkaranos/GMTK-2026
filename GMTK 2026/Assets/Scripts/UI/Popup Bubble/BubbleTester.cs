using NaughtyAttributes;
using UnityEngine;

public class BubbleTester : MonoBehaviour
{
    public PopupBubbleData testBubbleData;

    private void Start()
    {
        testBubbleData.onComplete = () => { TestListener(); };
    }

#if UNITY_EDITOR
    private bool InPlayMode => UnityEditor.EditorApplication.isPlaying;
    [Button, ShowIf("InPlayMode")]
#endif
    public void Spawn()
    {
        PopupBubbleManager.Instance.SpawnPopupBubble(testBubbleData, Vector3.zero);
    }

    public void TestListener()
    {
        Debug.Log("Recieved onComplete callback");
    }    
}
