
using System.Collections.Generic;
using UnityEngine;

public static partial class ProceduralDungeonGenerator
{
    public static void ProcedurallyGenerateDungeon()
    {
        //for (int i = 0; i < 100; i++)
        //{
        //    if (Roll(10))
        //        UnityEngine.Debug.Log("Roll with 10% chance: True");
        //    else
        //        UnityEngine.Debug.Log("Roll with 10% chance: False");
        //}

        //Room startRoom0x0 = AddRoom(RoomType.Start, new Coordinate(0, 0));
        //Room room1x0 = AddRoom(RoomType.Normal, new Coordinate(1, 0));
        //Room room_1x0 = AddRoom(RoomType.Normal, new Coordinate(-1, 0));
        //Room room0x_1 = AddRoom(RoomType.Normal, new Coordinate(0, -1));
        //Room shopRoom_2x0 = AddRoom(RoomType.Shop, new Coordinate(-2, 0));
        //Room room1x_1 = AddRoom(RoomType.Trap, new Coordinate(1, -1));
        //Room room0x_2 = AddRoom(RoomType.Normal, new Coordinate(0, -2));
        //Room room0x_3 = AddRoom(RoomType.Trap, new Coordinate(0, -3));
        //Room treasureRoom1x_3 = AddRoom(RoomType.Treasure, new Coordinate(1, -3));
        //Room secretRoom1x_2 = AddRoom(RoomType.Secret, new Coordinate(1, -2));
        //Room bossRoom_1x_3 = AddRoom(RoomType.Boss, new Coordinate(-1, -3));
        //Room superSecretRoom0x_4 = AddRoom(RoomType.SuperSecret, new Coordinate(0, -4));
        //
        //AddDoor(DoorType.Open, startRoom0x0, room1x0);
        //AddDoor(DoorType.Open, startRoom0x0, room_1x0);
        //AddDoor(DoorType.Open, startRoom0x0, room0x_1);
        //AddDoor(DoorType.Locked, room_1x0, shopRoom_2x0);
        //AddDoor(DoorType.Open, room1x0, room1x_1);
        //AddDoor(DoorType.Open, room0x_1, room1x_1);
        //AddDoor(DoorType.Open, room0x_1, room0x_2);
        //AddDoor(DoorType.Open, room0x_2, room0x_3);
        //AddDoor(DoorType.Locked, room0x_3, treasureRoom1x_3);
        //AddDoor(DoorType.Bombable, room0x_2, secretRoom1x_2);
        //AddDoor(DoorType.Bombable, room1x_1, secretRoom1x_2);
        //AddDoor(DoorType.Bombable, treasureRoom1x_3, secretRoom1x_2);
        //AddDoor(DoorType.Open, room0x_3, bossRoom_1x_3);
        //AddDoor(DoorType.Bombable, room0x_3, superSecretRoom0x_4);

        Room startRoom0x0 = AddRoom(RoomType.Start, new Coordinate(0, 0));
        GenerateRoomsFromRoom(startRoom0x0);



        UnityEngine.Debug.Log("There are " + GetDungeonRooms().Count + " rooms in this dungeon.");
        UnityEngine.Debug.Log("There are " + GetDungeonDoors().Count + " doors in this dungeon.");

        foreach(Room r in GetDungeonRooms()) { }
        foreach(Door d in GetDungeonDoors()) { }
    }

