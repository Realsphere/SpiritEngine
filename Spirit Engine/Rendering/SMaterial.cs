using Realsphere.Spirit.Mathematics;
using SharpDX;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realsphere.Spirit.Rendering
{
    public class SMaterial
    {
        public SColor Ambient = SColor.fromsharpdx(new Color4(0.2f));
        public SColor Diffuse = new SColor(255f, 255f, 255f, 1f);
        public SColor Specular = new SColor(255f, 255f, 255f, 1f);
        public SColor Emissive = SColor.fromsharpdx(new Color4(0f));
        public float SpecularPower = 20f;
        public STexture Texture = null;
        public SVector2[] UVs = new SVector2[0];

        /// <summary>
        /// Creates a white material.
        /// </summary>
        public static SMaterial Create()
        {
            return new SMaterial()
            {
                Ambient = SColor.fromsharpdx(new Color4(255f)),
                Diffuse = SColor.fromsharpdx(new Color4(255f)),
                Emissive = SColor.fromsharpdx(new Color4(255f)),
                Specular = SColor.fromsharpdx(new Color4(255f)),
                SpecularPower = 20f
            };
        }

        /// <summary>
        /// Creates a material with a texture.
        /// </summary>
        public static SMaterial Create(STexture texture)
        {
            return new SMaterial()
            {
                Ambient = SColor.fromsharpdx(new Color4(255f)),
                Diffuse = SColor.fromsharpdx(new Color4(255f)),
                Emissive = SColor.fromsharpdx(new Color4(255f)),
                Specular = SColor.fromsharpdx(new Color4(0f)),
                SpecularPower = 20f,
                Texture = texture
            };
        }

        public static SMaterial Create(SColor ambient)
        {
            return new SMaterial()
            {
                Ambient = ambient,
                Diffuse = new SColor(255f, 255f, 255f, 1f),
                Emissive = SColor.fromsharpdx(new Color4(0f)),
                Specular = new SColor(255f, 255f, 255f, 1f),
                SpecularPower = 20f
            };
        }

        public static SMaterial Create(SColor ambient, SColor diffuse)
        {
            return new SMaterial()
            {
                Ambient = ambient,
                Diffuse = diffuse,
                Emissive = SColor.fromsharpdx(new Color4(0f)),
                Specular = new SColor(255f, 255f, 255f, 1f),
                SpecularPower = 20f
            };
        }

        public static SMaterial Create(SColor ambient, SColor diffuse, SColor emissive)
        {
            return new SMaterial()
            {
                Ambient = ambient,
                Diffuse = diffuse,
                Emissive = emissive,
                Specular = new SColor(255f, 255f, 255f, 1f),
                SpecularPower = 20f
            };
        }

        public static SMaterial Create(SColor ambient, SColor diffuse, SColor emissive, SColor specular)
        {
            return new SMaterial()
            {
                Ambient = ambient,
                Diffuse = diffuse,
                Emissive = emissive,
                Specular = specular,
                SpecularPower = 20f
            };
        }

        public static SMaterial Create(SColor ambient, SColor diffuse, SColor emissive, SColor specular, float specularPower)
        {
            return new SMaterial()
            {
                Ambient = ambient,
                Diffuse = diffuse,
                Emissive = emissive,
                Specular = specular,
                SpecularPower = specularPower
            };
        }
    }
}
