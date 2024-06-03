using UnityEngine;

public class TriggeredObject : MonoBehaviour
{
    private Animator _anim;

    private static readonly int OpenOrClose = Animator.StringToHash("OpenOrClose");

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    public void Trigger()
    {
        _anim.SetTrigger(OpenOrClose);
    }
}
