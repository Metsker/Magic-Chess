using System;
using _Scripts.Logic;
using DG.Tweening;
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace _Scripts.Classes
{
    public class GameStarter : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private GameObject pickMenu;
        [SerializeField] private GameObject board;
        [SerializeField] private TextMeshProUGUI errorTxt;
        [SerializeField] private TextMeshProUGUI className;

        private const float fadeTime = 2;
        public static event Action OnGameStart;
        
        public void StartGame()
        {
            pickMenu.SetActive(false);
            board.SetActive(true);
            
            className.SetText(PlayerProps.Player.playerClass.ToString());
            className.gameObject.SetActive(true);

            OnGameStart?.Invoke();
        }

        public void OnStartError()
        {
            var interval = 1;
            DOTween.Sequence()
                .Append(errorTxt.DOFade(1, 1))
                .AppendInterval(interval).Append(errorTxt.DOFade(0, fadeTime - interval));
        }
        
    }
}