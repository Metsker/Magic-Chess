using _Scripts.Logic;
using Logic;
using UnityEngine;

namespace _Scripts.Multiplayer
{
    public class DBMoveObject
    {
        public int FirstTileX { get; set; }
        public int FirstTileY { get; set; }
        public int SecondTileX { get; set; }
        public int SecondTileY { get; set; }
        public int Turn { get; set; }

        public Tile GetFirstTile()
        {
            return GridManager.GetTileAtPosition(new Vector2Int(FirstTileX, FirstTileY), out var t) ?
                t : null;
        }
        public Tile GetSecondTile()
        {
            return GridManager.GetTileAtPosition(new Vector2Int(SecondTileX, SecondTileY), out var t) ?
                t : null;
        }
    }
}