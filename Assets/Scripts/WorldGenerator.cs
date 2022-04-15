//using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGenerator : MonoBehaviour
{
    int Biome_Width;
    int Biome_Height;

    bool[,] CA_Map;

    [SerializeField] int Width;
    [SerializeField] int Height;
    [Range(0, 8)]
    [SerializeField] int LiveNeighboursRequired;
    [SerializeField] int SmoothingSteps;

    [SerializeField] Tilemap StartingZone;
    [SerializeField] Tile FloorTile;
    [SerializeField] Tile WallTile;

    [SerializeField] Tilemap Nature_Biome;
    [SerializeField] Tile NatureFloorTile;
    [SerializeField] Tile NatureWallTile;

    [SerializeField] Tilemap Water_Biome;
    [SerializeField] Tile WaterFloorTile;
    [SerializeField] Tile WaterWallTile;

    [SerializeField] Tilemap Fire_Biome;
    [SerializeField] Tile FireFloorTile;
    [SerializeField] Tile FireWallTile;

    [SerializeField] int WallThresholdSize;
    [SerializeField] int RoomThresholdSize;

    [Range(1, 100)]
    [SerializeField] int FillPercent;

    [SerializeField] int PassageRadius;

    //watch this to join rooms: https://www.youtube.com/watch?v=eVb9kQXvEZM&list=PLFt_AvWsXl0eZgMK_DT5_biRkWXftAOf9&index=6&ab_channel=SebastianLague
    //rooms look rare, maybe we can make this rare spawn? loot, mob, event?
    //if a room is really small (maybe rarity loot/boss/mob etc based on size)
    //if single room + really big room, spawn boss?
        //would need to make sure there is atleast 1-3 bosses or something per map

    private void Start()
    {
        Biome_Width = Width * 5;
        Biome_Height = Height * 5;

        GenerateMap(StartingZone, FloorTile, WallTile, Width, Height);
        GenerateMap(Nature_Biome, NatureFloorTile, NatureWallTile, Biome_Width, Biome_Height);
        GenerateMap(Water_Biome, WaterFloorTile, WaterWallTile, Biome_Width, Biome_Height);
        GenerateMap(Fire_Biome, FireFloorTile, FireWallTile, Biome_Width, Biome_Height);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GenerateMap(StartingZone, FloorTile, WallTile, Width, Height);
            GenerateMap(Nature_Biome, NatureFloorTile, NatureWallTile, Biome_Width, Biome_Height);
            GenerateMap(Water_Biome, WaterFloorTile, WaterWallTile, Biome_Width, Biome_Height);
            GenerateMap(Fire_Biome, FireFloorTile, FireWallTile, Biome_Width, Biome_Height);
        }
    }

    void GenerateMap(Tilemap map, Tile floorTile, Tile wallTile, int width, int height)
    {
        InitialiseCAMap(width, height);
        SmoothCAMap(width, height);
        ProcessMap(width, height);
        FillMap(map, floorTile, wallTile, width, height);
    }

    void GenerateBiome()
    {
        //biome grid 10x10
        //define starting index of 10x10 grid
        //for each number of active biome tiles, loop and randomly pick a direction and go that way
            //avoid blocking self, if the next selection is not the last selection and is surrounded on all sides, pick again
            //active tile if determined ok to activate
            //store connected sides for later pathing
        //loop through biome grid and for each active one, generate a map in that location

        //pathing
        //for each stored connected side, pick a random point somewhere long the connected side, cut of a 3x10 path through each grid to have an open path connecting the zone
    }

    void PopulateBiome()
    {

    }

    void InitialiseCAMap(int width, int height)
    {
        CA_Map = new bool[width, height];
        for(int x = 0; x < width; x++)
            for(int y = 0; y < height; y++)
            {
                float r = Random.Range(1, 100);
                if (x == 0 || x == width - 1 || y==0 || y== height - 1)
                {
                    CA_Map[x, y] = true;
                }
                else
                {
                    if (r < FillPercent)
                        CA_Map[x, y] = true;
                    else
                        CA_Map[x, y] = false;
                }
            }
    }

    void SmoothCAMap(int width, int height)
    {
        for(int s = 0; s < SmoothingSteps; s++)
        {
            for(int x = 0; x < width; x++)
                for(int y = 0; y < height; y++)
                {
                    int aliveCount = GetAliveNeighborCount(x, y, width, height);
                    if (aliveCount > LiveNeighboursRequired)
                        CA_Map[x, y] = true;
                    else if (aliveCount < LiveNeighboursRequired)
                        CA_Map[x, y] = false;
                }
        }
    }

    int GetAliveNeighborCount(int x, int y, int width, int height)
    {
        int aliveCount = 0;
        for (int neighbor_X = x - 1; neighbor_X <= x + 1; neighbor_X++)
            for (int neighbor_Y = y - 1; neighbor_Y <= y + 1; neighbor_Y++)
            {
                //if (neighbor_X >= 0 && neighbor_X < Width && neighbor_Y >= 0 && neighbor_Y < Height)
                if(IsInMapRange(neighbor_X, neighbor_Y, width, height))
                {
                    if (neighbor_X != x || neighbor_Y != y)
                    {
                        aliveCount += CA_Map[neighbor_X, neighbor_Y] ? 1 : 0;
                    }
                }
                else
                {
                    aliveCount++;
                }
            }

        return aliveCount;
    }

    void FillMap(Tilemap map, Tile floorTile, Tile wallTile, int width, int height)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                bool value = CA_Map[x, y];

                Tile t;
                if (value)
                    t = wallTile;
                else
                    t = floorTile;

                map.SetTile(new Vector3Int(x, y, 0), t);
            }
        }
    }

    void ProcessMap(int width, int height)
    {
        //Removes isolated small walls
        List<List<Coord>> wallRegions = GetRegions(true, width, height);

        foreach (List<Coord> wallRegion in wallRegions)
        {
            if (wallRegion.Count < WallThresholdSize)
            {
                foreach (Coord tile in wallRegion)
                {
                    CA_Map[tile.tileX, tile.tileY] = false;
                }
            }
        }

        //Removes isolated small rooms
        List<List<Coord>> roomRegions = GetRegions(false, width, height);
        List<Room> survivingRooms = new List<Room>();

        foreach (List<Coord> roomRegion in roomRegions)
        {
            if (roomRegion.Count < RoomThresholdSize)
            {
                foreach (Coord tile in roomRegion)
                {
                    CA_Map[tile.tileX, tile.tileY] = true;
                }
            }
            else
            {
                survivingRooms.Add(new Room(roomRegion, CA_Map));
            }
        }

        survivingRooms.Sort();
        survivingRooms[0].IsMainRoom = true;
        survivingRooms[0].IsAccessibleFromMainRoom = true;

        ConnectClosestRooms(survivingRooms, width, height);
    }

    void ConnectClosestRooms(List<Room> allRooms, int width, int height, bool forceAccessibilityFromMainRoom = false)
    {
        List<Room> roomListA = new List<Room>();
        List<Room> roomListB = new List<Room>();

        if (forceAccessibilityFromMainRoom)
        {
            foreach(Room room in allRooms)
            {
                if(room.IsAccessibleFromMainRoom)
                {
                    roomListB.Add(room);
                }
                else
                {
                    roomListA.Add(room);
                }
            }
        }
        else
        {
            roomListA = allRooms;
            roomListB = allRooms;
        }

        int bestDistance = 0;
        Coord bestTileA = new Coord();
        Coord bestTileB = new Coord();
        Room bestRoomA = new Room();
        Room bestRoomB = new Room();
        bool possibleConnectionFound = false;
        
        foreach(Room roomA in roomListA)
        {
            if(!forceAccessibilityFromMainRoom)
            {
                possibleConnectionFound = false;
                if(roomA.ConnectedRooms.Count > 0)
                {
                    continue;
                }
            }

            foreach(Room roomB in roomListB)
            {
                if (roomA == roomB || roomA.IsConnected(roomB))
                    continue;

                for(int tileIndexA = 0; tileIndexA < roomA.EdgeTiles.Count; tileIndexA++)
                    for (int tileIndexB = 0; tileIndexB < roomB.EdgeTiles.Count; tileIndexB++)
                    {
                        Coord tileA = roomA.EdgeTiles[tileIndexA];
                        Coord tileB = roomB.EdgeTiles[tileIndexB];
                        int distanceBetweenRooms = (int)(Mathf.Pow(tileA.tileX - tileB.tileX, 2) + Mathf.Pow(tileA.tileY - tileB.tileY, 2));

                        if (distanceBetweenRooms < bestDistance || !possibleConnectionFound)
                        {
                            bestDistance = distanceBetweenRooms;
                            possibleConnectionFound = true;
                            bestTileA = tileA;
                            bestTileB = tileB;
                            bestRoomA = roomA;
                            bestRoomB = roomB;
                        }
                    }
            }

            if (possibleConnectionFound && !forceAccessibilityFromMainRoom)
            {
                CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB, width, height);
            }
        }

        if(possibleConnectionFound && forceAccessibilityFromMainRoom)
        {
            CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB, width, height);
            ConnectClosestRooms(allRooms, width, height, true);
        }

        if(!forceAccessibilityFromMainRoom)
        {
            ConnectClosestRooms(allRooms, width, height, true);
        }
    }

    void CreatePassage(Room roomA, Room roomB, Coord tileA, Coord tileB, int width, int height)
    {
        Room.ConnectRooms(roomA, roomB);

        List<Coord> line = GetLine(tileA, tileB);
        foreach(Coord c in line)
        {
            DrawCircle(c, PassageRadius, width, height);
        }
    }

    void DrawCircle(Coord c, int r, int width, int height)
    {
        for (int x = -r; x <= r; x++)
        {
            for (int y = -r; y <= r; y++)
            {
                if(x*x + y*y <= r*r)
                {
                    int drawX = c.tileX + x;
                    int drawY = c.tileY + y;

                    if (IsInMapRange(drawX, drawY, width, height))
                    {
                        CA_Map[drawX, drawY] = false;
                    }
                }
            }
        }
    }

    List<Coord> GetLine(Coord start, Coord end)
    {
        List<Coord> line = new List<Coord>();

        int x = start.tileX;
        int y = start.tileY;

        int dx = end.tileX - start.tileX;
        int dy = end.tileY - start.tileY;

        bool inverted = false;
        int step = System.Math.Sign(dx);
        int gradientStep = System.Math.Sign(dy);

        int longest = Mathf.Abs(dx);
        int shortest = Mathf.Abs(dy);

        if (longest < shortest)
        {
            inverted = true;
            longest = Mathf.Abs(dy);
            shortest = Mathf.Abs(dx);

            step = System.Math.Sign(dy);
            gradientStep = System.Math.Sign(dx);
        }

        int gradientAccumulation = longest / 2;
        for(int i = 0; i < longest; i++)
        {
            line.Add(new Coord(x, y));
            if (inverted)
            {
                y += step;
            }
            else
            {
                x += step;
            }

            gradientAccumulation += shortest;
            if(gradientAccumulation >= longest)
            {
                if (inverted)
                {
                    x += gradientStep;
                }
                else
                {
                    y += gradientStep;
                }
                gradientAccumulation -= longest;
            }
        }

        return line;
    }

    Vector3 CoordToWorldPoint(Coord tile)
    {
        //return new Vector3(-Width / 2 + 0.5f + tile.tileX, -Height / 2 + 0.5f + tile.tileY, -5);
        return new Vector3(tile.tileX, tile.tileY, 0);
    }

    List<List<Coord>> GetRegions(bool tileType, int width, int height)
    {
        List<List<Coord>> regions = new List<List<Coord>>();
        bool[,] mapFlags = new bool[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (mapFlags[x,y] == false && CA_Map[x,y] == tileType)
                {
                    List<Coord> newRegion = GetRegionTiles(x, y, width, height);
                    regions.Add(newRegion);
                    
                    foreach(Coord tile in newRegion)
                    {
                        mapFlags[tile.tileX, tile.tileY] = true;
                    }
                }
            }
        }

        return regions;
    }

    List<Coord> GetRegionTiles(int startX, int startY, int width, int height)
    {
        List<Coord> tiles = new List<Coord>();
        bool[,] mapFlags = new bool[width, height];
        bool tileType = CA_Map[startX, startY];

        Queue<Coord> queue = new Queue<Coord>();
        queue.Enqueue(new Coord(startX, startY));
        mapFlags[startX, startY] = true;

        while(queue.Count > 0)
        {
            Coord tile = queue.Dequeue();
            tiles.Add(tile);

            for (int x = tile.tileX - 1; x <= tile.tileX + 1; x++)
                for (int y = tile.tileY - 1; y <= tile.tileY + 1; y++)
                {
                    if(IsInMapRange(x, y, width, height) && (y == tile.tileY || x == tile.tileX))
                    {
                        if(mapFlags[x,y] == false && CA_Map[x,y] == tileType)
                        {
                            mapFlags[x, y] = true;
                            queue.Enqueue(new Coord(x, y));
                        }
                    }
                }

        }

        return tiles;
    }

    bool IsInMapRange(int x, int y, int width, int height)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }

    struct Coord
    {
        public int tileX;
        public int tileY;

        public Coord(int x, int y)
        {
            tileX = x;
            tileY = y;
        }
    }

    class Room : System.IComparable<Room>
    {
        public List<Coord> Tiles;
        public List<Coord> EdgeTiles;
        public List<Room> ConnectedRooms;
        public int RoomSize;
        public bool IsMainRoom;
        public bool IsAccessibleFromMainRoom;

        public Room()
        { }

        public Room(List<Coord> roomTiles, bool[,] map)
        {
            Tiles = roomTiles;
            RoomSize = Tiles.Count;
            ConnectedRooms = new List<Room>();
            EdgeTiles = new List<Coord>();
            foreach(Coord tile in Tiles)
            {
                for (int x = tile.tileX - 1; x <= tile.tileX + 1; x++)
                    for (int y = tile.tileY - 1; y <= tile.tileY + 1; y++)
                    {
                        if (x == tile.tileX || y == tile.tileY)
                        {
                            if (map[x,y] == true)
                            {
                                EdgeTiles.Add(tile);
                            }
                        }
                    }
            }
        }

        public static void ConnectRooms(Room roomA, Room roomB)
        {
            if (roomA.IsAccessibleFromMainRoom)
                roomB.SetAccessibleFromMainRoom();
            else if (roomB.IsAccessibleFromMainRoom)
                roomA.SetAccessibleFromMainRoom();

            roomA.ConnectedRooms.Add(roomB);
            roomB.ConnectedRooms.Add(roomA);
        }

        public int CompareTo(Room other)
        {
            return other.RoomSize.CompareTo(RoomSize);
        }

        public bool IsConnected(Room otherRoom)
        {
            return ConnectedRooms.Contains(otherRoom);
        }

        public void SetAccessibleFromMainRoom()
        {
            if(!IsAccessibleFromMainRoom)
            {
                IsAccessibleFromMainRoom = true;
                foreach(Room connectedRoom in ConnectedRooms)
                {
                    connectedRoom.SetAccessibleFromMainRoom();
                }
            }
        }
    }
}
