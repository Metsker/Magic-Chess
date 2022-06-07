using System.Collections.Generic;
using Logic;
using UnityEngine;

namespace Pieces
{
    public class Bishop : Piece
    {
        protected override void DrawValidTiles()
        {
            foreach (var t in tile.GetTilesBeforeEnemy(GetDirections(), 8, side))
            {
                t.SwitchValid(true);
            }
        }

        private List<Vector2Int> GetDirections()
        {
            return new()
            {
                Vector2Int.one, 
                -Vector2Int.one,
                UpLeft,
                -UpLeft
            };
        }
    }
}
