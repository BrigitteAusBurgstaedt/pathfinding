using UnityEngine;
using UnityEngine.Tilemaps;

namespace pathfinding
{

    /// <summary>
    /// Das Path Tile enth�lt zus�tzliche Informationen die f�r den Suchalgorithmus relevant sind und beim umwandeln in einen Spot ausgelesen werden. Das Tile kann �ber "Assets/Create/2D/Tiles/Path Tile" 
    /// erstellt werden und in eine Tile Palette eingef�gt werden.
    /// </summary>
    [CreateAssetMenu(fileName = "New Path Tile", menuName = "2D/Tiles/Path Tile")]
    public class PathTile : Tile
    {
        public int Cost;
        public bool IsWalkable;
    }
}

