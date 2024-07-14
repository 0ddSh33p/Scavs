using System.Collections.Generic;
using UnityEngine;

public class seedGen : MonoBehaviour
{
    public Vector2 seed, perlinScale, macroScale, trueScale;
    public Vector2Int size;
    public List<GameObject> players;
    public float height, perlinPercent, macroHeight;

    [SerializeField] private int renderDistance;
    [SerializeField] private float POIsPer100Units, maxTilt;
    [SerializeField] private GameObject Chunk;
    [SerializeField] private List<GameObject> structures;

    [SerializeField] private LayerMask layerMask;

    private Vector2Int posOfPlayer;
    private Vector2Int[,] field;
    private int minX = 0, minY = 0;
    private int maxX = 0, maxY = 0;

    private void Awake()
    {
        seed = new Vector2(Random.Range(-99999, 99999), Random.Range(-99999, 99999));
        players = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
        field = new Vector2Int[1, 1];
    }

    private void Update()
    {
        foreach (GameObject player in players)
        {
            Vector3 playerPos = player.transform.position;
            Vector3 transPos = transform.position;

            int halfRenderDistance = renderDistance / 2;
            int startX = Mathf.FloorToInt((playerPos.x + transPos.x) / size.x) - halfRenderDistance;
            int startY = Mathf.FloorToInt((playerPos.z + transPos.z) / size.y) - halfRenderDistance;

            for (int r = 0; r <= renderDistance; r++)
            {
                for (int c = 0; c <= renderDistance; c++)
                {
                    posOfPlayer = new Vector2Int(startX + r, startY + c);

                    if (IsPositionFree(posOfPlayer))
                    {
                        Vector3 chunkPosition = new Vector3(posOfPlayer.x * size.x, 0, posOfPlayer.y * size.y);
                        Instantiate(Chunk, chunkPosition, Quaternion.identity);
                    }
                }
            }
        }
    }

    private bool IsPositionFree(Vector2Int vector)
    {
        if (vector.Equals(default(Vector2Int)))
        {
            return false;
        }

        if (vector.x >= minX && vector.x <= maxX && vector.y >= minY && vector.y <= maxY)
        {
            return field[vector.x - minX, vector.y - minY].Equals(default(Vector2Int));
        }

        return true;
    }

    public void PlaceVector2Int(Vector2Int vector)
    {
        if (vector.x < minX || vector.x > maxX || vector.y < minY || vector.y > maxY)
        {
            ResizeArray(vector);
        }

        field[vector.x - minX, vector.y - minY] = vector;
    }

    private void ResizeArray(Vector2Int vector)
    {
        int newMinX = Mathf.Min(minX, vector.x);
        int newMinY = Mathf.Min(minY, vector.y);
        int newMaxX = Mathf.Max(maxX, vector.x);
        int newMaxY = Mathf.Max(maxY, vector.y);

        int newWidth = newMaxX - newMinX + 1;
        int newHeight = newMaxY - newMinY + 1;

        Vector2Int[,] newArray = new Vector2Int[newWidth, newHeight];

        for (int y = 0; y <= maxY - minY; y++)
        {
            for (int x = 0; x <= maxX - minX; x++)
            {
                newArray[x + (minX - newMinX), y + (minY - newMinY)] = field[x, y];
            }
        }

        minX = newMinX;
        minY = newMinY;
        maxX = newMaxX;
        maxY = newMaxY;

        field = newArray;
    }

    public void addPOIs(Vector3 loc)
    {
        if (Random.Range(0f, 1f) < POIsPer100Units)
        {
            loc += new Vector3(Random.Range(-50, 50), loc.y + height + macroHeight + 1, Random.Range(-50, 50));

            if (Physics.Raycast(loc, Vector3.down, out RaycastHit hit, Mathf.Infinity, layerMask))
            {
                Vector3 hitPoint = hit.point;
                Quaternion rotation = Quaternion.Euler(Random.Range(-maxTilt, maxTilt), Random.Range(0, 360), Random.Range(-maxTilt, maxTilt));
                Instantiate(structures[Random.Range(0, structures.Count)], hitPoint, rotation);
            }
        }
    }
}
