using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Items/Item")]
public class Item : ScriptableObject
{
    public string id;
    public GameObject item;

    public PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = PlayerMovement.Instance;
    }
}
