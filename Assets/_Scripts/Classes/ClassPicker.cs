using System;
using _Scripts.Logic;
using Logic;
using Photon.Pun;
using UnityEngine;

namespace _Scripts.Classes
{
    public class ClassPicker : MonoBehaviour
    {
        [SerializeField] private BaseClass classType;
        private GameStarter _gameStarter;

        private void Awake()
        {
            _gameStarter = GetComponentInParent<GameStarter>();
        }

        public void Pick()
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount < 2 && !Debugger.IsDebug)
            {
                _gameStarter.OnStartError();
                return;
            }
            PlayerProps.Player.gameObject.AddComponent(classType.GetType());
            PlayerProps.Player.playerClass = classType.playerClass;
            _gameStarter.StartGame();
        }
    }
}