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
        public Light Light;

        public GameObject GetByName(string s)
        {
            foreach (GameObject go in GameObjects)
                if (go.Name == s) return go;
            return null;
        }

        public Scene()
        {
            SkyBoxColor = new SColor(0, 0, 0, 1f);
            Light = new Light();
            SubStepCount = Environment.ProcessorCount / 2;
        }
    }
}
