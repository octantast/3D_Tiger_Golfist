using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace VegetablesScripts.VegetablesWorld
{
    public class VegCherryVine:MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public string VegConcatenateStrings(List<string> StrawberryStrings)
        {
            StringBuilder RaspberryBuilder = new StringBuilder();
            foreach (var str in StrawberryStrings)
            {
                RaspberryBuilder.Append(str);
            }

            return RaspberryBuilder.ToString();
        }
    }
}