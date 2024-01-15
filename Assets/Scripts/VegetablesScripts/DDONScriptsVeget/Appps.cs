using UnityEngine;

namespace VegetablesScripts.DDONScriptsVeget
{
    public class Appps:MonoBehaviour
    {
        public void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}