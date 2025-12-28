using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Campfire : MonoBehaviour
{
    [SerializeField] private float radius = 4f;
    [SerializeField] private Transform player;
    [SerializeField] private Tilemap zoneTilemap;
    [SerializeField] private TileBase campFireTile;
    [SerializeField] private TileBase innerTile;
    [SerializeField] private TileBase middleTile;
    [SerializeField] private TileBase outerTile;
    [SerializeField][Range(0f, 1f)] private float innerPercent = 0.3f;
    [SerializeField][Range(0f, 1f)] private float middlePercent = 0.6f;

    [SerializeField] private float playerHealth = 10f;
    private float maxHealth = 10f;
    private float damageRate = 1f;
    private float healRate = 2f;
    private Slider healthBar;

    void Start()
    {
        PlaceCampfireTile();
        DrawZoneTiles();
        playerHealth = maxHealth;

        healthBar = GameObject.Find("HealthBar")?.GetComponent<Slider>();
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = playerHealth;
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= radius)
        {
            playerHealth += healRate * Time.deltaTime;
            playerHealth = Mathf.Min(playerHealth, maxHealth);
        }
        else
        {
            playerHealth -= damageRate * Time.deltaTime;
            playerHealth = Mathf.Max(playerHealth, 0f);
        }

        if (healthBar != null)
        {
            healthBar.value = playerHealth;
        }

        if (playerHealth <= 0)
        {
            Debug.Log("Player died!");
        }
    }

    void PlaceCampfireTile()
    {
        if (zoneTilemap == null || campFireTile == null) return;

        Vector3Int centerPos = new Vector3Int(
            Mathf.RoundToInt(transform.position.x),
            Mathf.RoundToInt(transform.position.y),
            0
        );

        zoneTilemap.SetTile(centerPos, campFireTile);
    }

    public void DrawZoneTiles()
    {
        if (zoneTilemap == null || innerTile == null || middleTile == null || outerTile == null)
            return;

        zoneTilemap.ClearAllTiles();

        PlaceCampfireTile();

        float innerRadius = radius * innerPercent;
        float middleRadius = radius * middlePercent;

        int intRadius = Mathf.CeilToInt(radius);

        for (int x = -intRadius; x <= intRadius; x++)
        {
            for (int y = -intRadius; y <= intRadius; y++)
            {
                if (x == 0 && y == 0) continue;

                float distance = Mathf.Sqrt(x * x + y * y);

                if (distance <= radius)
                {
                    Vector3Int tilePos = new Vector3Int(
                        Mathf.RoundToInt(transform.position.x) + x,
                        Mathf.RoundToInt(transform.position.y) + y,
                        0
                    );

                    if (distance <= innerRadius)
                    {
                        zoneTilemap.SetTile(tilePos, innerTile);
                    }
                    else if (distance <= middleRadius)
                    {
                        zoneTilemap.SetTile(tilePos, middleTile);
                    }
                    else
                    {
                        zoneTilemap.SetTile(tilePos, outerTile);
                    }
                }
            }
        }
    }

    public void SetRadius(float newRadius)
    {
        radius = newRadius;
        DrawZoneTiles();
    }

    public float GetRadius()
    {
        return radius;
    }

    public void UpdateZonePercentages(float newInnerPercent, float newMiddlePercent)
    {
        innerPercent = Mathf.Clamp01(newInnerPercent);
        middlePercent = Mathf.Clamp(newMiddlePercent, innerPercent, 1f);
        DrawZoneTiles();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public float getHealth()
    {
        return playerHealth;
    }

    public float getMaxHealth()
    {
        return maxHealth;
    }

    public float GetRadius()
    {
        return radius;
    }
}