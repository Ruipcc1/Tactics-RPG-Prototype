using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    string currentMap;
    public static GameManager instance;

    public GameObject TilePrefab;
    public GameObject UserPlayerPrefab;
    public GameObject AiPlayerPrefab;

    public int mapSize = 11;

    public List<List<Tile>> map = new List<List<Tile>>();
    public List<Player> players = new List<Player>();
    public List<Tile> tiles = new List<Tile>();

    public int currentPlayerIndex = 0;

    public Text HPText;

    public Image characterPortrait;

    Transform mapTransform;

    // Start is called before the first frame update

    private void Awake()
    {
        instance = this;
        mapTransform = transform.Find("Map");
        generateMap();
        generatePlayers();
        HPText.text = "" + players[currentPlayerIndex].currentHP + "HP/" + players[currentPlayerIndex].HP + "HP";
    }

    // Update is called once per frame
    void Update()
    {
        if (players[currentPlayerIndex].currentHP <= 0)
        {
            players.Remove(players[currentPlayerIndex]);
            nextTurn();
        }
        players[currentPlayerIndex].TurnUpdate();
    }

    void OnGUI()
    {
        players[currentPlayerIndex].TurnOnGUI();
    }

    public void nextTurn()
    {
        removeHighlights();
        if (currentPlayerIndex + 1 < players.Count)
        {
            currentPlayerIndex++;
        }
        else
        {
            currentPlayerIndex = 0;
        }

        HPText.text = "" + players[currentPlayerIndex].currentHP + "HP/" + players[currentPlayerIndex].HP + "HP";
    }

    public void highlightTilesAt(Vector2 originLocation, Color highlightColor, int range, bool ignorePlayers = true)
    {
        List<Tile> highlightedTiles = new List<Tile>();
        if (ignorePlayers)
        {
            highlightedTiles = TileHighlight.FindHighlight(map[(int)originLocation.x][(int)originLocation.y], range);
        }
        else
        {
            highlightedTiles = TileHighlight.FindHighlight(map[(int)originLocation.x][(int)originLocation.y], range, players.Where(x => x.gridPosition != originLocation).Select(x => x.gridPosition).ToArray());
        }
        foreach (Tile t in highlightedTiles)
        {
            t.visual.transform.GetComponent<Renderer>().materials[0].color = highlightColor;
        }
    }

    public void removeHighlights()
    {
        for (int i =0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                if (!map[i][j].impassible) map[i][j].visual.transform.GetComponent<Renderer>().materials[0].color = Color.white;
            }
        }
    }

    public void moveCurrentPlayer(Tile destTile)
    {
        if (destTile.visual.transform.GetComponent<Renderer>().materials[0].color != Color.white && !destTile.impassible && players[currentPlayerIndex].positionQueue.Count == 0) {
            Tile.movable = false;
            removeHighlights();
            foreach (Tile t in Pathfinder.FindPath(map[(int)players[currentPlayerIndex].gridPosition.x][(int)players[currentPlayerIndex].gridPosition.y], destTile, players.Where(x => x.gridPosition != destTile.gridPosition && x.gridPosition != players[currentPlayerIndex].gridPosition).Select(x => x.gridPosition).ToArray()))
            {
                players[currentPlayerIndex].positionQueue.Add(map[(int)t.gridPosition.x][(int)t.gridPosition.y].transform.position + 1.5f * Vector3.up);
                Debug.Log("(" + players[currentPlayerIndex].positionQueue[players[currentPlayerIndex].positionQueue.Count - 1].x + "," + players[currentPlayerIndex].positionQueue[players[currentPlayerIndex].positionQueue.Count - 1].y + ")");
            }
            players[currentPlayerIndex].gridPosition = destTile.gridPosition;
            destTile.PlayerID = currentPlayerIndex;
        }
        else
        {
            Debug.Log("destination invalid");
        }
    }
    

    public void attackWithCurrentPlayer(Tile destTile)
    {
        if (destTile.visual.transform.GetComponent<Renderer>().materials[0].color != Color.white && !destTile.impassible)
        {
            Player target = null;
            foreach (Player p in players)
            {
                if (p.gridPosition == destTile.gridPosition)
                {
                    target = p;
                }
            }


            if (target != null)
            {
                if (players[currentPlayerIndex].gridPosition.x >= target.gridPosition.x - 1 && players[currentPlayerIndex].gridPosition.x <= target.gridPosition.x + 1 &&
                    players[currentPlayerIndex].gridPosition.y >= target.gridPosition.y - 1 && players[currentPlayerIndex].gridPosition.y <= target.gridPosition.y + 1)
                {
                    players[currentPlayerIndex].actionPoints--;
                    bool hit = Random.Range(0.0f, 1.0f) <= players[currentPlayerIndex].attackChance;
                    if (hit)
                    {
                        int amountofDamage = players[currentPlayerIndex].damageBase + Random.Range(0, players[currentPlayerIndex].damageRollSides);

                        target.currentHP -= amountofDamage;
                        Debug.Log(players[currentPlayerIndex].playerName + "hit" + target.playerName + "for" + amountofDamage);
                    }
                    else
                    {
                        Debug.Log(players[currentPlayerIndex].playerName + "missed" + target.playerName);
                    }
                }
                else
                {
                    Debug.Log("No Target");
                }
            }
            else
            {
                Debug.Log("destination invalid now");
            }
        }
    }

    void generateMap()
    {
        currentMap = "map.xml";
        loadMapFromXML(currentMap);
        /*for (int i = 0; i < mapSize; i++)
        {
            List<Tile> row = new List<Tile>();
            for (int j = 0; j < mapSize; j++)
            {
                Tile tile = GameObject.Instantiate(TilePrefab, new Vector3(i - Mathf.Floor(mapSize / 2), 0, -j + Mathf.Floor(mapSize / 2)), Quaternion.Euler(new Vector3())).GetComponent<Tile>();
                tile.gridPosition = new Vector2(i, j);
                tiles.Add(tile);
                row.Add(tile);
            }
            map.Add(row);
        }*/
    }

    void loadMapFromXML(string mapName)
    {
        MapXMLContainer container = MapSaveLoad.Load(mapName);
        mapSize = container.size;

        for (int i = 0; i < mapTransform.childCount; i++)
        {
            Destroy(mapTransform.GetChild(i).gameObject);
        }

        for (int i = 0; i < mapSize; i++)
        {
            List<Tile> row = new List<Tile>();
            for (int j = 0; j < mapSize; j++)
            {
                Tile tile = Instantiate(PrefabHolder.instance.TILE_BASE_PREFAB, new Vector3(i - Mathf.Floor(mapSize / 2), 0, -j + Mathf.Floor(mapSize / 2)), Quaternion.Euler(new Vector3())).GetComponent<Tile>();
                tile.transform.parent = mapTransform;
                tile.gridPosition = new Vector2(i, j);
                tile.setType((TileType)container.tiles.Where(x => x.locX == i && x.locY == j).First().id);
                row.Add(tile);
            }
            map.Add(row);
        }
    }

    void generatePlayers()
    {
        int playerCount = 0;
        PlayerMovement player;
        AIMovement aiplayer;
        Vector2 SpawnPoint;

        player = GameObject.Instantiate(UserPlayerPrefab, new Vector3(0 - Mathf.Floor(mapSize / 2), 1.5f, 0 + Mathf.Floor(mapSize / 2)), Quaternion.Euler(new Vector3())).GetComponent<PlayerMovement>();
        player.gridPosition = new Vector2(0, 0);
        player.playerName = "Heyward";
        players.Add(player);
        player.playerNumber = playerCount;
        playerCount++;

        player = GameObject.Instantiate(UserPlayerPrefab, new Vector3((mapSize - 1) - Mathf.Floor(mapSize / 2), 1.5f, -(mapSize - 1) + Mathf.Floor(mapSize / 2)), Quaternion.Euler(new Vector3())).GetComponent<PlayerMovement>();
        player.gridPosition = new Vector2(mapSize - 1, mapSize - 1);
        player.playerName = "Cory";
        players.Add(player);
        player.playerNumber = playerCount;
        playerCount++;

        player = GameObject.Instantiate(UserPlayerPrefab, new Vector3(4 - Mathf.Floor(mapSize / 2), 1.5f, -4 + Mathf.Floor(mapSize / 2)), Quaternion.Euler(new Vector3())).GetComponent<PlayerMovement>();
        player.gridPosition = new Vector2(4, 4);
        player.playerName = "Chas";
        players.Add(player);
        player.playerNumber = playerCount;
        playerCount++;

        aiplayer = GameObject.Instantiate(AiPlayerPrefab, new Vector3(5 - Mathf.Floor(mapSize / 2), 1.5f, -4 + Mathf.Floor(mapSize / 2)), Quaternion.Euler(new Vector3())).GetComponent<AIMovement>();
        aiplayer.gridPosition = new Vector2(5, 4);
        aiplayer.playerName = "Planshy";
        players.Add(aiplayer);
        aiplayer.playerNumber = playerCount;
        playerCount++;
    }
}

