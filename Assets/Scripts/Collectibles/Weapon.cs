using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Items/Weapon")]
public class Weapon : Item
{
    [Header("Characteristics")]
    public float damage;
    public float range;
    public float fireRate;
    public int maxAmmo;

    [Header("Ammo")]
    [SerializeField] private int currentAmmo;

    public void Shoot() => currentAmmo--;

}
