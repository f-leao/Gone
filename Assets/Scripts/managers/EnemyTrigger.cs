using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyTrigger : MonoBehaviour
{

    void Start()
    {
        EventsProvider.Instance.OnPlayerDeath.AddListener(Reset);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EventsProvider.Instance.OnIntruderAlarm.Invoke();
            //disable
            GetComponent<Collider>().enabled = false;
        }
    }

    void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }
}
