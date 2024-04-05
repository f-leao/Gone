using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatus : MonoBehaviour
{

    [Header("Health")]
    public int maxHP;
    [ReadOnly][SerializeField] int currentHP;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
        EventsProvider.Instance.OnPlayerDeath.AddListener(HealToMax);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
    }

    public bool IsDead() => (currentHP <= 0);

    public void Heal(int amount)
    {
        currentHP += amount;
        if (currentHP > maxHP)
            currentHP = maxHP;
    }

    public void HealToMax() => Heal(maxHP);
    public int GetHP() => currentHP;

}
