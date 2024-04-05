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
        if (onCooldown) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            onCooldown = true;
            React();
            ResetAnimation();
            Invoke(nameof(ResetCooldown), animationDuration);
        }
    }

    public virtual void React()
    {

    }

    protected void DisableCollider() => GetComponent<Collider>().enabled = false;

    protected void ResetAnimation() => animator.CrossFade("Frozen", 0f, 0);
    protected void ResetAnimator()
    {
        animator.Play("Frozen", 0, 0);
    }

    protected void ResetCooldown()
    {
        onCooldown = false;
    }
}
