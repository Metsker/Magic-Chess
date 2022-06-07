using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using _Scripts.Classes;
using _Scripts.Logic;
using _Scripts.Multiplayer;
using Photon.Pun;
using Pieces;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Logic
{
    public abstract class Piece : MonoBehaviour, IBeginDragHandler, IDragHandler, IDropHandler
    {
        public Tile startTile { get; private set; }
        [HideInInspector]
        public Tile tile;
        public Side side;
        public Pieces pieceType;

        private SpriteRenderer _spriteRenderer;
        
        private static GridManager _gridManager;
        private static DBAccess _dbAccess;
        private static Camera _camera;
        
        public Vector2Int beginDragPos;
        public bool isMoved;
        
        protected readonly Vector2Int UpLeft = new (-1, 1);

        public enum Side
        {
            White = 1,
            Black = -1
        }
        public enum Pieces
        {
            King,
            Queen,
            Rook,
            Bishop,
            Knight,
            Pawn
        }

        protected void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            
            if (_camera == null)
            {
                _camera = Camera.main;
            }

            if (_gridManager == null)
            {
                _gridManager = FindObjectOfType<GridManager>();
            }

            if (_dbAccess == null)
            {
                _dbAccess = FindObjectOfType<DBAccess>();
            }
        }

        private void Start()
        {
            startTile = tile;
        }

        public void Init(Tile t, Side s)
        {
            Init(t);
            side = s;
            
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.sprite = side == Side.White ?
                GridManager.gridManager.white[(int)pieceType] :
                GridManager.gridManager.black[(int)pieceType] ;

            if (!Application.isPlaying) return;
            _spriteRenderer.flipX = PlayerProps.side != Side.White ;
            _spriteRenderer.flipY = PlayerProps.side != Side.White ;

        }
        public void Init(Tile t)
        {
            tile = t;

            transform.SetParent(tile.transform);
            transform.SetSiblingIndex(0);
            transform.position = t.transform.position;
            
            tile.piece = this;
        }

        #region OnPointers

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!IsValidEvent()) { return; }
            
            _spriteRenderer.sortingOrder++;
            beginDragPos = tile.GetPosition();
            DrawValidTiles();
        }
    
        public void OnDrag(PointerEventData eventData)
        {
            if (!IsValidEvent()) { return; }
            
            var v = _camera.ScreenToWorldPoint(Input.mousePosition); v.z = 0;
            transform.position = v;
        }
    
    
        [SuppressMessage("ReSharper", "Unity.InefficientPropertyAccess")]
        public void OnDrop(PointerEventData eventData)
        {
            if (!IsValidEvent()) { return; }
            
            if (GetEventTile(eventData, out var t)  && !t.Equals(tile))
            {
                switch (t.isValid)
                {
                    case true:
                    {
                        var oldT = tile;
                        oldT.SwitchHighlight(false);
                        _gridManager.UpdateGameState(oldT, t);
                        _dbAccess.DBMove(oldT, t);
                        break;
                    }
                    default:
                        transform.position = (Vector2)beginDragPos;
                        break;
                }
            }
            else
            {
                transform.position = (Vector2)beginDragPos;
            }
            _spriteRenderer.sortingOrder--;

            foreach (var vt in FindObjectsOfType<Tile>().Where(tt=> tt.isValid))
            {
                vt.SwitchValid(false);;
            }
        }
        #endregion

        public bool GetEventTile(PointerEventData eventData, out Tile t)
        {
            var wp = _camera.ScreenToWorldPoint(eventData.position);
            var p = GridManager.V3ToV2Int(wp);
            return GridManager.GetTileAtPosition(p, out t);
        }
        private bool IsValidEvent()
        {
            if (Debugger.IsDebug)
            {
                return true;
            }
            return PlayerProps.isMyTurn && side == PlayerProps.side && PhotonNetwork.CurrentRoom.PlayerCount == 2;
        }
        protected abstract void DrawValidTiles();
    }
}