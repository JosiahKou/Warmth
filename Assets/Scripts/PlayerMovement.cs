using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    
    // Stick collection
    public int sticksCollected = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.freezeRotation = true;
    }

    void Update()
    {
        moveInput = new Vector2(
            (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed ? 1 : 0) -
            (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed ? 1 : 0),
            (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed ? 1 : 0) -
            (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed ? 1 : 0)
        );

        if (moveInput.magnitude > 1f)
        {
            moveInput.Normalize();
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveInput * speed;
    }
    
    // Called when the player collects a stick
    public void CollectStick()
    {
        sticksCollected++;
        Debug.Log("Sticks collected: " + sticksCollected);
        
        // You can add additional logic here, like:
        // - Play a collection sound
        // - Update UI
        // - Add the stick to the campfire, etc.
    }
    
    // Optional: Get the current stick count
    public int GetStickCount()
    {
        return sticksCollected;
    }
}