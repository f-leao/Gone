using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : SingletonMonoBehaviour<PlayerMovement>
{
    #region Variables

    #region Movement
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    [SerializeField] private bool isSprinting;

    public float groudDrag;
    #endregion

    #region Jumping
    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;
    bool isJumping;
    #endregion

    #region Key Binds
    [Header("Key Binds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode releaseKey = KeyCode.LeftControl;
    public KeyCode nextCameraKey = KeyCode.C;
    public KeyCode lastCheckpointKey = KeyCode.B;
    public KeyCode nextCheckpointKey = KeyCode.N;
    #endregion

    #region Ground Check
    [Header("Groud Check")]
    public float playerHeight;
    public LayerMask groundMask;
    bool isGrounded;
    public RaycastHit groundInfo;
    #endregion

    #region Slope Handling
    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private bool onSlope;
    #endregion

    #region Ledge Grab
    [Header("Ledge Grab")]
    public float ledgeMaxDistance;
    public float hangCooldown;
    [ReadOnly][SerializeField] bool isHanging;
    bool readyToHang;
    public RaycastHit ledgeInfo;
    #endregion

    #region References
    [Header("References")]
    public Transform orientation;
    public Transform playerModel;

    public Transform feetPosition;
    public Transform headPosition;

    public Rigidbody rb;
    #endregion

    #region Input
    float horizontalInput;
    float verticalInput;
    #endregion

    #region Auxiliar
    Vector3 moveDirection;
    bool isInGodMode;
    #endregion

    #region Move State
    [SerializeField] private MoveState moveState;

    public enum MoveState
    {
        Walking,
        Sprinting,
        Hanging,
        Air,
        GodSpeed
    }
    #endregion

    #endregion

    #region Runtime
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        readyToHang = true;
        isInGodMode = false;
        EventsProvider.Instance.OnBackToCheckpoint.AddListener(Freeze);
    }

    // Update is called once per frame
    void Update()
    {
        CheckGround();
        CheckLedge();
        HandleState();
        ProcessInput();
        ControlDrag();
        ControlSpeed();
    }

    void FixedUpdate()
    {
        //update playerModel rotation
        playerModel.rotation = Quaternion.Euler(0f, orientation.rotation.eulerAngles.y, 0f);

        MovePlayer();
    }
    #endregion

    private void ProcessInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(releaseKey))
        {
            readyToHang = false;
            Invoke(nameof(ResetHang), hangCooldown);
        }
        
        if (Input.GetKey(jumpKey))
            ProcessSpacePressed();

        isSprinting = Input.GetKey(sprintKey);

        if (Input.GetKeyDown(nextCameraKey))
            PlayerCam.Instance.NextCameraPosition();

        if (Input.GetKeyDown(lastCheckpointKey))
            GameManager.Instance.BackToCheckpoint();

        if (Input.GetKeyDown(nextCheckpointKey))
            GameManager.Instance.NextCheckpoint();
    }

    private void ProcessSpacePressed()
    {

        if (readyToJump)
        {
            EventsProvider.Instance.OnJump.Invoke();

            if (isGrounded)
                Jump();
            else if (isHanging)
                JumpWhileHanging();
        }
    }

    private void HandleState()
    {
        if (isInGodMode)
        {
            moveState = MoveState.GodSpeed;
            moveSpeed = sprintSpeed*2f;
        }
        else if (isGrounded)
        {
            moveState = MoveState.Walking;
            moveSpeed = walkSpeed;

            if (isSprinting)
            {
                moveState = MoveState.Sprinting;
                moveSpeed = sprintSpeed;
            }
        }
        else if (isHanging)
            moveState = MoveState.Hanging;
        else
            moveState = MoveState.Air;
    }

    private void MovePlayer()
    {
        moveDirection = GetMoveDirection();

        float multiplier = 1f;

        if (!(isGrounded || isHanging))
            multiplier *= airMultiplier;

        //if hanging, only move sideways
        if (isHanging)
            moveDirection = orientation.right * horizontalInput;

        rb.AddForce(moveDirection.normalized * moveSpeed * 10f * multiplier * rb.mass, ForceMode.Force);
    }

    private void CheckGround()
    {
        RaycastHit[] hit = Physics.SphereCastAll(feetPosition.position,
                                  0.3f,
                                  Vector3.down,
                                  0f,
                                  groundMask);

        isGrounded = hit.Length > 0;

        if (isGrounded)
        {
            Land();
            groundInfo = hit[0];
        }
    }

    private void CheckLedge()
    {
        if (isHanging = (readyToHang && !isGrounded && Physics.Raycast(headPosition.position, orientation.forward, out ledgeInfo, ledgeMaxDistance, groundMask)) )
            {
                Freeze();
                rb.useGravity = false;
                PlayerCam.Instance.LookAtPoint(ledgeInfo.normal * -1f);
                Land();
            }
        else
            rb.useGravity = true;
    }

    private bool OnSlope()
    {
        float angle = Mathf.Abs(Vector3.Angle(Vector3.up, groundInfo.normal));

        return angle < maxSlopeAngle && angle > 0f && isGrounded;
    }

    private Vector3 GetMoveDirection()
    {
        Vector3 vert = orientation.forward * verticalInput;
        Vector3 hor = orientation.right * horizontalInput;

        Vector3 temp = vert + hor;

        //if hanging only allow horizontal movement
        if (isHanging)
            return hor;

        return OnSlope() ? Vector3.ProjectOnPlane(temp, groundInfo.normal) : temp;
    }

    private void ControlDrag() => rb.drag = isGrounded ? groudDrag : 0f;

    private void ControlSpeed()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    public void LaunchInDirection(Vector3 direction, float force)
    {
        rb.AddForce(direction.normalized * force, ForceMode.Impulse);
    }

    public void Jump()
    {
        readyToJump = false;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        LaunchInDirection(Vector3.up, jumpForce);

        Invoke(nameof(ResetJump), jumpCooldown);
    }

    public void JumpWhileHanging()
    {
        readyToHang = false;
        Jump();
        Invoke(nameof(ResetHang), hangCooldown);
    }

    private void Land()
    {
        if (!isJumping) return;

        isJumping = false;
        EventsProvider.Instance.OnLand.Invoke();
    }

    private void ResetJump() => readyToJump = isJumping = true;

    private void ResetHang() => readyToHang = true;

    private void OnCollisionEnter(Collision collision)
    {
        GameObject obj = collision.gameObject;

        if (obj.layer == LayerMask.NameToLayer("kill"))
        {
            GameManager.Instance.KillPlayer();
        }
    //     else if (obj.layer == LayerMask.NameToLayer("end"))
    //     {
    //         GameManager.Instance.EndGame();
    //     }
    //     else if (isJumping)
    //     {
    //         onJump.Invoke();
    //         isJumping = false;
    //     }
    //     else if (isGrounded)
    //     {
    //         onLand.Invoke();
    //     }
    }

    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("Checkpoint"))
    //     {
    //         GameManager.Instance.SetCheckpoint(other.name);
    //     }
    // }

    public void Freeze() => rb.velocity = Vector3.zero;

    public void ResetVerticalVelocity() => rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

    public bool IsGrounded() => isGrounded;

    public void SetGodMode(bool state) => isInGodMode = state;
}
