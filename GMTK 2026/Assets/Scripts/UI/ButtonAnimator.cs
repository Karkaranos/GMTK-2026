using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonAnimator : MonoBehaviour
{
    [SerializeField, BoxGroup("Components")]
    private Animator _animator;

    [SerializeField, BoxGroup("Animation")]
    private string _isHoveredID = "B_HOVERED";
    [SerializeField, BoxGroup("Animation")]
    private string _confirmID = "T_CONFIRM";

    public void SetHover(bool isHovered)
        => _animator.SetBool(_isHoveredID, isHovered);

    public void SetConfirm()
        => _animator.SetTrigger(_confirmID);

}
