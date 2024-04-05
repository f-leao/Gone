using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : PowerUp
{
    public float dashForce;
    public float dashDuration;
    public float maxSpeed;

    public override void Start()
    {
        base.Start();
        EventsProvider.Instance.OnJump.AddListener(ConsumeEffect);
        EventsProvider.Instance.OnLand.AddListener(DelayedEnable);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Disable();
        }
    }

    public override void ConsumeEffect()
    {
        if (!isActive) return;

        base.ConsumeEffect();
        playerMovement.SetGodMode(true);
        playerMovement.rb.useGravity = false;
        playerMovement.ResetVerticalVelocity();
        playerMovement.LaunchInDirection(playerMovement.orientation.forward, dashForce);
        Invoke(nameof(DisengageDashMode), dashDuration);
    }

    private void DisengageDashMode() 
    {
        playerMovement.rb.useGravity = true;
        playerMovement.SetGodMode(false);
        Deactivate();
        DelayedEnable();
    }
}
