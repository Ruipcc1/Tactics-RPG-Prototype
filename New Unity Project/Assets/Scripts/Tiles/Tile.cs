using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class Tile : MonoBehaviour
{
    GameObject TilePrefab;

    public GameObject visual;

    public TileType type = TileType.Normal;

    public Vector2 gridPosition = Vector2.zero;

    public int movementCost = 1;
    public bool impassible = false;

    public int PlayerID;
    public Player player;

    public static bool movable;



    public List<Tile> neighbors = new List<Tile>();

    // Start is called before the first frame update

    void Start()
    {
        movable = true;
        if (SceneManager.GetActiveScene().name == "Game Scene")
        {
            generateNeighbors();
            checkUnit();
        }
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

    }

    private void OnMouseEnter()
    {
        if (SceneManager.GetActiveScene().name == "Map Creator" && Input.GetMouseButton(0))
        {
            setType(MapManager.instance.palletSelection);
        }
        /*
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
        if (SceneManager.GetActiveScene().name == "Game Scene")
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
            if (GameManager.instance.players.Any(x => x.gridPosition == gridPosition))
            {
                Debug.Log("There's a unit here");
                GameManager.instance.HPText.text = "" + GameManager.instance.players[PlayerID].currentHP + "HP/" + GameManager.instance.players[PlayerID].HP + "HP";
            }
        } else if(SceneManager.GetActiveScene().name == "Map Creator")
        {
            setType(MapManager.instance.palletSelection);
        }
    }

    void checkUnit()
    {
        foreach (Player p in GameManager.instance.players)
        {
            if (p.gridPosition == gridPosition)
            {
                PlayerID = p.playerNumber;
            }
        }
    }

    public void setType(TileType t)
    {
        type = t;
        //definitions of TileType
        switch (t)
        {
            case TileType.Normal:
                movementCost = 1;
                impassible = false;
                TilePrefab = PrefabHolder.instance.TILE_NORMAL_PREFAB;
                break;

            case TileType.Bush:
                movementCost = 2;
                impassible = false;
                TilePrefab = PrefabHolder.instance.TILE_BUSH_PREFAB;
                break;

            case TileType.River:
                movementCost = 3;
                impassible = false;
                TilePrefab = PrefabHolder.instance.TILE_RIVER_PREFAB;
                break;

            case TileType.Wall:
                movementCost = 9;
                impassible = true;
                TilePrefab = PrefabHolder.instance.TILE_WALL_PREFAB;
                break;
        }
        generateVisuals();
    }
    public void generateVisuals()
    {
        GameObject container = transform.Find("Visuals").gameObject;

        //remove all children first
        for (int i = 0; i < container.transform.childCount; i++)
        {
            Destroy(container.transform.GetChild(i).gameObject);
        }

        GameObject newVisual = Instantiate(TilePrefab, transform.position, Quaternion.Euler(new Vector3(0,90,0)));
        newVisual.transform.parent = container.transform;

        visual = newVisual;
    }
}
