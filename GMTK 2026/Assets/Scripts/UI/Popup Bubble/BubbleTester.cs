using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

public class BubbleTester : MonoBehaviour
{
    public PopupBubbleData testBubbleData;

    private void Start()
    {
        testBubbleData.onComplete = () => { TestListener(); };
    }

    private bool InPlayMode => EditorApplication.isPlaying;
    [Button, ShowIf("InPlayMode")]
    public void Spawn()
    {
        PopupBubbleManager.Instance.SpawnPopupBubble(testBubbleData, Vector3.zero);
    }

    public void TestListener()
    {
        Debug.Log("Recieved onComplete callback");
    }    
}
