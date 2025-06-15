using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed class ProcGen : MonoBehaviour
{
    public void GenerateDungeon(int mapWidth, int mapHeight, int roomMaxSize, int roomMinSize, int maxRooms,
        List<RectangularRoom> rooms)
    {
        for (int roomNum = 0; roomNum < maxRooms; roomNum++)
        {
            int roomWidth = Random.Range(roomMinSize, roomMaxSize);
            int roomHeight = Random.Range(roomMinSize, roomMaxSize);
            
            int roomX = Random.Range(0, mapWidth - roomWidth - 1);
            int roomY = Random.Range(0, mapHeight - roomHeight - 1);
            
            RectangularRoom newRoom = new RectangularRoom(roomX, roomY, roomWidth, roomHeight);

            if (newRoom.Overlaps(rooms))
            {
                continue;
            }

            for (int x = roomX; x < roomX + roomWidth; x++)
            {
                for (int y = roomY; y < roomY + roomHeight; y++)
                {
                    if (x == roomX || x == roomX + roomWidth - 1 || y == roomY || y == roomY + roomHeight - 1)
                    {
                        if (SetWallTileIfEmpty(new Vector3Int(x, y, 0)))
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (MapManager.Instance.ObstacleMap.GetTile(new Vector3Int(x, y, 0)))
                        {
                            MapManager.Instance.ObstacleMap.SetTile(new Vector3Int(x, y, 0), null);
                        }
                        MapManager.Instance.FloorMap.SetTile(new Vector3Int(x, y, 0), MapManager.Instance.FloorTile);
                    }
                }
            }

            if (MapManager.Instance.Rooms.Count == 0)
            {
                MapManager.Instance.CreatePlayer(newRoom.Center());
            }
            else
            {
                TunnelBetween(MapManager.Instance.Rooms[MapManager.Instance.Rooms.Count - 1], newRoom);
            }
            
            rooms.Add(newRoom);
        }
    }

    private void TunnelBetween(RectangularRoom oldRoom, RectangularRoom newRoom)
    {
        Vector2Int oldRoomCenter = oldRoom.Center();
        Vector2Int newRoomCenter = newRoom.Center();
        Vector2Int tunnelCorner;

        if (Random.value < 0.5f)
        {
            tunnelCorner = new Vector2Int(newRoomCenter.x, oldRoomCenter.y);
        }
        else
        {
            tunnelCorner = new Vector2Int(oldRoomCenter.x, newRoomCenter.y);
        }
        
        List<Vector2Int> tunnelCoords = new List<Vector2Int>();
        BresenhamLine(oldRoomCenter,tunnelCorner,tunnelCoords);
        BresenhamLine(tunnelCorner,newRoomCenter,tunnelCoords);

        for (int i = 0; i < tunnelCoords.Count; i++)
        {
            if (MapManager.Instance.ObstacleMap.HasTile(new Vector3Int(tunnelCoords[i].x, tunnelCoords[i].y, 0)))
            {
                MapManager.Instance.ObstacleMap.SetTile(new Vector3Int(tunnelCoords[i].x, tunnelCoords[i].y, 0), null);
            }
            
            MapManager.Instance.FloorMap.SetTile(new Vector3Int(tunnelCoords[i].x, tunnelCoords[i].y, 0), MapManager.Instance.FloorTile);

            for (int x = tunnelCoords[i].x - 1; x <= tunnelCoords[i].x + 1; x++)
            {
                for (int y = tunnelCoords[i].y - 1; y <= tunnelCoords[i].y + 1; y++)
                {
                    if (SetWallTileIfEmpty(new Vector3Int(x, y, 0)))
                    {
                        continue;
                    }
                }
            }
        }
    }

    private bool SetWallTileIfEmpty(Vector3Int pos)
    {
        if (MapManager.Instance.FloorMap.GetTile(new Vector3Int(pos.x, pos.y, 0)))
        {
            return true;
        }
        else
        {
            MapManager.Instance.ObstacleMap.SetTile(new Vector3Int(pos.x, pos.y, 0), MapManager.Instance.WallTile);
            return false;
        }
    }

    private void BresenhamLine(Vector2Int roomCenter, Vector2Int tunnelCorner, List<Vector2Int> tunnelCoords)
    {
        int x = roomCenter.x, y = roomCenter.y;
        int dx = Mathf.Abs(tunnelCorner.x - roomCenter.x), dy = Mathf.Abs(tunnelCorner.y - roomCenter.y);
        int sx = roomCenter.x < tunnelCorner.x ? 1 : -1,  sy = roomCenter.y < tunnelCorner.y ? 1 : -1;
        int err = dx - dy;
        while (true)
        {
            tunnelCoords.Add(new Vector2Int(x, y));
            if (x == tunnelCorner.x && y == tunnelCorner.y)
            {
                break;
            }
            int e2 = 2 * err;
            if (e2 > dy)
            {
                err -= dy;
                x += sx;
            }

            if (e2 < dx)
            {
                err += dx;
                y += sy;
            }
        }
    }
}
