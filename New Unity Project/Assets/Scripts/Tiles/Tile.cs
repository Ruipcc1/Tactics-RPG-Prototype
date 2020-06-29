using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2 gridPosition = Vector2.zero;

    public int movementCost = 1;
    public int range;

    public static bool movable;
    public bool impassible = false;

    public List<Tile> neighbors = new List<Tile>();

    // Start is called before the first frame update
    void Start()
    {
        movable = true;
        generateNeighbors();
    }

    void generateNeighbors()
    {
        //up
        if (gridPosition.y > 0)
        {
            Vector2 n = new Vector2(gridPosition.x, gridPosition.y - 1);
            neighbors.Add(GameManager.instance.map[(int)Mathf.Round(n.x)][(int)Mathf.Round(n.y)]);
        }
        //down
        if (gridPosition.y < GameManager.instance.mapSize - 1)
        {
            Vector2 n = new Vector2(gridPosition.x, gridPosition.y + 1);
            neighbors.Add(GameManager.instance.map[(int)Mathf.Round(n.x)][(int)Mathf.Round(n.y)]);
        }

        //left
        if (gridPosition.x > 0)
        {
            Vector2 n = new Vector2(gridPosition.x - 1, gridPosition.y);
            neighbors.Add(GameManager.instance.map[(int)Mathf.Round(n.x)][(int)Mathf.Round(n.y)]);
        }
        //right
        if (gridPosition.x < GameManager.instance.mapSize - 1)
        {
            Vector2 n = new Vector2(gridPosition.x + 1, gridPosition.y);
            neighbors.Add(GameManager.instance.map[(int)Mathf.Round(n.x)][(int)Mathf.Round(n.y)]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (impassible == true)
        {
            transform.GetComponent<Renderer>().material.color = Color.grey;
        }
    }

    private void OnMouseEnter()
    {/*
        if (GameManager.instance.players[GameManager.instance.currentPlayerIndex].moving)
        {
            transform.GetComponent<Renderer>().material.color = Color.blue;
        } else if (GameManager.instance.players[GameManager.instance.currentPlayerIndex].attacking)
        {
            transform.GetComponent<Renderer>().material.color = Color.red;
        }
        */
    }
    private void OnMouseExit()
    {
        //transform.GetComponent<Renderer>().material.color = Color.white;
    }

    private void OnMouseDown()
    {
        if (movable)
        {
            if (GameManager.instance.players[GameManager.instance.currentPlayerIndex].moving)
            {
                GameManager.instance.moveCurrentPlayer(this);
            }
            else if (GameManager.instance.players[GameManager.instance.currentPlayerIndex].attacking)
            {
                GameManager.instance.attackWithCurrentPlayer(this);
            }
        }
    }
}
