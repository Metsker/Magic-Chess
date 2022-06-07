using System;
using System.Threading.Tasks;
using _Scripts.Classes;
using ExitGames.Client.Photon;
using Logic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.Logic
{
    public class PlayerProps : MonoBehaviour
    {
        public static Piece.Side side { get; private set; }
        public static PlayerProps Player;
        public static bool isMyTurn { get; set; }
        public Classes playerClass;
        private Transform _cam;
        
        public enum Classes
        {
            Necromancer,
            KingsPawn
        }
        
        private async void Start()
        {
            Player = this;
            _cam = Camera.main.transform;
            side = (Piece.Side) Enum.ToObject(typeof(Piece.Side), RandomizedSide()); 

            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                Hashtable h = new Hashtable {{"Side", side}};
                PhotonNetwork.MasterClient.SetCustomProperties(h);
            }
            else
            {
                while (PhotonNetwork.MasterClient.CustomProperties.Count < 1)
                {
                    await Task.Yield();
                }
                
                var mSide = (Piece.Side) PhotonNetwork.MasterClient.CustomProperties["Side"];
                side = mSide == Piece.Side.White ? Piece.Side.Black : Piece.Side.White;

                Hashtable h = new Hashtable {{"Side", (int) side }};
                PhotonNetwork.LocalPlayer.SetCustomProperties(h);
            }
        }

        private void OnEnable()
        {
            GameStarter.OnGameStart += AdjustCamera;
        }

        private void OnDisable()
        {
            GameStarter.OnGameStart -= AdjustCamera;
        }

        private void AdjustCamera()
        {
            switch (side)
            {
                case Piece.Side.White:
                    isMyTurn = true;
                    break;
                case Piece.Side.Black:
                    _cam.Rotate(new Vector3(0,0,180));
                    foreach (var p in FindObjectsOfType<Piece>())
                    {
                        var sr = p.GetComponent<SpriteRenderer>();
                        sr.flipX = true;
                        sr.flipY = true;
                    }
                    break;
            }
        }
        
        private int RandomizedSide()
        {
            switch (Random.Range(0,2))
            {
                case 0:
                    return (int)Piece.Side.Black;
                default:
                    return (int)Piece.Side.White;
            }
            
        }
    }
}