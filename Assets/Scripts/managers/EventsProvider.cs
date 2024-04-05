using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventsProvider : SingletonMonoBehaviour<EventsProvider>
{
    public UnityEvent OnShowPlayer;
    public UnityEvent OnHidePlayer;
    public UnityEvent OnBackToCheckpoint;
    public UnityEvent OnJump;
    public UnityEvent OnLand;
    public UnityEvent OnPlayerDeath;
    public UnityEvent OnFellForTrap;
    public UnityEvent OnGodSpeed;
}
