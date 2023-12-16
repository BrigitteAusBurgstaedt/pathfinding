using UnityEngine;
using UnityEngine.Tilemaps;

namespace pathfinding
{

    /// <summary>
    /// Das Path Tile enthält zusätzliche Informationen die für den Suchalgorithmus relevant sind und beim umwandeln in einen Spot ausgelesen werden. Das Tile kann über "Assets/Create/2D/Tiles/Path Tile" 
    /// erstellt werden und in eine Tile Palette eingefügt werden.
    /// </summary>
    [CreateAssetMenu(fileName = "New Path Tile", menuName = "2D/Tiles/Path Tile")]
    public class PathTile : Tile
    {
        public int Cost;
        public bool IsWalkable;
    }
}

