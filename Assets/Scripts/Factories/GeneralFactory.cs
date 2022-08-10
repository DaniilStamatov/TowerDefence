using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public abstract class GeneralFactory : ScriptableObject
    {
        private Scene _scene;

        protected T MoveToScene<T>(T go) where T : MonoBehaviour
        {
            if (!_scene.isLoaded)
            {
                if (Application.isEditor)
                {
                    _scene = SceneManager.GetSceneByName(name);
                    if (!_scene.isLoaded)
                    {
                        _scene = SceneManager.CreateScene(name);
                    }
                }
                else
                {
                    _scene = SceneManager.CreateScene(name);
                }
            }
            T instance = Instantiate(go);
            SceneManager.MoveGameObjectToScene(instance.gameObject, _scene);
            return instance;
        }
    }
}
