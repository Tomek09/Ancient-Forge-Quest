using System.Linq;
using UnityEngine;

namespace AncientForgeQuest.Utility
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance = null;

        public static T Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                var array = Resources.FindObjectsOfTypeAll<T>();
                return (from t in array where t.gameObject.scene.name != null select _instance = t).FirstOrDefault();

            }
        }

        private void Awake()
        {
            _instance = this as T;
            OnAwake();
        }

        protected virtual void OnAwake()
        {
        }
    }
}
