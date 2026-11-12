using UnityEngine;

/// <summary>
/// Controla a movimentacao horizontal e o pulo do personagem.
/// Toda alteracao de posicao ocorre via Rigidbody, no FixedUpdate.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Referencias")]
    public GroundCheck groundCheck;

    [Header("Parametros")]
    [Tooltip("Velocidade horizontal em unidades por segundo.")]
    public float moveSpeed = 6f;

    [Tooltip("Velocidade vertical aplicada no inicio do pulo.")]
    public float jumpForce = 9f;

    private Rigidbody body;
    private float horizontalInput;
    private bool jumpRequested;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();

        body.constraints |= RigidbodyConstraints.FreezePositionZ;
        body.constraints |= RigidbodyConstraints.FreezeRotationX;
        body.constraints |= RigidbodyConstraints.FreezeRotationY;
        body.constraints |= RigidbodyConstraints.FreezeRotationZ;

        if (groundCheck == null)
        {
            groundCheck = GetComponentInChildren<GroundCheck>();
        }
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            jumpRequested = true;
        }
    }

    private void FixedUpdate()
    {
        if (body == null)
        {
            return;
        }

        Vector3 velocity = body.velocity;
        velocity.x = horizontalInput * moveSpeed;
        velocity.z = 0f;
        body.velocity = velocity;

        if (!jumpRequested)
        {
            return;
        }

        jumpRequested = false;

        bool canJump = groundCheck != null && groundCheck.IsGrounded;

        if (canJump && body.velocity.y <= 0.1f)
        {
            body.velocity = new Vector3(body.velocity.x, jumpForce, 0f);
        }
    }
}
