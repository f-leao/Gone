using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : SingletonMonoBehaviour<EnemyManager>
{

    void FixedUpdate()
    {
        if (transform.childCount == 0)
            EventsProvider.Instance.OnAllowWin.Invoke();
    }
}
