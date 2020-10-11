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

  public Camera cameraMain;

  public Canvas dialogBox;

  // Create the a global level that will be load the json file into
  Level level = new Level();

  // Main game Tilemap
  public Tilemap mapClick;

  // Tilemap tiles objects
  public GameObject tileNumber;
  public Tile tileBasic;
  public Sprite square1;
  public Sprite square2;
  public Sprite square3;


  // Start is called before the first frame update
  void Start()
  {
   b  // Read the json file into a local string 
    string json = File.ReadAllText(UnityEngine.Application.dataPath + "/Scripts/Maps/Map1.json");
    // Debug.Log(json);

    // Build json string into an object
    level = JsonUtility.FromJson<Level>(json);

    // Sets the main camera  to the same size at the level height
    cameraMain.orthographicSize = level.height / 2;
    //view.orthographicSize = 20;

    // Loop for the coordinates
    for (int y = 0; y < level.height; y++)
    {
      for (int x = 0; x < level.width; x++)
      {
        // The correct x and y index in the array is the height time the max width plus the width since for everything max width you will get a new row
        int levelCell = y * level.width + x;

        // Create a new asset inside the game to break its reference to the origin
        tileBasic.gameObject = Instantiate(tileNumber, new Vector3(0, 0, -20), Quaternion.identity);

        // Prefab can now be instanced without changing the other tiles prefabs
        TextMesh number = tileBasic.gameObject.GetComponentInChildren<TextMesh>();

        // Logic for tile type 1
        if (level.tileType[levelCell] == 1)
        {
          int tileNumberCorrect = 0;
          for (int xmax = 0; xmax < level.width; xmax++)
          {
            int logic = levelCell + xmax;
            if (0 < logic && logic < level.width * level.height)
            {
              //Debug.Log(logic);
              if (level.tileCorrect[logic] == 1) { tileNumberCorrect++; }
            }
          }
          for (int ymax = 0; ymax < level.height; ymax++)
          {
            int logic = levelCell - ymax * level.width;
            if (0 < logic && logic <=  level.width * level.height)
            {
              //Debug.Log(logic);
              if (level.tileCorrect[logic] == 1) { tileNumberCorrect++; }
            }
          }
          number.text = tileNumberCorrect.ToString();
        }

        // Logic for tile type 2
        if (level.tileType[levelCell] == 2)
        {
          int tileNumberCorrect = 0;
          for (int xdif = -1; xdif < 2; xdif++)
          {
            for (int ydif = -1; ydif < 2; ydif++)
            {
              int logic = (y + ydif) * level.width + x + xdif;
              if (0 < logic && logic < level.width * level.height) {
                if (level.tileCorrect[logic] == 1) { tileNumberCorrect++; }
              }
            }
          }
          number.text = tileNumberCorrect.ToString();
        }

        // Set tile sprite setting based of the first column and last row
        if (x == 0 && y != level.height - 1) 
        {
          tileBasic.sprite = square2;
        }else if(y == level.height-1 && x != 0)
        {
          tileBasic.sprite = square3;
        }else
        {
          tileBasic.sprite = square1;
        }

        // Centre the tiles rather than using the top right quadrant
        int xCentre = x - level.width / 2;
        int yCentre = y - level.height / 2;

        // This is another instantiate but all the changes need be in place before its creation
        mapClick.SetTile(new Vector3Int(xCentre, yCentre, 0), tileBasic);
        
        // Checking level for a currently selected tile, will be used in save files
        if (level.tileSelected[levelCell] == 1)
        {
          SetTileColour(Color.red, new Vector3Int(xCentre, yCentre, 0));
        }

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
      Vector3 mouse_pos = Input.mousePosition;
      mouse_pos = Camera.main.ScreenToWorldPoint(mouse_pos);

      // Return tileMap x and y (x, y, z) based on the clicked tile position
      Vector3Int tileVector_int = TileVector_get(mouse_pos);
      //Debug.Log(idClick);

      // Set color to red if tile is white else color is white
      if (mapClick.GetColor(tileVector_int) == Color.white)
      {
        SetTileColour(Color.red, tileVector_int);
        level.tileSelected[LevelArrayIndex(tileVector_int)] = 1;
      }
      else
      {
        SetTileColour(Color.white, tileVector_int);
        level.tileSelected[LevelArrayIndex(tileVector_int)] = 0;
      }
    }

    // Check if you Won by comparing correct tiles with selected tiles, else save the game
    if(level.tileSelected.SequenceEqual(level.tileCorrect)) 
    {
      //Debug.Log("Won");
      dialogBox.gameObject.SetActive(true);
    }else
    { 
      string json = JsonUtility.ToJson(level);
      File.WriteAllText(UnityEngine.Application.dataPath + "/Scripts/Maps/Map1.json", json);
      //Debug.Log("Not Won");
    }
  }

  // Basic functions
  Vector3Int TileVector_get(Vector3 levelPos)
  {
    // Return the tile that is clicked
    Vector3Int tilePos = mapClick.WorldToCell(levelPos);
    
    // Make sure its in the game screen but also exclude the left coloumn and top row from selection, it is adjusted centering it like a graph
    if (tilePos.x >= level.width / 2 || tilePos.y >= level.height / 2 - 1 || tilePos.x < -level.width / 2 + 1 || tilePos.y < -level.height / 2)
    {
      // An unreachable tile
      tilePos = new Vector3Int(0, 0, -1);
    }

    // Return it as an int vector, this is needed to change colors
    return tilePos;
  }

  int LevelArrayIndex(Vector3 tilePos)
  {
    // Turning all negative tile positions into positive
    tilePos.x += level.width / 2;
    tilePos.y = (tilePos.y + level.height / 2) * level.width;

    int levelIndex = (int)(tilePos.x + tilePos.y);

    // Returning the index of the level
    return levelIndex;
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