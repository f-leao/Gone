using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PowerUp : MonoBehaviour
{
    public float respawnCooldown = 3f;
    protected PlayerMovement playerMovement;
    protected bool isActive;

    // Start is called before the first frame update
    public virtual void Start()
    {
        playerMovement = PlayerMovement.Instance;

        EventsProvider.Instance.OnPlayerDeath.AddListener(ConsumeEffect);
        EventsProvider.Instance.OnBackToCheckpoint.AddListener(ConsumeEffect);
    }

    // Update is called once per frame
    public virtual void ConsumeEffect()
    {
        
    }

    private void SetState(bool state)
    {
        gameObject.GetComponent<Collider>().enabled = state;
        transform.gameObject.SetActive(state);
        isActive = !state;
    }

    protected void Disable() => SetState(false);
    protected void Enable() => SetState(true);
    protected void DelayedEnable() => Invoke(nameof(Enable), respawnCooldown);
    protected void Activate() => isActive = true;
    protected void Deactivate() => isActive = false;
}
