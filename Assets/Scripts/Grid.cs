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

public class Grid : MonoBehaviour
{
  private int width = 3;
  private int height = 3;
  private float tileSize = 1;

  private Vector3Int id_click = new Vector3Int(-1, -1, -1);
  private Vector3 startPos;
  public Tilemap tilemap;
  public Tile tile1;

  // Start is called before the first frame update
  void Start()
  {
    for (int x = 0; x < width; x++)
    {
      for (int y = 0; y < height; y++)
      {
        tilemap.SetTile(new Vector3Int(x, y, 0), tile1);
      };
    };
  }

  // Update is called once per frame
  void Update()
  {
    if (Input.GetMouseButtonDown(0))
    {
      startPos = Input.mousePosition;
      Vector3 pos = Camera.main.ScreenToWorldPoint(startPos);
      //Debug.Log(pos);
      id_click = GetIdCell(pos);
      //Debug.Log(id_click);
      if (tilemap.GetColor(id_click) == Color.white)
      {
        SetTileColour(Color.red, id_click, tilemap);
      }
    }
  }

  private void SetTileColour(Color colour, Vector3Int position, Tilemap tilemap)
  {
    // Flag the tile, inidicating that it can change colour.
    // By default it's set to "Lock Colour".
    tilemap.SetTileFlags(position, TileFlags.None);

    // Set the colour.
    tilemap.SetColor(position, colour);
  }

  Vector3Int GetIdCell(Vector3 pos)
  {
    Vector3Int cellPosition = tilemap.WorldToCell(pos);

    if (cellPosition.x >= width || cellPosition.y >= height || cellPosition.x < 0 || cellPosition.y < 0)
    {
      cellPosition = new Vector3Int(-1, -1, -1);
    }

    return cellPosition;
  }
}
