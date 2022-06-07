using System.Threading.Tasks;
using DG.Tweening;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace _Scripts.Multiplayer
{
    public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
    {
        [SerializeField] private TMP_InputField createInput;
        [SerializeField] private TMP_InputField joinInput;
        [SerializeField] private TextMeshProUGUI errorTmp;

        public void CreateRoom()
        {
            RoomOptions roomOptions = new RoomOptions {MaxPlayers = 2};
            PhotonNetwork.CreateRoom(createInput.text, roomOptions);
        }

        public void JoinRoom()
        {
            PhotonNetwork.JoinRoom(joinInput.text);
        }
        public void JoinRandomRoom()
        {
            PhotonNetwork.JoinRandomRoom();
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            base.OnCreateRoomFailed(returnCode, message);
            OnFailed(message);
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            base.OnJoinRoomFailed(returnCode, message);
            OnFailed(message);
        }
    
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            base.OnJoinRandomFailed(returnCode, message);
            OnFailed(message);
        }

        private void OnFailed(string message)
        {
            errorTmp.SetText(message);
            DOTween.Sequence().Append(errorTmp.DOFade(1, 1)).
                AppendInterval(5).Append(errorTmp.DOFade(0, 1));
        }

        public override void OnJoinedRoom()
        {
            PhotonNetwork.LoadLevel("Game");
        }
    }
}
