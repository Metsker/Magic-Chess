using System.Linq;
using _Scripts.Logic;
using UnityEngine;

namespace _Scripts.Messages
{
    public class Log : MonoBehaviour
    {
        [SerializeField] private Message[] messages;
        
        private static Message[] _messages;

        private void Start()
        {
            _messages = messages;
        }

        public static void SendMessageToLog(string message)
        {
            var freeM = _messages.FirstOrDefault(m => !m.gameObject.activeSelf);
            if (freeM == default) return;
            freeM.text = message;
            freeM.gameObject.SetActive(true);
        }
    }
}