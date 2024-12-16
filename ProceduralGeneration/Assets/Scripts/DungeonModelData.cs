using System.Collections.Generic;

public static partial class ProceduralDungeonGenerator
{
    static LinkedList<DungeonModelData> dungeonModelData;
    static LinkedList<Room> rooms;
    static LinkedList<Door> doors;
    static System.Random rand;
    const int SeedForRandomGeneration = 1337;
    private const int MaximumRoomDepth = 5;
    private const float FloorPercentChanceModifier = 1.0f;

    public static void Init()
    {
        dungeonModelData = new LinkedList<DungeonModelData>();
        rooms = new LinkedList<Room>();
        doors = new LinkedList<Door>();
        rand = new System.Random(SeedForRandomGeneration);
    }

    private static Room AddRoom(RoomType roomType, Coordinate coordinate)
    {
        Room room = new Room(roomType, coordinate);
        rooms.AddLast(room);
        dungeonModelData.AddLast(room);
        return room;
    }

    private static Door AddDoor(DoorType doorType, Room room1, Room room2)
    {
        Door door = new Door(doorType, room1, room2);
        room1.doors.AddLast(door);
        room2.doors.AddLast(door);
        doors.AddLast(door);
        dungeonModelData.AddLast(door);
        return door;
    }

    private static bool Roll(float percentageChanceOfSuccess)
    {
        float percentageChanceOfSuccessAsDecimal = percentageChanceOfSuccess / 100f;
        return rand.NextDouble() < percentageChanceOfSuccessAsDecimal;
    }

    public static void DestroyDungeon()
    {
        rooms.Clear();
        doors.Clear();
    }

    public static LinkedList<DungeonModelData> GetDungeonModelData()
    {
        return dungeonModelData;
    }

    public static LinkedList<Room> GetDungeonRooms()
    {
        return rooms;
    }
    
    public static LinkedList<Door> GetDungeonDoors()
    {
        return doors;
    }

    public static bool CheckIfCoordinateIsEmpty(Coordinate coordinate)
    {
        foreach (var room in rooms)
        {
            if (room.coordinate.IsEqualTo(coordinate))
            {
                return false;
            }
        }

        return true;
    }

    public static bool CheckIfCoordinateAndNeighboursIsEmpty(Coordinate coordinate)
    {
        foreach (var room in rooms)
        {
            if (room.coordinate.IsEqualTo(coordinate))
            {
                return false;
            }
        }

        Coordinate xPlusOne = new Coordinate(coordinate.x + 1, coordinate.y);
        Coordinate yPlusOne = new Coordinate(coordinate.x, coordinate.y + 1);
        Coordinate xMinusOne = new Coordinate(coordinate.x - 1, coordinate.y);
        Coordinate yMinusOne = new Coordinate(coordinate.x, coordinate.y - 1);

        bool isxPlusOneFree = CheckIfCoordinateIsEmpty(xPlusOne);
        bool isyPlusOneFree = CheckIfCoordinateIsEmpty(yPlusOne);
        bool isxMinusOneFree = CheckIfCoordinateIsEmpty(xMinusOne);
        bool isyMinusOneFree = CheckIfCoordinateIsEmpty(yMinusOne);

        return (isxPlusOneFree && isxMinusOneFree) || (isyPlusOneFree && isyMinusOneFree);
    }

    public static Room FindRoomOfType(RoomType type)
    {
        foreach (var room in rooms)
        {
            if (room.type == type)
            {
                return room;
            }
        }

        return null;
    }
}

public enum RoomType
{
    None, Start, Normal, Secret, SuperSecret, Shop, Treasure, Trap, Boss
}

public enum DoorType
{
    None, Open, Locked, Bombable
}

public class Coordinate
{
    public int x, y;

    public Coordinate(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public bool IsEqualTo(Coordinate otherCoord)
    {
        if (x == otherCoord.x && y == otherCoord.y)
            return true;
        return false;
    }
}

public abstract class DungeonModelData {}

public class Door : DungeonModelData
{
    public UnityEngine.GameObject visual;
    public DoorType type;
    public Room room1, room2;

    public Door(DoorType type, Room room1, Room room2)
    {
        this.type = type;
        this.room1 = room1;
        this.room2 = room2;
    }
}

public class Room : DungeonModelData
{
    public UnityEngine.GameObject visual;
    public RoomType type;
    public Coordinate coordinate;
    public LinkedList<Door> doors;

    public int depth = 0;

    public Room(RoomType type, Coordinate coordinate)
    {
        this.type = type;
        this.coordinate = coordinate;
        doors = new LinkedList<Door>();
    }
}
