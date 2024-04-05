using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakePlatform : PuzzlePlatform
{
    public override void Start()
    {
        base.Start();
    }

    public override void React()
    {
        animator.CrossFade("Fall", 0f, 0);
        Invoke(nameof(DisableCollider), animationDuration/2);
        Destroy(gameObject, animationDuration);
    }
}
