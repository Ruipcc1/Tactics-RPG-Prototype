using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class TileXML
{
    [XmlAttribute("id")]
    public int id;

    [XmlAttribute("locX")]
    public int locX;

    [XmlAttribute("locY")]
    public int locY;
}

[XmlRoot("MapCollection")]

[System.Serializable]
public class MapXMLContainer
{
    [XmlAttribute("size")]
    public int size;

    [XmlArray("Tiles")]
    [XmlArrayItem("Tile")]
    public List<TileXML> tiles = new List<TileXML>();
}

[System.Serializable]
public static class MapSaveLoad
{
    public static MapXMLContainer CreateMapContainer(List<List<Tile>> map)
    {
        List<TileXML> tiles = new List<TileXML>();

        for (int i = 0; i < map.Count; i++)
        {
            for (int j = 0; j < map[i].Count; j++)
            {
                tiles.Add(MapSaveLoad.CreateTileXML(map[i][j]));
            }
        }
        return new MapXMLContainer()
        {
            size = map.Count,
            tiles = tiles
        };
    }

    public static TileXML CreateTileXML(Tile tile)
    {
        return new TileXML()
        {
            id = (int)tile.type,
            locX = (int)tile.gridPosition.x,
            locY = (int)tile.gridPosition.y
        };
    }

    public static void Save(MapXMLContainer mapContainer, string filename)
    {
        MapXMLContainer save = mapContainer;
        BinaryFormatter bf = new BinaryFormatter();
        var serializer = new XmlSerializer(typeof(MapXMLContainer));
        FileStream stream = File.Create(filename);
        bf.Serialize(stream, save);
        stream.Close();
    }

    public static MapXMLContainer Load(string filename)
    {
        BinaryFormatter bf = new BinaryFormatter();

        FileStream stream = new FileStream(filename, FileMode.Open);
        MapXMLContainer save = bf.Deserialize(stream) as MapXMLContainer;
        stream.Close();
        return save;
    }
}
