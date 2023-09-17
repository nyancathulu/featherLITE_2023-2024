using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
//using UnityEditor.Tilemaps;

public class RoomBuildingScript : MonoBehaviour
{

    public List<Tilemap> Tilemaps = new List<Tilemap>();

    [SerializeField] float taketile_Time;


    [SerializeField] Transform RedtemplateOrigin;
    [SerializeField] Transform BluetemplateOrigin;

    [SerializeField] int number_of_rooms;
    [SerializeField] int room_size_Y;
    [SerializeField] int room_size_X;


    int number_of_real_rooms;
    [SerializeField] int Map_Size_Y;
    [SerializeField] int Map_Size_X;

    List<FilledTile> Master_Filled_Tiles;
    
    void Start()
    {
        //Debug.Log(tilemap.origin);

        /*        BoundsInt bounds = tilemap.cellBounds;
                TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

                for (int x = 0; x < bounds.size.x; x++)
                {
                    for (int y = 0; y < bounds.size.y; y++)
                    {
                        TileBase tile = allTiles[x + y * bounds.size.x];
                        if (tile != null)
                        {
                            Debug.Log("x:" + x + " y:" + y + " tile:" + tile.name);
                        }
                        else
                        {
                            Debug.Log("x:" + x + " y:" + y + " tile: (null)");
                        }
                    }
                }*/
        number_of_real_rooms = Mathf.FloorToInt(Map_Size_Y / room_size_Y);
    }


    public void BuildMaps()
    {
        BuildRedMap();
        BuildBlueMap();
        Master_Filled_Tiles = SaveFilledTiles();
        StartCoroutine(TakeTiles());
    }



    void BuildRedMap()
    {
        for (int i = 0; i < number_of_real_rooms; i++)
        {
            int randomRoomNumber = Random.Range(1, number_of_rooms + 1);
            Vector2Int roomOrigin = new Vector2Int((int)RedtemplateOrigin.position.x, (int)RedtemplateOrigin.position.y - (room_size_Y * randomRoomNumber));
            for (int k = 0; k < Tilemaps.Count; k++)
            {
                Tilemap current_tilemap = Tilemaps[k];
                TileBase[,] roomTileArray = new TileBase[room_size_X, room_size_Y];
                for (int y = 0; y < room_size_Y; y++)
                {
                    for (int x = 0; x < room_size_X; x++)
                    {
                        roomTileArray[x, y] = current_tilemap.GetTile(new Vector3Int(roomOrigin.x + x, roomOrigin.y + y, 0));
                    }
                }

                for (int y = 0; y < room_size_Y; y++)
                {
                    for (int x = 0; x < room_size_X; x++)
                    {
                        current_tilemap.SetTile(new Vector3Int(-32 + x, (-24 + 12 * i) + y), roomTileArray[x, y]);
                    }
                }
            }

        }
    }

    void BuildBlueMap()
    {
        for (int i = 0; i < number_of_real_rooms; i++)
        {
            int randomRoomNumber = Random.Range(1, number_of_rooms + 1);
            Vector2Int roomOrigin = new Vector2Int((int)BluetemplateOrigin.position.x, (int)BluetemplateOrigin.position.y - (room_size_Y * randomRoomNumber));
            for (int k = 0; k < Tilemaps.Count; k++)
            {
                Tilemap current_tilemap = Tilemaps[k];
                TileBase[,] roomTileArray = new TileBase[room_size_X, room_size_Y];
                for (int y = 0; y < room_size_Y; y++)
                {
                    for (int x = 0; x < room_size_X; x++)
                    {
                        roomTileArray[x, y] = current_tilemap.GetTile(new Vector3Int(roomOrigin.x + x, roomOrigin.y + y, 0));
                    }
                }

                for (int y = 0; y < room_size_Y; y++)
                {
                    for (int x = 0; x < room_size_X; x++)
                    {
                        current_tilemap.SetTile(new Vector3Int(x, (-24 + 12 * i) + y), roomTileArray[x, y]);
                    }
                }
            }

        }
    }

    List<FilledTile> SaveFilledTiles()
    {
        List<FilledTile> Filled_List = new List<FilledTile>();

        for (int k = 0; k < Tilemaps.Count; k++)
        {
            Tilemap current_tilemap = Tilemaps[k];
            BoundsInt bounds = current_tilemap.cellBounds;
            TileBase[] allTiles = current_tilemap.GetTilesBlock(bounds);
            for (int x = 0; x < bounds.size.x; x++)
            {
                for (int y = 0; y < bounds.size.y; y++)
                {
                    // get the tile at the current position
                    TileBase tile = allTiles[x + y * bounds.size.x];

                    // if the tile is not null, add its position to the list
                    if (tile != null)
                    {
                        Vector3Int position = new Vector3Int(x + bounds.xMin, y + bounds.yMin, 0);

                        Filled_List.Add(new FilledTile(position, current_tilemap));
                    }
                }
            }
        }

        return Filled_List;
    }



    IEnumerator TakeTiles()
    {
        while (true)
        {
            float timer = taketile_Time;

            while (timer > 0)
            {
                timer -= Time.deltaTime;
                yield return 0;
            }

            FilledTile tile_to_remove = Master_Filled_Tiles[Random.Range(0, Master_Filled_Tiles.Count)];
            tile_to_remove.tilemap.SetTile(tile_to_remove.position, null);
        }


    }












    public class FilledTile
    {
        public Vector3Int position;
        public Tilemap tilemap;

        public FilledTile(Vector3Int pos, Tilemap map)
        {
            position = pos;
            tilemap = map;
        }
    }
}
