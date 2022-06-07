using System;
using _Scripts.Logic;
using Logic;
using Photon.Pun;
using Pieces;

namespace _Scripts.Classes
{
    public class CallbackListener : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
    {
        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            var p = info.photonView.gameObject.GetComponent<Piece>();
            var t = GridManager.GetTileAtPosition(p.transform.position);
            
            //Чекать класс отправителя
            p.Init(t, (Piece.Side)info.Sender.CustomProperties["Side"]);
            BaseObserver.Observe<Knight>();
        }
    }
}