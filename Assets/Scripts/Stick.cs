using UnityEngine;

public class Stick : MonoBehaviour
{
    [Header("Visual Settings")]
    public float rotationSpeed = 100f;
    
    private StickSpawner spawner;
    
    void Start()
    {
        // Find the spawner in the scene
        spawner = FindFirstObjectByType<StickSpawner>();
        
        if (spawner == null)
        {
            Debug.LogWarning("StickSpawner not found in scene!");
        }
    }
    
    void Update()
    {
        // Make the stick rotate for visual appeal
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player touched this stick
        if (other.CompareTag("Player"))
        {
            // Get the player's movement script and add to their stick count
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.CollectStick();
            }
            else
            {
                Debug.LogWarning("PlayerMovement script not found on player!");
            }
            
            // Notify spawner that a stick was collected so it can spawn more
            if (spawner != null)
            {
                spawner.OnStickCollected();
            }
            
            Debug.Log("Stick collected!");
            
            // Destroy this stick object
            Destroy(gameObject);
        }
    }
}