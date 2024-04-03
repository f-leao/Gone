using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReduceGravity : PowerUp
{
    public float gravityMultiplier;
    public float airMultiplier;

    public override void Start()
    {
        base.Start();
        EventsProvider.Instance.OnLand.AddListener(ResetEffect);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerMovement.rb.mass *= gravityMultiplier;
            playerMovement.airMultiplier *= airMultiplier;
            base.Disable();
        }
    }

    public override void ResetEffect()
    {
        if (!isActive) return;

        base.ResetEffect();
        playerMovement.rb.mass /= gravityMultiplier;
        playerMovement.airMultiplier /= airMultiplier;
        base.Enable();
    }
}
