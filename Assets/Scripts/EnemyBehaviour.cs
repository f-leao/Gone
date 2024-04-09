using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyBehaviour : MonoBehaviour
{
    public Transform player;
    public float speed = 5f;
    public GameObject explosionPrefab;
    public float explosionDuration = 2f;

    [ReadOnly][SerializeField] private Vector3 direction;

    [ReadOnly] [SerializeField] private bool isEngaged;
    Animator animator;
    Transform startTransform;

    void Start()
    {
        animator = GetComponent<Animator>();

        startTransform = transform;

        Reset();

        startTransform = transform;

        EventsProvider.Instance.OnIntruderAlarm.AddListener(ActivateChase);
        EventsProvider.Instance.OnPlayerDeath.AddListener(Reset);
    }

    private void FixedUpdate()
    {
        FollowPlayer();
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject obj = collision.gameObject;

        if (obj.CompareTag("Player"))
        {
            PlayerMovement playerMovement = obj.GetComponent<PlayerMovement>();
            playerMovement.TakeHit(direction);
            EventsProvider.Instance.OnPlayerHit.Invoke();
        }
    }

    private void FollowPlayer()
    {

        if (!isEngaged) return;

        Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
        
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        transform.LookAt(targetPosition);
    }

    public void Explode()
    {
        isEngaged = false;
        Instantiate(explosionPrefab, gameObject.transform.position, Quaternion.identity);
        Die();
    }

    private void Die()
    {
        EventsProvider.Instance.OnIntruderAlarm.RemoveListener(ActivateChase);
        EventsProvider.Instance.OnPlayerDeath.RemoveListener(Reset);
        transform.SetParent(null);
        Destroy(gameObject);
    }

    private void ActivateChase()
    {
        isEngaged = true;
        animator.CrossFade("Slow Run", 0f, 0);
    }

    private void Reset()
    {
        isEngaged = false;
        animator.CrossFade("Idle", 0f, 0);
        transform.position = startTransform.position;
    }
}
