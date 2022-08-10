using UnityEngine;

namespace Assets.Scripts
{
    public abstract class GameBehaviour : MonoBehaviour
    {

        public virtual bool GameUpdate() => true;

        public abstract void Recycle();
      
    }
}
