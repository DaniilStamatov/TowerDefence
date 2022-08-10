using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    [Serializable]
    public class GameBehaviourCollection
    {
        private List<GameBehaviour> _behaviours = new List<GameBehaviour>();


        public bool IsEmpty => _behaviours.Count == 0;
        public void Add(GameBehaviour behaviour)
        {
            _behaviours.Add(behaviour);
        }

        public void GameUpdate()
        {
            for (int i = 0; i < _behaviours.Count; i++)
            {
                if (!_behaviours[i].GameUpdate())
                {
                    var lastIndex = _behaviours.Count - 1;
                    _behaviours[i] = _behaviours[lastIndex];
                    _behaviours.RemoveAt(lastIndex);
                    i -= 1;
                }
            }
        }

        public void Clear()
        {
            foreach (var behaviour in _behaviours)
            {
                behaviour.Recycle();
            }
            _behaviours.Clear();
        }
    }
}
