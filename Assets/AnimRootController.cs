using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimRootController : MonoBehaviour
{
    public Animator anim;

    public void AllowRootMotion()
    {
        anim.applyRootMotion = true;
    }

    public void DisallowRootMotion()
    {
        anim.applyRootMotion = false;
    }
}
