using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class IsometricSpriteController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 6f;

    [Header("Animation")]
    public Animator animator;

    private Rigidbody rb;
    private InputSystem_Actions controls;
    private Vector2 moveInput;
    private Vector2 lastMoveDirection = new Vector2(0, -1); // Default facing South

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        controls = new InputSystem_Actions();
    }

    private void OnEnable() => controls.Player.Enable();
    private void OnDisable() => controls.Player.Disable();

    private void Update()
    {
        moveInput = controls.Player.Move.ReadValue<Vector2>();

        // Update Animator parameters
        if (moveInput.sqrMagnitude > 0.01f)
        {
            lastMoveDirection = moveInput.normalized;
            animator.SetFloat("Speed", moveInput.sqrMagnitude);
        }
        else
        {
            animator.SetFloat("Speed", 0f);
        }

        // Feed the input direction to the Blend Tree
        animator.SetFloat("MoveX", lastMoveDirection.x);
        animator.SetFloat("MoveY", lastMoveDirection.y);
    }

    private void FixedUpdate()
    {
        // Move in 3D space, but DO NOT rotate the Rigidbody
        Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y).normalized;
        rb.linearVelocity = new Vector3(movement.x * moveSpeed, rb.linearVelocity.y, movement.z * moveSpeed);
    }
}