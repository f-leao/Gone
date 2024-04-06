using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserAmmo : MonoBehaviour
{

    public float cooldown = 5f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EventsProvider.Instance.OnAmmoPickup.Invoke();
            Disable();
            Invoke(nameof(Enable), cooldown);
        }
    }

    private void Enable() => SetState(true);
    private void Disable() => SetState(false);
    
    private void SetState(bool state)
    {
        gameObject.SetActive(state);
    }
}
