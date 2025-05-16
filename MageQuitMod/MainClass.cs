using Newtonsoft.Json;
using SRDebugger.Internal;
using SRDebugger.Services;
using SRDebugger.Services.Implementation;
using SRF.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace MageQuitMod
{
    class MainClass : MonoBehaviour
    {
        public SRDebugWrapper.SRDebugWrapper DebugWrapper;

        public void Start()
        {

        }

        public void Update()
        {
        }

        public void OnGUI()
        {
            if(GUI.Button(new Rect(new Vector2(20, 20), new Vector2(100, 20)), "Unload"))
                Loader.Unload();

            if (GUI.Button(new Rect(new Vector2(20, 40), new Vector2(100, 20)), "Test"))
            {
                DebugWrapper = new SRDebugWrapper.SRDebugWrapper();
            }
        }
    }
}
