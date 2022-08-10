using Assets.Scripts;
using UnityEngine;

public class GameTile : MonoBehaviour
{
    [SerializeField] private Transform _arrow;

    private GameTile _north, _east, _west, _south, _nextOnPath;

    private int _distance;
    public bool HasPath => _distance != int.MaxValue;
    public GameTile NextOnPath => _nextOnPath;

    public bool IsAlternative { get; set; }

    private Quaternion _northRotation = Quaternion.Euler(90, 0, 0);
    private Quaternion _eastRotation = Quaternion.Euler(90, 90, 0);
    private Quaternion _southRotation = Quaternion.Euler(90, 180, 0);
    private Quaternion _westRotation = Quaternion.Euler(90, 270, 0);

    public Vector3 ExitPoint { get; private set; }

    public Direction PathDirection { get; private set; }

    private GameTileContent _content;
    public GameTileContent Content
    {
        get => _content;
        set
        {
            if (_content != null)
            {
                _content.Recycle();
            }
            _content = value;
            _content.transform.localPosition = transform.localPosition;

        }
    }    //initialize tiles neighbours
    public static void MakeEastWestNeighbours(GameTile east, GameTile west)
    {
        west._east = east;
        east._west = west;
    }

    public static void MakeNorthSouthNeighbours(GameTile north, GameTile south)
    {
        north._south = south;
        south._north = north;
    }

    public void ClearPath()
    {
        _distance = int.MaxValue;
        _nextOnPath = null;
    }

    public void BecomeDestination()
    {
        _distance = 0;
        _nextOnPath = null;
        ExitPoint = transform.localPosition;
    }

    private GameTile GrowPathTo(GameTile neighbor, Direction direction)
    {
        if (!HasPath || neighbor == null || neighbor.HasPath)
        {
            return null;
        }

        neighbor._distance = _distance + 1;
        neighbor._nextOnPath = this;
        neighbor.ExitPoint = neighbor.transform.localPosition + direction.GetHalfVectors();
        neighbor.PathDirection = direction;
        return neighbor.Content.IsBlockingPath ? null : neighbor;
    }

    public GameTile GrowPathNorth() => GrowPathTo(_north, Direction.South);
    public GameTile GrowPathSouth() => GrowPathTo(_south, Direction.North);
    public GameTile GrowPathEast() => GrowPathTo(_east, Direction.West);
    public GameTile GrowPathWest() => GrowPathTo(_west, Direction.East);

    public void ShowPath()
    {
        if (_distance == 0)
        {
            _arrow.gameObject.SetActive(false);
            return;
        }

        _arrow.gameObject.SetActive(true);
        _arrow.localRotation =
            _nextOnPath == _north ? _northRotation :
            _nextOnPath == _south ? _southRotation :
            _nextOnPath == _west ? _westRotation :
            _eastRotation;
    }
}

