using UnityEngine;
public class StickSpawner : MonoBehaviour
{
    [Header("Stick Settings")]
    public GameObject stickPrefab;
    public int maxSticks = 10;
    public float spawnInterval = 2f;

    [Header("Spawn Area")]
    public float spawnRadius = 25f; // Radius of the circular spawn area
    public Vector2 spawnAreaCenter = Vector2.zero; // Center of spawn area

    [Header("Exclusion Zone")]
    public Campfire campfire; // Reference to the campfire object
    public Vector2 exclusionZoneOffset = Vector2.zero; // Offset from campfire position

    private float spawnTimer;
    private int currentStickCount;

    void Update()
    {
        if (currentStickCount < maxSticks)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnInterval)
            {
                SpawnStick();
                spawnTimer = 0f;
            }
        }
    }

    void SpawnStick()
    {
        Vector2 spawnPosition = GetRandomPositionOutsideCircle();

        if (spawnPosition != Vector2.zero)
        {
            GameObject stick = Instantiate(stickPrefab, spawnPosition, Quaternion.Euler(0, 0, Random.Range(0, 360)));
            currentStickCount++;

            // Subscribe to stick destruction to update count
            Stick stickComponent = stick.GetComponent<Stick>();
            if (stickComponent != null)
            {
                stick.GetComponent<Stick>().enabled = true;
            }
        }
    }

    Vector2 GetRandomPositionOutsideCircle()
    {
        int maxAttempts = 30;
        float exclusionRadius = campfire.Radius;
        
        for (int i = 0; i < maxAttempts; i++)
        {
            float randomX = Random.Range(-spawnAreaSize / 2, spawnAreaSize / 2);
            float randomY = Random.Range(-spawnAreaSize / 2, spawnAreaSize / 2);
            Vector2 randomPos = spawnAreaCenter + new Vector2(randomX, randomY);
            
            Vector2 exclusionCenter = campfire.transform.position;
            float distance = Vector2.Distance(randomPos, exclusionCenter);
            
            if (distance >= exclusionRadius)
            {
                return randomPos;
            }
        }

        Debug.LogWarning("Could not find valid spawn position after " + maxAttempts + " attempts");
        return Vector2.zero;
    }
    
    public void OnStickCollected()
    {
        currentStickCount--;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 center3D = new Vector3(spawnAreaCenter.x, spawnAreaCenter.y, 0);
        Vector3 size = new Vector3(spawnAreaSize, spawnAreaSize, 0.1f);
        Gizmos.DrawWireCube(center3D, size);
        
        if (campfire != null)
        {
            Gizmos.color = Color.red;
            DrawCircle(campfire.transform.position, campfire.Radius);
        }
    }

    void DrawCircle(Vector3 center, float radius)
    {
        int segments = 32;
        float angle = 0f;
        Vector3 lastPoint = center + new Vector3(radius, 0, 0);

        for (int i = 1; i <= segments; i++)
        {
            angle = i * 360f / segments * Mathf.Deg2Rad;
            Vector3 newPoint = center + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
            Gizmos.DrawLine(lastPoint, newPoint);
            lastPoint = newPoint;
        }
    }
}