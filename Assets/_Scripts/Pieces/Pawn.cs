using System;
using System.Collections.Generic;
using _Scripts.Logic;
using Logic;
using UnityEngine;

namespace Pieces
{
    public class Pawn : Piece
    {
        protected override void DrawValidTiles()
        {
            var steps = isMoved ? 1 : 2;
            
            foreach (var t in tile.GetOnlyEmptyTiles(new [] { Vector2Int.up }, steps, side))
            {
                t.SwitchValid(true);
            }
            
            foreach (var t in tile.GetOnlyEnemyTiles(GetDirections(), 1, side))
            {
                t.SwitchValid(true);
            }
        }

        private List<Vector2Int> GetDirections()
        {
            var list = new List<Vector2Int>
            {
                Vector2Int.one,
                UpLeft
            };

            if (PlayerProps.Player.playerClass != PlayerProps.Classes.KingsPawn) return list;
            list.Add(Vector2Int.up);

            return list;
        }
    }
}