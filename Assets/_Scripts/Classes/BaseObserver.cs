using System.Collections.Generic;
using System.Linq;
using _Scripts.Logic;
using Logic;

namespace _Scripts.Classes
{
    public abstract class BaseObserver : BaseClass
    {
        protected static readonly List<Piece> ObserveList = new ();
        
        private void OnEnable()
        {
            GridManager.OnTurnEnd += OnTurnEnd;
        }
        private void OnDisable()
        {
            GridManager.OnTurnEnd -= OnTurnEnd;
        }

        
        public static void Observe<T>() where T : Piece
        {
            if (!PlayerProps.Player.TryGetComponent<BaseObserver>(out _)) return;
            
            foreach (var k in FindObjectsOfType<T>().Where(k => k.side == PlayerProps.side && !ObserveList.Contains(k)))
            {
                ObserveList.Add(k);
            }
        }
        
        protected abstract void OnTurnEnd();
    }
}