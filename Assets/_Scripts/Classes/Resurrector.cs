using System;
using System.Linq;
using _Scripts.Logic;
using _Scripts.Messages;
using Logic;
using Photon.Pun;
using UnityEngine;

namespace _Scripts
{
    public class Resurrector : MonoBehaviour
    {
        private int turnsToRes { get; set; }
        private Piece.Pieces _pieceType;
        private Tile _tile;

        private void Awake()
        {
            _tile = GetComponent<Tile>();
        }

        public void OnEnable()
        {
            GridManager.OnTurnEnd += OnTurnEnd;
        }

        public void OnDisable()
        {
            GridManager.OnTurnEnd -= OnTurnEnd;
        }

        public void Init(Tile t, Piece.Pieces p, int ttr)
        {
            _tile = t;
            _pieceType = p;
            turnsToRes = ttr;
        }
        
        private void OnTurnEnd()
        {
            turnsToRes--;
            
            if (turnsToRes > 0)
            {
                SendMessage();
                return;
            }

            if (!_tile.IsEmpty())
            {
                _tile = PlayerProps.side == Piece.Side.White ? 
                    GridManager.Tiles.Values.First(t => t.IsEmpty()) : GridManager.Tiles.Values.Last(t => t.IsEmpty());
            }

            PhotonNetwork.Instantiate(_pieceType.ToString(), _tile.transform.position, Quaternion.identity);
            Destroy(this);
        }

        public void SendMessage()
        {
            Log.SendMessageToLog($"Конь воскреснет через {turnsToRes} ход(а)");
        }
    }
}