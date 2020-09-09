using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Tile = UnityEngine.Tilemaps.Tile;

public class MainGame : MonoBehaviour
{
  [Serializable]
  private class Level
  {
    public int width;
    public int height;
    public int[] tileType;
    public int[] tileCorrect;
    public int[] tileSelected;
    public string name;
  }

  //Create the a global level that will be load the json file into
  Level level = new Level();

  // Main game Tilemap
  public Tilemap mapClick;

  // Tilemap tiles objects
  public GameObject tileNumber;
  public Tile tileBasic;

  // Start is called before the first frame update
  void Start()
  {
    // Read the json file into a local string 
    string json = File.ReadAllText(Application.dataPath + "/Scripts/Map1.json");
    //Debug.Log(json);

    level = JsonUtility.FromJson<Level>(json);
    //Debug.Log(level.tileArray[0]);
    level.tileCorrect = new int[level.width * level.height];
    level.tileSelected = new int[level.width * level.height];

    // Simple loop for the coordinates
    for (int x = 0; x < level.width; x++)
    {
      for (int y = 0; y < level.height; y++)
      {
        // The correct x and y index in the array is the height time the max width plus the width since for everything max width you will get a new row
        int levelCell = y * level.width + x;

        // Create a new asset inside the game to break its reference to the origin
        tileBasic.gameObject = Instantiate(tileNumber, transform);

        // Prefab can now be instanced without changing the other tiles prefabs
        TextMesh number = tileBasic.gameObject.GetComponentInChildren<TextMesh>();

        // Simple logic since 0 is default in array
        if (level.tileType[levelCell] != 0)
        {
          number.text = level.tileType[levelCell].ToString();
        }

        // This is another instantiate but all the changes need be in place before creation
        mapClick.SetTile(new Vector3Int(x, y, 0), tileBasic);

        // Destroy the original object after the launch
        Destroy(tileBasic.gameObject, 1);
      };
    };
  }

  // Update will just listen to mouse clicks
  void Update()
  {
    // Is the mouse button down and not up?
    if (Input.GetMouseButtonDown(0) && !Input.GetMouseButtonUp(0))
    {
      // Mouse position to camera position
      Vector3 pos = Input.mousePosition;
      pos = Camera.main.ScreenToWorldPoint(pos);

      // Return tileMap x and y (x, y, z) based on the clicked tile position
      Vector3Int idClick = GetIdCell(pos);
      Debug.Log(idClick);

      //level.tileSelected[idClick.y * level.width + idClick.x] = 1;

      // Set color to red if tile is white else color is white
      if (mapClick.GetColor(idClick) == Color.white)
      {
        SetTileColour(Color.red, idClick);
      }else
      {
        SetTileColour(Color.white, idClick);
      }
    }



    string json = JsonUtility.ToJson(level);
    //Debug.Log(json);

    File.WriteAllText(Application.dataPath + "/Scripts/Map1.json", json);
  }

  // Basic functions
  Vector3Int GetIdCell(Vector3 pos)
  {
    // Return the tile that is clicked
    Vector3Int cellPosition = mapClick.WorldToCell(pos);

    // Make sure its in the game screen but also exclude the left coloumn and top row from selection
    if (cellPosition.x >= level.width || cellPosition.y >= level.height - 1 || cellPosition.x < 1 || cellPosition.y < 0)
    {
      cellPosition = new Vector3Int(-1, -1, -1);
    }

    return cellPosition;
  }



  // Manipulate TileMaps 
  private void SetTileColour(Color colour, Vector3Int position)
  {
    // Flag the tile, inidicating that it can change colour.
    mapClick.SetTileFlags(position, TileFlags.None);

    // Set the colour.
    mapClick.SetColor(position, colour);
  }
}