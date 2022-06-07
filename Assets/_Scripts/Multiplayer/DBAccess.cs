using System;
using System.Globalization;
using System.Threading.Tasks;
using _Scripts.Logic;
using ExitGames.Client.Photon;
using Logic;
using MongoDB.Bson;
using MongoDB.Driver;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace _Scripts.Multiplayer
{
    public class DBAccess : MonoBehaviour, IOnEventCallback
    {
        private readonly MongoClient _client = new ("mongodb+srv://Metsker:p@cluster0.pk6ld.mongodb.net/?retryWrites=true&w=majority");
        private IMongoDatabase _database;
        private IMongoCollection<BsonDocument> _collection;

        public static string roomId { get; set; }

        private async void Start()
        {
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                roomId = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient };
                PhotonNetwork.RaiseEvent(22, 123, raiseEventOptions, SendOptions.SendUnreliable);
            }
            while (roomId == null)
            {
                await Task.Yield();
            }
        
            _database = _client.GetDatabase("ChessDB");
            var cName = roomId;

            if (!(await _database.ListCollectionNamesAsync()).ToList().Contains(cName))
            {
                await _database.CreateCollectionAsync(cName, new CreateCollectionOptions
                {
                    Capped = true, MaxSize = 1024, MaxDocuments = 10,
                });
            }
        
            _collection = _database.GetCollection<BsonDocument>(cName);
        }

        private void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        private void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        public async void DBMove(Tile firstTile, Tile secondTile)
        {
            var t = GridManager.turn - 1;
            var move = new DBMoveObject
            {
                FirstTileX = firstTile.GetPosition().x,
                FirstTileY = firstTile.GetPosition().y,
                SecondTileX = secondTile.GetPosition().x,
                SecondTileY = secondTile.GetPosition().y,
                Turn = t
            };
        
            await _collection.InsertOneAsync(move.ToBsonDocument());

            RaiseEventOptions ro = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
            SendOptions so = new SendOptions { Reliability = true };
            PhotonNetwork.RaiseEvent(42, t, ro, so);
        }

        private DBMoveObject GetLastMove(int t)
        {
            BsonDocument bs = new BsonDocument("Turn", t);
            FindOptions fo = new FindOptions
            {
                MaxAwaitTime = new TimeSpan(0,0,10)
            };
            var r = _collection.Find(bs,fo);

            DBMoveObject moveObject = new DBMoveObject();
            foreach (var s in r.ToList())
            {
                moveObject = new DBMoveObject
                {
                    FirstTileX = (int)s[1],
                    FirstTileY = (int)s[2],
                    SecondTileX = (int)s[3],
                    SecondTileY = (int)s[4]
                };
            }
            return moveObject;
        }

        public void OnEvent(EventData photonEvent)
        {
            switch (photonEvent.Code)
            {
                case 22:
                    SetRoomIdToClient();
                    break;
                case 32:
                    roomId = (string)photonEvent.CustomData;
                    break;
                case 42:
                    FindObjectOfType<GridManager>().
                        UpdateOthersGameState(GetLastMove((int)photonEvent.CustomData));
                    break;
            }
        }
        private void SetRoomIdToClient()
        {
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
            PhotonNetwork.RaiseEvent(32, roomId, raiseEventOptions, SendOptions.SendReliable);
        }
    }
}
