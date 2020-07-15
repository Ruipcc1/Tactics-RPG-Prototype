using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;
    public int mapSize;

    public GameObject Tile;

    string SavingMap;


    public List<List<Tile>> map = new List<List<Tile>>();

    public TileType palletSelection = TileType.Normal;

    Transform mapTransform;

    private void Awake()
    {
        instance = this;
        mapTransform = transform.Find("Map");
    }
    private void Start()
    {
        generateBlankMap(11);
    }
    void generateBlankMap(int mSize)
    {
        mapSize = mSize;

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
                tile.setType(TileType.Bush);
                row.Add(tile);
            }
            map.Add(row);
        }
    }

    private void OnGUI()
    {
        Rect rect = new Rect(10, Screen.height - 80, 100, 60);
        if(GUI.Button(rect, "Normal"))
        {
            palletSelection = TileType.Normal;
        }

        rect = new Rect(10 + (100 + 10) * 1, Screen.height - 80, 100, 60);
        if (GUI.Button(rect, "Bush"))
        {
            palletSelection = TileType.Bush;
        }

        rect = new Rect(10 + (100 + 10) * 2, Screen.height - 80, 100, 60);
        if (GUI.Button(rect, "River"))
        {
            palletSelection = TileType.River;
        }

        rect = new Rect(10 + (100 + 10) * 3, Screen.height - 80, 100, 60);
        if (GUI.Button(rect, "Wall"))
        {
            palletSelection = TileType.Wall;
        }

        //IO
        rect = new Rect(Screen.width - (10 + (100 + 10) *1), Screen.height - 80, 100, 60);
        if (GUI.Button(rect, "Load Map"))
        {
            loadMapFromXML(SavingMap);
        }

        rect = new Rect(Screen.width - (10 + (100 + 10) * 2), Screen.height - 80, 100, 60);
        if (GUI.Button(rect, "Save Map"))
        {
            saveMapFromXML();
        }

        rect = new Rect(Screen.width - (10 + (100 + 10) * 3), Screen.height - 80, 100, 60);
        if (GUI.Button(rect, "Clear"))
        {
            generateBlankMap(mapSize);
        }
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
                tile.setType((TileType)container.tiles.Where(x => x.locX ==i && x.locY == j).First().id);
                row.Add(tile);
            }
            map.Add(row);
        }
    }

    void saveMapFromXML()
    {
        MapSaveLoad.Save(MapSaveLoad.CreateMapContainer(map), "map2");
        SavingMap = "map2";
    }
}
