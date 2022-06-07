using System;
using System.Collections.Generic;
using _Scripts.Multiplayer;
using Logic;
using UnityEngine;

namespace _Scripts.Logic
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private int width, height;
        [SerializeField] private Tile tilePrefab;
        [SerializeField] private Transform cameraT;

        public Color baseColor, offsetColor;
        public Piece[] pieces;
        public static int turn { get; private set; }
    
        [Header("Sprites")]
        public Sprite[] white, black;
        public static GridManager gridManager { get; private set; }
        public static Dictionary<Vector2Int, Tile> Tiles;

        public static event Action OnTurnEnd;

        private void Awake()
        {
            gridManager = this;
            Tiles = new Dictionary<Vector2Int, Tile>();
            for (var i = 0; i < transform.childCount; i++)
            {
                var t = transform.GetChild(i).GetComponent<Tile>();
                Tiles[t.GetPosition()] = t;
            }
        }

        private void Start()
        {
            turn = 0;
        }

        [ContextMenu("Create Grid")]
        public void Create()
        {
            GenerateGrid();
        }
        [ContextMenu("Delete Grid")]
        public void Delete()
        {
            foreach (var c in FindObjectsOfType<Tile>())
            {
                DestroyImmediate(c.gameObject);
            }
        }
        [ContextMenu("Reload Grid")]
        public void Reload()
        {
            gridManager = this;
            Delete();
            Create();

            #region White
        
            Tiles[new Vector2Int(0,0)].InitPiece(Piece.Pieces.Rook, Piece.Side.White);
            Tiles[new Vector2Int(1,0)].InitPiece(Piece.Pieces.Knight, Piece.Side.White);
            Tiles[new Vector2Int(2,0)].InitPiece(Piece.Pieces.Bishop, Piece.Side.White);
            Tiles[new Vector2Int(3,0)].InitPiece(Piece.Pieces.King, Piece.Side.White);
            Tiles[new Vector2Int(4,0)].InitPiece(Piece.Pieces.Queen, Piece.Side.White);
            Tiles[new Vector2Int(5,0)].InitPiece(Piece.Pieces.Bishop, Piece.Side.White);
            Tiles[new Vector2Int(6,0)].InitPiece(Piece.Pieces.Knight, Piece.Side.White);
            Tiles[new Vector2Int(7,0)].InitPiece(Piece.Pieces.Rook, Piece.Side.White);
            Tiles[new Vector2Int(0,1)].InitPiece(Piece.Pieces.Pawn, Piece.Side.White);
            Tiles[new Vector2Int(1,1)].InitPiece(Piece.Pieces.Pawn, Piece.Side.White);
            Tiles[new Vector2Int(2,1)].InitPiece(Piece.Pieces.Pawn, Piece.Side.White);
            Tiles[new Vector2Int(3,1)].InitPiece(Piece.Pieces.Pawn, Piece.Side.White);
            Tiles[new Vector2Int(4,1)].InitPiece(Piece.Pieces.Pawn, Piece.Side.White);
            Tiles[new Vector2Int(5,1)].InitPiece(Piece.Pieces.Pawn, Piece.Side.White);
            Tiles[new Vector2Int(6,1)].InitPiece(Piece.Pieces.Pawn, Piece.Side.White);
            Tiles[new Vector2Int(7,1)].InitPiece(Piece.Pieces.Pawn, Piece.Side.White);
            #endregion,
            #region Black,
            Tiles[new Vector2Int(0,7)].InitPiece(Piece.Pieces.Rook, Piece.Side.Black);
            Tiles[new Vector2Int(1,7)].InitPiece(Piece.Pieces.Knight, Piece.Side.Black);
            Tiles[new Vector2Int(2,7)].InitPiece(Piece.Pieces.Bishop, Piece.Side.Black);
            Tiles[new Vector2Int(3,7)].InitPiece(Piece.Pieces.King, Piece.Side.Black);
            Tiles[new Vector2Int(4,7)].InitPiece(Piece.Pieces.Queen, Piece.Side.Black);
            Tiles[new Vector2Int(5,7)].InitPiece(Piece.Pieces.Bishop, Piece.Side.Black);
            Tiles[new Vector2Int(6,7)].InitPiece(Piece.Pieces.Knight, Piece.Side.Black);
            Tiles[new Vector2Int(7,7)].InitPiece(Piece.Pieces.Rook, Piece.Side.Black);
            Tiles[new Vector2Int(0,6)].InitPiece(Piece.Pieces.Pawn, Piece.Side.Black);
            Tiles[new Vector2Int(1,6)].InitPiece(Piece.Pieces.Pawn, Piece.Side.Black);
            Tiles[new Vector2Int(2,6)].InitPiece(Piece.Pieces.Pawn, Piece.Side.Black);
            Tiles[new Vector2Int(3,6)].InitPiece(Piece.Pieces.Pawn, Piece.Side.Black);
            Tiles[new Vector2Int(4,6)].InitPiece(Piece.Pieces.Pawn, Piece.Side.Black);
            Tiles[new Vector2Int(5,6)].InitPiece(Piece.Pieces.Pawn, Piece.Side.Black);
            Tiles[new Vector2Int(6,6)].InitPiece(Piece.Pieces.Pawn, Piece.Side.Black);
            Tiles[new Vector2Int(7,6)].InitPiece(Piece.Pieces.Pawn, Piece.Side.Black);
            #endregion
        }
        private void GenerateGrid()
        {
            Tiles = new Dictionary<Vector2Int, Tile>();
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var tPos = new Vector2Int(x, y);
                    var spawnedTile = Instantiate(tilePrefab, (Vector2)tPos, Quaternion.identity);
                    spawnedTile.transform.SetParent(transform);
                    spawnedTile.name = $"Tile {x}, {y}";

                    var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 ==0);
                
                    spawnedTile.InitTile(isOffset);

                    Tiles[tPos] = spawnedTile;
                }
            }

            cameraT.position = new Vector3(width * 0.5f - 0.5f,height
                * 0.5f - 0.5f, -10);
        }

        public static bool GetTileAtPosition(Vector2Int pos, out Tile t)
        {
            if (Tiles.TryGetValue(pos, out var tile))
            {
                t = tile;
                return true;
            }
            t = null;
            return false;
        }
        public static Tile GetTileAtPosition(Vector3 pos)
        {
            return Tiles.TryGetValue(V3ToV2Int(pos), out var tile) ? tile : null;
        }
        public static Vector2Int V3ToV2Int(Vector3 vector3)
        {
            return new (Mathf.RoundToInt(vector3.x), Mathf.RoundToInt(vector3.y));;
        }
        public void UpdateGameState(Tile oldT, Tile newT)
        {
            if (!newT.IsEmpty())
            {
                Destroy(newT.piece.gameObject);
                //State
            }
            oldT.piece.Init(newT);

            oldT.piece = null;
            newT.piece.isMoved = true;
            turn++;
            PlayerProps.isMyTurn = !PlayerProps.isMyTurn;
        
            OnTurnEnd?.Invoke();
        }
    
        public void UpdateOthersGameState(DBMoveObject moveObject)
        {
            UpdateGameState(moveObject.GetFirstTile(), moveObject.GetSecondTile());
        }
    }
}
