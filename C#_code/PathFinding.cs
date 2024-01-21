using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public List<TileS> FindPath(TileS startTile, TileS targetTile)
    {
        // Initialize the start tile's G and H costs
        startTile.gCost = 0;
        startTile.hCost = CalculateDistance(startTile, targetTile);

        List<TileS> openSet = new List<TileS>();
        HashSet<TileS> closedSet = new HashSet<TileS>();
        openSet.Add(startTile);

        while (openSet.Count > 0)
        {
            TileS currentTile = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                // Check if this tile has a lower F cost, or if it has an equal F cost and a lower H cost
                if (openSet[i].fCost < currentTile.fCost ||
                   (openSet[i].fCost == currentTile.fCost && openSet[i].hCost < currentTile.hCost))
                {
                    currentTile = openSet[i];
                }
            }

            openSet.Remove(currentTile);
            closedSet.Add(currentTile);

            if (currentTile == targetTile)
            {
                return RetracePath(startTile, targetTile);
            }

            foreach (TileS neighbor in currentTile.neighbors)
            {
                if (!neighbor.isWalkable || closedSet.Contains(neighbor))
                {
                    continue; // Skip this neighbor if it's not walkable or already processed
                }

                int tentativeGCost = currentTile.gCost + CalculateDistance(currentTile, neighbor);

                // If this path to neighbor is better, or if the neighbor is not in openSet
                if (tentativeGCost < neighbor.gCost || !openSet.Contains(neighbor))
                {

                    neighbor.gCost = tentativeGCost;
                    neighbor.hCost = CalculateDistance(neighbor, targetTile);
                    neighbor.parent = currentTile;

                    // Update the cost display text on the tile
                   //neighbor.UpdateCostText();

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }

        // If you've exited the while loop without returning a path, then there is no path
        return new List<TileS>();
    }

    static List<TileS> RetracePath(TileS startTile, TileS endTile)
    {
        List<TileS> path = new List<TileS>();
        TileS currentTile = endTile;

        while (currentTile != startTile)
        {
            path.Add(currentTile);
            currentTile = currentTile.parent; // Set the currentTile to its parent
        }
        path.Add(startTile);
        path.Reverse();
        return path;
    }
    private int CalculateDistance(TileS tileA, TileS tileB)
    {
        int distX = Mathf.Abs(tileA.GridPosition.x - tileB.GridPosition.x);
        int distY = Mathf.Abs(tileA.GridPosition.y - tileB.GridPosition.y);
        return distX + distY; // Manhattan distance
    }

}

