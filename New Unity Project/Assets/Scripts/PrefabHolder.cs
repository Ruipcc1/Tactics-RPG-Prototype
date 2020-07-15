using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabHolder : MonoBehaviour
{
    public static PrefabHolder instance;

    public GameObject TILE_BASE_PREFAB;

    public GameObject TILE_NORMAL_PREFAB;
    public GameObject TILE_BUSH_PREFAB;
    public GameObject TILE_RIVER_PREFAB;
    public GameObject TILE_WALL_PREFAB;


    void Awake()
    {
        instance = this;
    }
}
