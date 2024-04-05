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
        EventsProvider.Instance.OnLand.AddListener(ConsumeEffect);
    }

    // void OnCollisionEnter(Collision collision)
    // {
    //     if (collision.gameObject.CompareTag("Player"))
    //     {
    //         playerMovement.rb.mass *= gravityMultiplier;
    //         playerMovement.airMultiplier *= airMultiplier;
    //         base.Disable();
    //     }
    // }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerMovement.rb.mass *= gravityMultiplier;
            playerMovement.airMultiplier *= airMultiplier;
            Disable();
        }
    }


    public override void ConsumeEffect()
    {
        if (!isActive) return;

        base.ConsumeEffect();
        playerMovement.rb.mass /= gravityMultiplier;
        playerMovement.airMultiplier /= airMultiplier;
        Enable();
    }
}
