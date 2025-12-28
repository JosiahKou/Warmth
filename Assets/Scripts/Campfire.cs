using UnityEngine;
using UnityEngine.Tilemaps;

public class Campfire : MonoBehaviour
{
    [SerializeField] private float radius = 5f;
    [SerializeField] private Transform player;
    [SerializeField] private Tilemap zoneTilemap;
    [SerializeField] private TileBase innerTile;
    [SerializeField] private TileBase middleTile;
    [SerializeField] private TileBase outerTile;
    [SerializeField][Range(0f, 1f)] private float innerPercent = 0.3f;
    [SerializeField][Range(0f, 1f)] private float middlePercent = 0.6f;

    void Start()
    {
        DrawZoneTiles();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= radius)
        {
            Debug.Log("Player is within campfire zone!");
        }
        else
        {
            Debug.Log("Player is outside campfire zone");
        }
    }

    public void DrawZoneTiles()
    {
        if (zoneTilemap == null || innerTile == null || middleTile == null || outerTile == null)
            return;

        zoneTilemap.ClearAllTiles();

        float innerRadius = radius * innerPercent;
        float middleRadius = radius * middlePercent;

        int intRadius = Mathf.CeilToInt(radius);

        for (int x = -intRadius; x <= intRadius; x++)
        {
            for (int y = -intRadius; y <= intRadius; y++)
            {
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
}