using Realsphere.Spirit.Rendering;
using Realsphere.Spirit.RenderingCommon;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Buffer = SharpDX.Direct3D11.Buffer;
using SamplerState = SharpDX.Direct3D11.SamplerState;
using VertexBufferBinding = SharpDX.Direct3D11.VertexBufferBinding;

namespace Realsphere.Spirit.Internal
{
    internal class MeshRenderer : RendererBase
    {
        // The vertex buffer
        List<Buffer> vertexBuffers = new List<Buffer>();
        // The index buffer
        List<Buffer> indexBuffers = new List<Buffer>();
        // Texture resources
        internal List<ShaderResourceView> textureViews = new List<ShaderResourceView>();

        internal SMaterial mat;

        Mesh mesh;
        // Provide access to the underlying mesh object
        public Mesh Mesh { get { return mesh; } }

        // The per material buffer to use so that the mesh parameters can be used
        public Buffer PerMaterialBuffer { get; set; }

        public MeshRenderer(Mesh mesh)
        {
            this.mesh = mesh;
        }

        protected override void CreateDeviceDependentResources()
        {
            // Dispose of each vertex, index buffer and texture
            vertexBuffers.ForEach(vb => RemoveAndDispose(ref vb));
            vertexBuffers.Clear();
            indexBuffers.ForEach(ib => RemoveAndDispose(ref ib));
            indexBuffers.Clear();
            textureViews.ForEach(tv => RemoveAndDispose(ref tv));
            textureViews.Clear();
            RemoveAndDispose(ref samplerState);

            // Retrieve our SharpDX.Direct3D11.Device1 instance
            var device = DeviceManager.Direct3DDevice;

            // Initialize vertex buffers
            for (int indx = 0; indx < mesh.VertexBuffers.Count; indx++)
            {
                var vb = mesh.VertexBuffers[indx];
                Vertex[] vertices = new Vertex[vb.Length];
                for (var i = 0; i < vb.Length; i++)
                {
                    // Create vertex
                    vertices[i] = new Vertex(vb[i].Position, vb[i].Normal, vb[i].Color, vb[i].UV);
                }

                vertexBuffers.Add(ToDispose(Buffer.Create(device, BindFlags.VertexBuffer, vertices.ToArray())));
                vertexBuffers[vertexBuffers.Count - 1].DebugName = "VertexBuffer_" + indx.ToString();
            }

            // Initialize index buffers
            foreach (var ib in mesh.IndexBuffers)
            {
                indexBuffers.Add(ToDispose(Buffer.Create(device, BindFlags.IndexBuffer, ib)));
                indexBuffers[indexBuffers.Count - 1].DebugName = "IndexBuffer_" + (indexBuffers.Count - 1).ToString();
            }

            // Load textures if a material has any.
            // The CMO file format supports up to 8 per material
            foreach (var m in mesh.Materials)
            {
                // Diffuse Color
                for (var i = 0; i < m.Textures.Length; i++)
                {
                    if (SharpDX.IO.NativeFile.Exists(m.Textures[i]))
                        textureViews.Add(ToDispose(ShaderResourceView.FromFile(device, m.Textures[i])));
                    else
                        textureViews.Add(null);
                }
            }

            // Create our sampler state
            samplerState = ToDispose(new SamplerState(device, new SamplerStateDescription()
            {
                AddressU = TextureAddressMode.Clamp,
                AddressV = TextureAddressMode.Clamp,
                AddressW = TextureAddressMode.Clamp,
                BorderColor = new Color4(0, 0, 0, 0),
                ComparisonFunction = Comparison.Never,
                Filter = Filter.MinMagMipLinear,
                MaximumAnisotropy = 16,
                MaximumLod = float.MaxValue,
                MinimumLod = 0,
                MipLodBias = 0.0f
            }));
        }

        protected override void DoRender()
        {
            if (Game.app.pauseRendering) return;

            // Retrieve device context
            var context = DeviceManager.Direct3DContext;

            // Draw sub-meshes grouped by material
            for (var mIndx = 0; mIndx < mesh.Materials.Count; mIndx++)
            {
                if (Game.app.pauseRendering) return;
                var subMeshesForMaterial =
                    (from sm in mesh.SubMeshes
                     where sm.MaterialIndex == mIndx
                     select sm).ToArray();

                // If the material buffer is available and there are submeshes
                // using the material update the PerMaterialBuffer

                if (PerMaterialBuffer != null && subMeshesForMaterial.Length > 0)
                {
                    // update the PerMaterialBuffer constant buffer
                    var material = new ConstantBuffers.PerMaterial()
                    {
                        Ambient = mat.Ambient.sharpdxcolor,
                        Diffuse = mat.Diffuse.sharpdxcolor,
                        Emissive = mat.Emissive.sharpdxcolor,
                        Specular = mat.Specular.sharpdxcolor,
                        SpecularPower = mat.SpecularPower,
                        UVTransform = mat.UVs.Length == 0 ? mIndx < mat.UVs.Length ? mat.UVs[mIndx].sharpDXVector : new Vector2() : new Vector2()
                    };

                    // Bind textures to the pixel shader
                    int texIndxOffset = mIndx * Mesh.MaxTextures;
                    material.HasTexture = (uint)(texture == null ? 0 : 1);
                    if (Game.app.pauseRendering) return;
                    if (material.HasTexture == 1) context.PixelShader.SetShaderResource(0, texture);

                    // Set texture sampler state
                    context.PixelShader.SetSampler(0, samplerState);

                    // Update material buffer
                    context.UpdateSubresource(ref material, PerMaterialBuffer);
                }

                // For each sub-mesh
                foreach (var subMesh in subMeshesForMaterial)
                {
                    if (Game.app.pauseRendering) return;
                    // Ensure the vertex buffer and index buffers are in range
                    if (subMesh.VertexBufferIndex < vertexBuffers.Count && subMesh.IndexBufferIndex < indexBuffers.Count)
                    {
                        // Retrieve and set the vertex and index buffers
                        var vertexBuffer = vertexBuffers[(int)subMesh.VertexBufferIndex];
                        context.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(vertexBuffer, Utilities.SizeOf<Vertex>(), 0));
                        context.InputAssembler.SetIndexBuffer(indexBuffers[(int)subMesh.IndexBufferIndex], Format.R16_UInt, 0);
                        // Set topology
                        context.InputAssembler.PrimitiveTopology = SharpDX.Direct3D.PrimitiveTopology.TriangleList;
                    }

                    // Draw the sub-mesh (includes Primitive count which we multiply by 3)
                    // The submesh also includes a start index into the vertex buffer
                    context.DrawIndexed((int)subMesh.PrimCount * 3, (int)subMesh.StartIndex, 0);
                }
            }
        }
    }
}
