using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
using UnityEngine.SceneManagement;
using System.Runtime.Versioning;
using UnityEngine.Tilemaps; 

public class MainGame : MonoBehaviour
{
  private readonly int width = 10;
  private readonly int height = 10;
  
  private Vector3 startPos;

  public Tilemap mapClick;

  public Tile tileBasic;
  public GameObject tileNumber;

  // Start is called before the first frame update
  void Start()
  {
    TextMesh number = tileNumber.GetComponentInChildren<TextMesh>();
    number.text = "1";
    tileBasic.gameObject = tileNumber;
    for (int x = 0; x < width; x++)
    {
      for (int y = 0; y < height; y++)
      {
        mapClick.SetTile(new Vector3Int(x, y, 0), tileBasic);
      };
    };
  }

  // Update should just listen to mouse clicks
  void Update()
  {
    if (Input.GetMouseButtonDown(0) && !Input.GetMouseButtonUp(0))
    {
      startPos = Input.mousePosition;
      Vector3 pos = Camera.main.ScreenToWorldPoint(startPos);
      //Debug.Log(pos);
      Vector3Int idClick = GetIdCell(pos);
      //Debug.Log(id_click);
      if (mapClick.GetColor(idClick) == Color.white)
      {
        SetTileColour(Color.red, idClick);
      }else
      {
        SetTileColour(Color.white, idClick);
      }
    }
  }

  //Basic functions
  Vector3Int GetIdCell(Vector3 pos)
  {
    // Return the tile that is clicked
    Vector3Int cellPosition = mapClick.WorldToCell(pos);

    // Make sure its in the game screen but also exclude the left coloumn and top row from selection
    if (cellPosition.x >= width || cellPosition.y >= height-1 || cellPosition.x < 1 || cellPosition.y < 0)
    {
      cellPosition = new Vector3Int(-1, -1, -1);
    }

    return cellPosition;
  }

  //Manipulate TileMaps

  //ClickMap 
  private void SetTileColour(Color colour, Vector3Int position)
  {
    // Flag the tile, inidicating that it can change colour.
    mapClick.SetTileFlags(position, TileFlags.None);

    // Set the colour.
    mapClick.SetColor(position, colour);
  }

  //NumberMap
}
