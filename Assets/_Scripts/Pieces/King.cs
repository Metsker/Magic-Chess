using System;
using System.Collections.Generic;
using _Scripts.Logic;
using Logic;
using Photon.Pun;
using UnityEngine;

namespace Pieces
{
    public class King : Piece
    {
        [SerializeField] private GameObject deathEffect;
        
        protected override void DrawValidTiles()
        {
            foreach (var t in tile.GetTilesBeforeEnemy(GetDirections(), 1, side))
            {
                t.SwitchValid(true);
            }
        }

        private List<Vector2Int> GetDirections()
        {
            return new()
            {
                Vector2Int.up,
                Vector2Int.left,
                Vector2Int.right,
                Vector2Int.down,
                Vector2Int.one,
                -Vector2Int.one,
                UpLeft,
                -UpLeft
            };
        }

        private void OnDestroy()
        {
            deathEffect.SetActive(true);
            PlayerProps.isMyTurn = false;
        }
    }
}
