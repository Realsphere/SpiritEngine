using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Realsphere.Spirit.Mathematics;
using Realsphere.Spirit.SceneManagement;
using Realsphere.Spirit.RenderingCommon;
using Realsphere.Spirit.Internal;

namespace Realsphere.Spirit.Rendering
{
    public abstract class SRenderHook
    {
        /// <summary>
        /// Gets called everytime when Spirit renders.
        /// </summary>
        /// <param name="dxDevice">The DirectX Device Pointer</param>
        /// <param name="dxContext">The DirectX Context Pointer</param>
        /// <param name="scene">The Active Scene</param>
        /// <param name="playerPosition">The Player Position</param>
        /// <param name="mousePosition">The Mouse Position</param>
        public abstract void OnRender(IntPtr d2dDevice, IntPtr d2dContext, IntPtr d3dDevice, IntPtr d3dContext, Scene scene, SVector3 playerPosition, SVector2 mousePosition);
    }

    public static class SRenderHookRegister
    {
        internal static List<SRenderHook> Hooks = new();
        internal static bool dontCall = false;

        public static void RegisterRenderHook(SRenderHook hook)
        {
            dontCall = true;
            Hooks.Add(hook);
            dontCall = false;
        }

        public static void UnregisterRenderHook(SRenderHook hook)
        {
            dontCall = true;
            Hooks.Remove(hook);
            dontCall = false;
        }
    }
}