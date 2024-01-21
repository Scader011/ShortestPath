using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class GridM : MonoBehaviour
{
    public List<Vector2Int> blockedPositions;
    public static GridM Instance;
    public Pathfinding Pathfinding;
    private List<TileS> currentPath;
    private int currentPathIndex = 0;
    public int width = 10;
    public int height = 10;
    public GameObject tilePrefab;
    public float tileSize = 1.0f;
    private TileS[,] grid;

    private void Awake()
    {
        Instance = this;
        grid = new TileS[width, height];
        Pathfinding = GetComponent<Pathfinding>();
    }

    void Start()
    {
        GenerateGrid();
        DefineBlockedTiles();
        SetAllNeighbors();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) {
            ShowNextTileInPath();
        }
       
    }
    void GenerateGrid()
    {
        grid = new TileS[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject newTileGO = Instantiate(tilePrefab, new Vector3(x * tileSize, y * tileSize, 0), Quaternion.identity);
                newTileGO.transform.parent = this.transform;
                TileS newTile = newTileGO.GetComponent<TileS>();
                newTile.GridPosition = new Vector2Int(x, y);
                grid[x, y] = newTile;
            }
        }
    }
    private void DefineBlockedTiles()
    {
        // Example of predefined blocked tiles

        foreach (var position in blockedPositions)
        {
            TileS tile = GetTileAtPosition(position);
            if (tile != null)
            {
                tile.isWalkable = false;
                tile.GetComponent<SpriteRenderer>().color = Color.black;  // Color blocked tiles black
            }
        }
    }

    public TileS GetTileAtPosition(Vector2Int position)
    {
        if (position.x >= 0 && position.x < grid.GetLength(0) &&
            position.y >= 0 && position.y < grid.GetLength(1))
        {
            return grid[position.x, position.y];
        }
        else
        {
            return null; // Return null if the position is out of bounds
        }
    }
    void SetAllNeighbors()
    {
        foreach (TileS tile in grid)
        {
            tile.SetNeighbors();
        }
    }

    public bool IsPositionValid(Vector2Int position)
    {
        return position.x >= 0 && position.x < width && position.y >= 0 && position.y < height;
    }

    public void StartPathfinding()
    {
        if (TileS.StartTile != null && TileS.EndTile != null)
        {
            currentPath = Pathfinding.FindPath(TileS.StartTile, TileS.EndTile);
            currentPathIndex = 0; // Reset the index
        }
        else
        {
            Debug.LogError("Start or End tile is null.");
        }
    }
    public void ResetGridAndPath()
    {
        // Reset the colors of all tiles
        foreach (TileS tile in grid)
        {
            if (tile != null)
            {
                tile.ResetColor(); // Make sure the ResetColor method exists in TileS
            }
        }

        // Reset the start and end tiles
        TileS.ResetTiles();
    }
    public void ShowNextTileInPath()
    {
        // Check if we are before the last tile in the path
        if (currentPath != null && currentPathIndex < currentPath.Count - 2)
        {
            // Get the next tile to show
            TileS tileToShow = currentPath[currentPathIndex+1];
            if (tileToShow != null)
            {
                // Color the tile
                tileToShow.GetComponent<SpriteRenderer>().color = Color.cyan; // Example color
            }

            // Increment the index for the next call
            currentPathIndex++;
        }
    }

}

