using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointState : MonoBehaviour
{

    Transform active;
    Transform inactive;

    void Start()
    {
        active = transform.Find("Active");
        inactive = transform.Find("Inactive");
        Deactivate();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.SetCheckpoint(this.name);
        }
    }
    
    public void Activate()
    {
        CheckpointActive(true);
    }

    public void Deactivate()
    {
        CheckpointActive(false);
    }

    private void CheckpointActive(bool state)
    {
        active.gameObject.SetActive(state);
        inactive.gameObject.SetActive(!state);
        GetComponent<Collider>().enabled = !state;
    }
}
