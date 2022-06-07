using Photon.Pun;
using UnityEngine;

namespace _Scripts.Multiplayer
{
    public class Disconnect : MonoBehaviour
    {
        public void DsC()
        {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.LoadLevel("Lobby");
        }
    }
}
