using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PowerUp : MonoBehaviour
{
    // public UnityEvent onJump;
    protected PlayerMovement playerMovement;
    protected GameObject iconHolder;
    protected bool isActive;

    // Start is called before the first frame update
    public virtual void Start()
    {
        playerMovement = PlayerMovement.Instance;
        iconHolder = gameObject.transform.GetChild(0).gameObject;

        gameObject.GetComponent<Collider>().isTrigger = false;
        EventsProvider.Instance.OnPlayerDeath.AddListener(ResetEffect);
        EventsProvider.Instance.OnBackToCheckpoint.AddListener(ResetEffect);
    }

    // Update is called once per frame
    public virtual void ResetEffect()
    {
        
    }

    private void SetState(bool state)
    {
        gameObject.GetComponent<Collider>().enabled = state;
        iconHolder.SetActive(state);
        isActive = !state;
    }

    protected void Disable() => SetState(false);
    protected void Enable() => SetState(true);
}
