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

    void Start()
    {
        isEngaged = false;
        animator = GetComponent<Animator>();
        animator.CrossFade("Idle", 0f, 0);

        EventsProvider.Instance.OnIntruderAlarm.AddListener(ActivateChase);
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
        Instantiate(explosionPrefab, gameObject.transform.position, Quaternion.identity);
        Invoke(nameof(Die), explosionDuration);
    }

    private void Die() => Destroy(gameObject);

    private void ActivateChase()
    {
        isEngaged = true;
        animator.CrossFade("Slow Run", 0f, 0);
    }
}
