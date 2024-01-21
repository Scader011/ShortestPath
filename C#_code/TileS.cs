using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TileS : MonoBehaviour
{
    public static TileS StartTile = null;
    public static TileS EndTile = null;
    public Vector2Int GridPosition;
    public List<TileS> neighbors;
    public int gCost;
    public int hCost;
    public TileS parent;
    private SpriteRenderer spriteRenderer;
    /*public TextMeshProUGUI gCostText;
    public TextMeshProUGUI hCostText;
    public TextMeshProUGUI fCostText;
    */
    public bool isWalkable = true;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = isWalkable ? Color.white : Color.black;
        var blockedPositions = new List<Vector2Int>
    {
        new Vector2Int(1, 2),
        new Vector2Int(3, 4),
        // Add more positions as needed
    };

    }

    public int fCost
    {
        get { return gCost + hCost; }
    }

    void OnMouseDown()
    {
      
        if (StartTile == null)
        {
            StartTile = this;
            spriteRenderer.color = Color.green;
        }
        else if (EndTile == null && StartTile != this)
        {
            EndTile = this;
            spriteRenderer.color = Color.red;
            GridM.Instance.StartPathfinding();
        }
        else
        {
            ResetTiles();
            GridM.Instance.ResetGridAndPath();
        }
      
    }
    

    public static void ResetTiles()
    {
        if (StartTile != null)
        {
            StartTile.ResetColor();
            StartTile = null;
        }

        if (EndTile != null)
        {
            EndTile.ResetColor();
            EndTile = null;
        }
    }

    public void ResetColor()
    {
        // Only reset color if the tile is walkable; otherwise, it's a blocking tile and should remain black
        if (isWalkable)
        {
            spriteRenderer.color = Color.white;
        }
    }
    public void SetNeighbors()
    {
        neighbors = new List<TileS>();

        var directions = new Vector2Int[] {
            new Vector2Int(1, 0), new Vector2Int(-1, 0),
            new Vector2Int(0, 1), new Vector2Int(0, -1),
            new Vector2Int(1, 1), new Vector2Int(-1,1),
            new Vector2Int(1,-1), new Vector2Int(-1,-1),
        };

        foreach (var dir in directions)
        {
            var neighborPos = GridPosition + dir;
            if (GridM.Instance.IsPositionValid(neighborPos))
            {
                neighbors.Add(GridM.Instance.GetTileAtPosition(neighborPos));
            }
        }
    }
    /*public void UpdateCostText()
    {

         gCostText.text = gCost.ToString();
         hCostText.text = hCost.ToString();
         fCostText.text = fCost.ToString();
    }
    */
}



