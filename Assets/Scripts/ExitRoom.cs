using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExitRoom : SingletonMonoBehaviour<ExitRoom>
{
    [SerializeField] Transform exitGate;
    [SerializeField] Transform exitSign;

    void Start()
    {
        SetExitRoomState(false);

        EventsProvider.Instance.OnAllowWin.AddListener(AllowWin);
    }

    void AllowWin() => SetExitRoomState(true);

    void SetExitRoomState(bool state)
    {
        exitGate.gameObject.SetActive(state);
        exitSign.GetComponentInChildren<Light>().enabled = state;
    }
}
