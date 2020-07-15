using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AIMovement : Player
{
    // Start is called before the first frame update
    void Start()
    {
        currentHP = 25;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHP <= 0)
        {
            transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
            transform.GetComponent<Renderer>().material.color = Color.red;
        }
    }

    public override void TurnUpdate()
    {
        transform.GetComponent<Renderer>().material.color = Color.green;
        if (positionQueue.Count > 0)
        {
            if (Vector3.Distance(positionQueue[0], transform.position) > 0.1f)
            {
                transform.position += (positionQueue[0] - transform.position).normalized * moveSpeed * Time.deltaTime;
                if (Vector3.Distance(positionQueue[0], transform.position) <= 0.1f)
                {
                    transform.position = positionQueue[0];
                    positionQueue.RemoveAt(0);
                    if (positionQueue.Count == 0)
                    {
                        actionPoints--;
                        Tile.movable = true;
                        GameManager.instance.highlightTilesAt(gridPosition, Color.blue, moveDistance, false);
                    }
                }
            }
        }
        else
        {
            //priority queue
            //attack if in range and with lowest HP
            List<Tile> attacktilesInRange = TileHighlight.FindHighlight(GameManager.instance.map[(int)gridPosition.x][(int)gridPosition.y], attackRange);
            //List<Tile> movementToAttackTilesInRange = TileHighlight.FindHighlight(GameManager.instance.map[(int)gridPosition.x][(int)gridPosition.y], movementPerActionPoint + attackRange);
            List<Tile> movementTilesInRange = TileHighlight.FindHighlight(GameManager.instance.map[(int)gridPosition.x][(int)gridPosition.y], moveDistance + 1000);
            //attack if in range and with lowest HP
            if (attacktilesInRange.Where(x => GameManager.instance.players.Where(y => y.GetType() != typeof(AIMovement) && y.currentHP > 0 && y != this && y.gridPosition == x.gridPosition).Count() > 0).Count() > 0)
            {
                var opponentsInRange = attacktilesInRange.Select(x => GameManager.instance.players.Where(y => y.GetType() != typeof(AIMovement) && y.currentHP > 0 && y != this && y.gridPosition == x.gridPosition).Count() > 0 ? GameManager.instance.players.Where(y => y.gridPosition == x.gridPosition).First() : null).ToList();
                Player opponent = opponentsInRange.OrderBy(x => x != null ? -x.currentHP : 1000).First();

                GameManager.instance.removeHighlights();
                moving = false;
                attacking = true;
                GameManager.instance.highlightTilesAt(gridPosition, Color.red, attackRange);

                GameManager.instance.attackWithCurrentPlayer(GameManager.instance.map[(int)opponent.gridPosition.x][(int)opponent.gridPosition.y]);
            }
            //move towards nearest opponent
            else if (movementTilesInRange.Where(x => GameManager.instance.players.Where(y => y.GetType() != typeof(AIMovement) && y.currentHP > 0 && y != this && y.gridPosition == x.gridPosition).Count() > 0).Count() > 0)
            {
                var opponentsInRange = movementTilesInRange.Select(x => GameManager.instance.players.Where(y => y.GetType() != typeof(AIMovement) && y.currentHP > 0 && y != this && y.gridPosition == x.gridPosition).Count() > 0 ? GameManager.instance.players.Where(y => y.gridPosition == x.gridPosition).First() : null).ToList();
                Player opponent = opponentsInRange.OrderBy(x => x != null ? -x.currentHP : 1000).ThenBy(x => x != null ? Pathfinder.FindPath(GameManager.instance.map[(int)gridPosition.x][(int)gridPosition.y], GameManager.instance.map[(int)x.gridPosition.x][(int)x.gridPosition.y]).Count() : 1000).First();

                GameManager.instance.removeHighlights();
                moving = true;
                attacking = false;
                GameManager.instance.highlightTilesAt(gridPosition, Color.blue, moveDistance, false);

                List<Tile> path = Pathfinder.FindPath(GameManager.instance.map[(int)gridPosition.x][(int)gridPosition.y], GameManager.instance.map[(int)opponent.gridPosition.x][(int)opponent.gridPosition.y], GameManager.instance.players.Where(x => x.gridPosition != gridPosition && x.gridPosition != opponent.gridPosition).Select(x => x.gridPosition).ToArray());
                if (path.Count() > 1)
                {
                    List<Tile> actualMovement = TileHighlight.FindHighlight(GameManager.instance.map[(int)gridPosition.x][(int)gridPosition.y], moveDistance, GameManager.instance.players.Where(x => x.gridPosition != gridPosition).Select(x => x.gridPosition).ToArray());
                    path.Reverse();
                    if (path.Where(x => actualMovement.Contains(x)).Count() > 0) GameManager.instance.moveCurrentPlayer(path.Where(x => actualMovement.Contains(x)).First());
                }
            }
        }
        base.TurnUpdate();
    }
    public override void TurnOnGUI()
    {

    }
}
