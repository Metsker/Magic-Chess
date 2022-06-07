using System.Collections.Generic;
using _Scripts.Logic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Logic
{
    public class Tile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject highlight;
        [SerializeField] private GameObject valid;
        public bool isValid { get; private set; }

        public Piece piece;
        private SpriteRenderer _sRenderer;

        public void InitTile(bool isOffset)
        {
            _sRenderer = GetComponent<SpriteRenderer>();
            _sRenderer.color = isOffset  ? GridManager.gridManager.offsetColor  : GridManager.gridManager.baseColor;
        }

        public void InitPiece(Piece.Pieces type, Piece.Side side)
        {
            var p = Instantiate(GridManager.gridManager.pieces[(int)type], transform);
        
            p.Init(this, side);
        }
    
        public Vector2Int GetPosition()
        {
            return GridManager.V3ToV2Int(transform.position);
        }

        public void SwitchValid(bool state)
        {
            valid.SetActive(state);
            isValid = state;
        }
    
        public void SwitchHighlight(bool state)
        {
            highlight.SetActive(state);
        }
    
        public List<Tile> GetTilesBeforeEnemy(List<Vector2Int> dir, int steps, Piece.Side side)
        {
            var t = new List<Tile>();

            foreach (var d in dir)
            {
                GoThrough(d);
            }

            void GoThrough(Vector2Int d)
            {
                d *= (int) side;
                var v = GetPosition() + d;
                for (int i = 0; i < steps; i++)
                {
                    if (GridManager.Tiles.TryGetValue(v, out var tile))
                    {
                        v += d;
                        switch (!tile.IsEmpty())
                        {
                            case true when tile.piece.side == side:
                                return;
                            case true when tile.piece.side != side:
                                t.Add(tile);
                                return;
                        }
                        t.Add(tile);
                    }
                }
            }
            return t;
        }
    
        public List<Tile> GetOnlyEnemyTiles(List<Vector2Int> dir, int steps, Piece.Side side)
        {
            var t = new List<Tile>();

            foreach (var d in dir)
            {
                GoThrough(d);
            }

            void GoThrough(Vector2Int d)
            {
                d *= (int) side;
                var v = GetPosition() + d;
                for (int i = 0; i < steps; i++)
                {
                    if (!GridManager.Tiles.TryGetValue(v, out var tile)) continue;
                    v += d;
                    if (tile.IsEmpty() || tile.piece.side == side) continue;
                    t.Add(tile);
                }
            }
            return t;
        }

        public List<Tile> GetOnlyEmptyTiles(Vector2Int[] dir, int steps, Piece.Side side)
        {
            var t = new List<Tile>();

            foreach (var d in dir)
            {
                GoThrough(d);
            }
        
            void GoThrough(Vector2Int d)
            {
                d *= (int) side;
                var v = GetPosition() + d;
                for (int i = 0; i < steps; i++)
                {
                    if (!GridManager.Tiles.TryGetValue(v, out var tile)) continue;
                    v += d;
                    if (!tile.IsEmpty()) return;
                    t.Add(tile);
                }
            }
            return t;
        }
    
        public List<Tile> GetTiles(List<Vector2Int> poses, Piece.Side side)
        {
            var t = new List<Tile>();
            foreach (var p in poses)
            {
                var v = GetPosition() + p;
                if (GridManager.Tiles.TryGetValue(v, out var tile))
                {
                    switch (!tile.IsEmpty())
                    {
                        case true when tile.piece.side == side:
                            continue;
                        case true when tile.piece.side != side:
                            break;
                    }
                    t.Add(tile);
                }
            }
            return t;
        }
    
        public bool IsEmpty()
        {
            return piece == null;
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (piece.side == PlayerProps.side)
            {
                SwitchHighlight(true);
            }
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            SwitchHighlight(false);
        }
    }
}
