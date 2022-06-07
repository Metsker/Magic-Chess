using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Logic
{
    public class Message : MonoBehaviour
    {
        [SerializeField] private float fadeTime = 2.5f;
        
        public string text { get; set; }
        private TextMeshProUGUI _txt;

        private void Awake()
        {
            _txt = GetComponent<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            _txt.SetText(text);
            float interval = 1;
            
            var log = GetComponentInParent<Image>();
            log.DOFade(0.4f, 1);
            
            DOTween.Sequence()
                .Append(_txt.DOFade(1,1))
                .AppendInterval(interval).
                Append(_txt.DOFade(0, fadeTime - interval).OnComplete(() =>
                {
                    gameObject.SetActive(false);
                    log.DOFade(0, 1);
                }));
        }
    }
}
