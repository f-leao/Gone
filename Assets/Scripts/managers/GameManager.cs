using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : SingletonMonoBehaviour<GameManager>, IManager
{

    [Header("Player")]
    public GameObject player;
    private PlayerMovement playerMovement;
    private Transform playerPos;
    private CharacterStatus playerStatus;

    [Header("Checkpoints")]
    GameObject checkpointsObject;
    [ReadOnly][SerializeField] Dictionary<string, Transform> checkpoints;
    [ReadOnly][SerializeField] int currentCheckpointIndex;
    [ReadOnly][SerializeField] Transform currentCheckpoint;
    int nCheckpoints;

    [Header("UI")]
    public TMPro.TextMeshProUGUI deathCountText;
    int deathCount;
    public TMPro.TextMeshProUGUI playerHPText;

    // Start is called before the first frame update
    void Start()
    {
        LoadCheckpoints();
        LoadPlayerInfo();
        PutPlayerAtCheckpoint();
        deathCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerStatus.IsDead())
            KillPlayer();
    }

    void FixedUpdate()
    {
        deathCountText.text = "Deaths: " + deathCount;
        playerHPText.text = "HP: " + playerStatus.GetHP();
    }

    private void LoadCheckpoints()
    {
        checkpointsObject = GameObject.Find("Checkpoints");

        if (checkpointsObject == null)
        {
            Debug.LogError("Checkpoints object not found!");
            return;
        }

        nCheckpoints = checkpointsObject.transform.childCount;
        
        checkpoints = new Dictionary<string, Transform>(nCheckpoints);

        foreach (Transform checkpoint in checkpointsObject.transform)
        {
            checkpoints.Add(checkpoint.name, checkpoint);
        }

        currentCheckpoint = checkpoints["Spawn"];

        currentCheckpointIndex = 0;

    }

    private void LoadPlayerInfo()
    {
        playerMovement = player.GetComponent<PlayerMovement>();
        playerPos = player.transform;
        playerStatus = player.GetComponent<CharacterStatus>();
    }

    public void Kill(GameObject caller)
    {
        //check if player is the caller
        if (caller != player)
        {
            //fail
            Debug.LogError("Caller is not the player");
            return;
        }
        KillPlayer();
    }

    public void KillPlayer()
    {
        BackToCheckpoint();
        EventsProvider.Instance.OnPlayerDeath.Invoke();
        deathCount++;
    }

    public void BackToCheckpoint() {
        EventsProvider.Instance.OnBackToCheckpoint.Invoke();
        PutPlayerAtCheckpoint();
    }

    private void PutPlayerAtCheckpoint()
    {
        player.SetActive(false);
        playerPos.position = currentCheckpoint.position;
        PlayerCam.Instance.SetPlayerRotation(currentCheckpoint.rotation.eulerAngles.y);
        player.SetActive(true);
    }

    public void NextCheckpoint()
    {
        UpdateCheckpointIndex();
        currentCheckpointIndex = ++currentCheckpointIndex % nCheckpoints;
        SetCheckpoint(checkpointsObject.transform.GetChild(currentCheckpointIndex).name);
        BackToCheckpoint();
    }

    public void SetCheckpoint(string checkpointName) 
    {
        //deactivate old checkpoint
        currentCheckpoint.GetComponent<CheckpointState>().Deactivate();

        //update current checkpoint
        currentCheckpoint = checkpoints[checkpointName];

        //activate new checkpoint
        currentCheckpoint.GetComponent<CheckpointState>().Activate();
    }

    private void UpdateCheckpointIndex() => currentCheckpointIndex = currentCheckpoint.GetSiblingIndex();
    
}