    static void GenerateRoomsFromRoom(Room startRoom)
    {

        switch (startRoom.type)
        {
            case RoomType.Start:
            {
                int numberOfRooms = Random.Range(2, 5);
                for (int i = 0; i < numberOfRooms; i++)
                {
                    Coordinate roomCoords = FindEmptySpaceBesideRoom(startRoom);

                    if (roomCoords == null) continue;

                    Room newRoom = AddRoom(RoomType.Normal, roomCoords);
                    newRoom.depth = startRoom.depth + 1;
                    newRoom.type = DetermineRoomTypeFromDepth(newRoom.depth);
                    GenerateRoomsFromRoom(newRoom);
                    AddDoor(DoorType.Open, startRoom, newRoom);
                }
            }
                break;
            case RoomType.Normal:
            {
                int numberOfRooms = Random.Range(1, 3);
                for (int i = 0; i < numberOfRooms; i++)
                {
                    Coordinate roomCoords = FindEmptySpaceBesideRoom(startRoom);

                    if (roomCoords == null) continue;

                    // Determine if you should spawn rooms based on distance from start
                    if (startRoom.depth < MaximumRoomDepth)
                    {
                        // Determine room type based off the depth
                        Room newRoom = AddRoom(RoomType.Normal, roomCoords);
                        newRoom.depth = startRoom.depth + 1;
                        newRoom.type = DetermineRoomTypeFromDepth(newRoom.depth);

                        if (newRoom.type == RoomType.Normal)
                        {
                            GenerateRoomsFromRoom(newRoom);
                        }

                        AddDoor(DoorType.Open, startRoom, newRoom);
                    }
                    else
                    {
                        Room newRoom = AddRoom(RoomType.Normal, roomCoords);
                        newRoom.depth = startRoom.depth + 1;

                        if (FindRoomOfType(RoomType.Treasure) == null)
                        {
                            newRoom.type = RoomType.Treasure;
                        }
                        else if (FindRoomOfType(RoomType.Shop) == null)
                        {
                            newRoom.type = RoomType.Shop;
                        }
                        else if (FindRoomOfType(RoomType.Boss) == null)
                        {
                            newRoom.type = RoomType.Boss;
                        }

                        AddDoor(DoorType.Locked, startRoom, newRoom);
                    }
                }

                break;
            }
        }
    }

    private static Coordinate FindEmptySpaceBesideRoom(Room room)
    {
        Debug.Log("Room Coords: " + room.coordinate.x + ", " + room.coordinate.y);

        Coordinate currentRoomCoordinate = room.coordinate;
        List<Coordinate> freeCoordinates = new List<Coordinate>();

        Coordinate xPlusOne = new Coordinate(currentRoomCoordinate.x + 1, currentRoomCoordinate.y);
        Coordinate yPlusOne = new Coordinate(currentRoomCoordinate.x, currentRoomCoordinate.y + 1);
        Coordinate xMinusOne = new Coordinate(currentRoomCoordinate.x - 1, currentRoomCoordinate.y);
        Coordinate yMinusOne = new Coordinate(currentRoomCoordinate.x, currentRoomCoordinate.y - 1);

        //if x+1 is empty, if y+1 is empty, if x-1 is empty, if y-1 is empty
        bool isxPlusOneFree = CheckIfCoordinateAndNeighboursIsEmpty(xPlusOne);
        bool isyPlusOneFree = CheckIfCoordinateAndNeighboursIsEmpty(yPlusOne);
        bool isxMinusOneFree = CheckIfCoordinateAndNeighboursIsEmpty(xMinusOne);
        bool isyMinusOneFree = CheckIfCoordinateAndNeighboursIsEmpty(yMinusOne);

        if(isxPlusOneFree) freeCoordinates.Add(xPlusOne);
        if(isxMinusOneFree) freeCoordinates.Add(xMinusOne);
        if(isyPlusOneFree) freeCoordinates.Add(yPlusOne);
        if(isyMinusOneFree) 
            freeCoordinates.Add(yMinusOne);

        // randomly pick one to spit out thats free
        int numRoomsFree =
            (isxPlusOneFree ? 1 : 0)+
            (isyPlusOneFree ? 1 : 0) +
            (isxMinusOneFree ? 1 : 0) +
            (isyMinusOneFree ? 1 : 0);

        if (numRoomsFree > 0)
        {
            int randomRoom = Random.Range(0, numRoomsFree);
            return freeCoordinates[randomRoom];

        }
        
        return null;
    }

    private static RoomType DetermineRoomTypeFromDepth(int depth)
    {
        if (Roll(20 * depth * FloorPercentChanceModifier) && FindRoomOfType(RoomType.Treasure) == null)
        {
            return RoomType.Treasure;
        }

        if (Roll(20 * depth / 2.0f * FloorPercentChanceModifier) && FindRoomOfType(RoomType.Shop) == null)
        {
            return RoomType.Shop;
        }

        return RoomType.Normal;
    }


}
