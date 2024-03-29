﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Realsphere.Spirit.Physics;
using Realsphere.Spirit.Rendering;

namespace Realsphere.Spirit.SceneManagement
{
    public class Scene
    {
        public List<GameObject> GameObjects = new List<GameObject>();
        public List<Trigger> Triggers = new List<Trigger>();
        public SColor SkyBoxColor;
        public int SubStepCount;
        //public List<Light> Lights;
        public DirectionalLight Light;

        public GameObject GetByName(string s)
        {
            foreach (GameObject go in GameObjects)
                if (go.Name == s) return go;
            return null;
        }

        public Scene()
        {
            SkyBoxColor = new SColor(0, 0, 0, 1f);
            SubStepCount = Environment.ProcessorCount / 2;
            Light = new();
        }
    }
}
