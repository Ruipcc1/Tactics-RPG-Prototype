using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TileHighlight
{
    public TileHighlight()
    {

    }
    
    public static List<Tile> FindHighlight(Tile originTile, int movementPoints)
    {
        return FindHighlight(originTile, movementPoints, new Vector2[0], true);
    }
    public static List<Tile> FindHighlight(Tile originTile, int movementPoints, Vector2[] occupiedTile)
    {
        return FindHighlight(originTile, movementPoints, occupiedTile, false);
    }

    public static List<Tile> FindHighlight(Tile originTile, int movementPoints, Vector2[] occupiedTile, bool attacking)
    {
        List<Tile> closed = new List<Tile>();
        List<TilePath> open = new List<TilePath>();

        TilePath originPath = new TilePath();
        if (attacking)
        {
            originPath.addTileAttack(originTile);
        }
        else
        {
            originPath.addTile(originTile);
        }

        open.Add(originPath);

        while (open.Count > 0)
        {
            TilePath current = open[0];
            open.Remove(open[0]);

            if (closed.Contains(current.lastTile))
            {
                continue;
            }
            if (current.costOfPath > movementPoints + 1)
            {
                continue;
            }

            closed.Add(current.lastTile);

            foreach (Tile t in current.lastTile.neighbors)
            {
                if (t.impassible || occupiedTile.Contains(t.gridPosition)) continue;
                TilePath newTilePath = new TilePath(current);
                if (attacking)
                {
                    newTilePath.addTileAttack(t);
                    open.Add(newTilePath);
                }
                else
                {
                    newTilePath.addTile(t);
                    open.Add(newTilePath);
                }
            }
        }
        closed.Remove(originTile);
        closed.Distinct();
        return closed;
    }
}