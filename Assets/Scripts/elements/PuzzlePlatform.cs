using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePlatform : MonoBehaviour
{
    public float animationDuration = 1.0f;
    protected Animator animator;
    protected bool onCooldown = false;
    public virtual void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {

        collision.transform.SetParent(transform);

        if (collision.gameObject.CompareTag("Player"))
        {
            if (onCooldown) return;
            
            onCooldown = true;
            React();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        collision.transform.SetParent(null);
    }

    public virtual void React()
    {

    }

    protected void DisableCollider() => GetComponent<Collider>().enabled = false;

    protected void ResetPlatform()
    {
        ResetCooldown();
        animator.CrossFade("Idle", 0f, 0);
    }

    protected void ResetCooldown()
    {
        onCooldown = false;
    }

    protected void AnimateState(string state, float fadeDuration)
    {
        if (onCooldown) return;

        animator.CrossFade(state, fadeDuration, 0);
    }
}
