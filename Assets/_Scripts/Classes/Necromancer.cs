using System.Collections.Generic;
using Pieces;
using Unity.VisualScripting;

namespace _Scripts.Classes
{
    public class Necromancer : BaseObserver
    {
        private const int defaultResTurns = 3;
        
        protected void Start()
        {
            Observe<Knight>();
        }

        protected override void OnTurnEnd()
        {
            List<int> indexesToRemove = new List<int>();
            foreach (var p in ObserveList)
            {
                if (!p.IsDestroyed() || p.startTile.TryGetComponent<Resurrector>(out _)) continue;

                var r = p.startTile.AddComponent<Resurrector>();
                r.Init(p.startTile, p.pieceType, defaultResTurns);
                
                r.SendMessage();
                indexesToRemove.Add(ObserveList.IndexOf(p));
            }

            foreach (var i in indexesToRemove)
            {
                ObserveList.RemoveAt(i);
            }
        }
    }
}