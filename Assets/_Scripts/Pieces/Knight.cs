using System;
using System.Collections.Generic;
using Logic;
using UnityEngine;

namespace Pieces
{
    public class Knight : Piece
    {
        
        protected override void DrawValidTiles()
        {
            foreach (var t in tile.GetTiles(GetDirections(), side))
            {
                t.SwitchValid(true);
            }
        }
        
        private List<Vector2Int> GetDirections()
        {
            return new()
            {
                new Vector2Int(1,2),
                new Vector2Int(1,-2),
                new Vector2Int(-1,2),
                new Vector2Int(-1,-2),
                new Vector2Int(2,1),
                new Vector2Int(2,-1),
                new Vector2Int(-2,1),
                new Vector2Int(-2,-1),
            };
        }
    }
}
