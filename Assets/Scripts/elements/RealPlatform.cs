using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealPlatform : PuzzlePlatform
{
    public override void Start()
    {
        base.Start();
    }
    
    public override void React()
    {
        animator.CrossFade("Dip", 0f, 0);
        Invoke(nameof(ResetPlatform), animationDuration);
    }
}
