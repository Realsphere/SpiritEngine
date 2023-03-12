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

        Mesh mesh;
        // Provide access to the underlying mesh object
        public Mesh Mesh { get { return mesh; } }

        // The per material buffer to use so that the mesh parameters can be used
        public Buffer PerMaterialBuffer { get; set; }
        // The per armature constant buffer to use
        public Buffer PerArmatureBuffer { get { return Game.app.perArmatureBuffer; } }

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

        // Create and allow access to a timer
        System.Diagnostics.Stopwatch clock = new System.Diagnostics.Stopwatch();
        public System.Diagnostics.Stopwatch Clock
        {
            get { return clock; }
            set { clock = value; }
        }

        // The currently active animation (allows nulls)
        public Mesh.Animation? CurrentAnimation { get; set; }

        // Play once or loop the animation?
        public bool PlayOnce { get; set; }

        protected override void DoRender()
        {
            if (Game.app.pauseRendering) return;

            // Calculate elapsed seconds
            var time = clock.ElapsedMilliseconds / 1000.0f;

            // Retrieve device context
            var context = DeviceManager.Direct3DContext;

            if(CurrentAnimation != null)
            {

                // Calculate skin matrices for each bone
                ConstantBuffers.PerArmature skinMatrices = new ConstantBuffers.PerArmature();
                if (mesh.Bones != null)
                {
                    // Retrieve each bone's local transform
                    for (var i = 0; i < mesh.Bones.Count; i++)
                    {
                        skinMatrices.Bones[i] = mesh.Bones[i].BoneLocalTransform;
                    }

                    // Load bone transforms from animation frames
                    if (CurrentAnimation.HasValue)
                    {
                        // Keep track of the last key-frame used for each bone
                        Mesh.Keyframe?[] lastKeyForBones = new Mesh.Keyframe?[mesh.Bones.Count];
                        // Keep track of whether a bone has been interpolated
                        bool[] lerpedBones = new bool[mesh.Bones.Count];
                        for (var i = 0; i < CurrentAnimation.Value.Keyframes.Count; i++)
                        {
                            // Retrieve current key-frame
                            var frame = CurrentAnimation.Value.Keyframes[i];

                            // If the current frame is not in the future
                            if (frame.Time <= time)
                            {
                                // Keep track of last key-frame for bone
                                lastKeyForBones[frame.BoneIndex] = frame;
                                // Retrieve transform from current key-frame
                                skinMatrices.Bones[frame.BoneIndex] = frame.Transform;
                            }
                            // Frame is in the future, check if we should interpolate
                            else
                            {
                                // Only interpolate a bone's key-frames ONCE
                                if (!lerpedBones[frame.BoneIndex])
                                {
                                    // Retrieve the previous key-frame if exists
                                    Mesh.Keyframe prevFrame;
                                    if (lastKeyForBones[frame.BoneIndex] != null)
                                        prevFrame = lastKeyForBones[frame.BoneIndex].Value;
                                    else
                                        continue; // nothing to interpolate
                                                  // Make sure we only interpolate with 
                                                  // one future frame for this bone
                                    lerpedBones[frame.BoneIndex] = true;

                                    // Calculate time difference between frames
                                    var frameLength = frame.Time - prevFrame.Time;
                                    var timeDiff = time - prevFrame.Time;
                                    var amount = timeDiff / frameLength;

                                    // Interpolation using Lerp on scale and translation, and Slerp on Rotation (Quaternion)
                                    Vector3 t1, t2;   // Translation
                                    Quaternion q1, q2;// Rotation
                                    float s1, s2;     // Scale
                                                      // Decompose the previous key-frame's transform
                                    prevFrame.Transform.DecomposeUniformScale(out s1, out q1, out t1);
                                    // Decompose the current key-frame's transform
                                    frame.Transform.DecomposeUniformScale(out s2, out q2, out t2);

                                    // Perform interpolation and reconstitute matrix
                                    skinMatrices.Bones[frame.BoneIndex] =
                                        Matrix.Scaling(MathUtil.Lerp(s1, s2, amount)) *
                                        Matrix.RotationQuaternion(Quaternion.Slerp(q1, q2, amount)) *
                                        Matrix.Translation(Vector3.Lerp(t1, t2, amount));
                                }
                            }

                        }
                    }

                    // Apply parent bone transforms
                    // We assume here that the first bone has no parent
                    // and that each parent bone appears before children
                    for (var i = 1; i < mesh.Bones.Count; i++)
                    {
                        var bone = mesh.Bones[i];
                        if (bone.ParentIndex > -1)
                        {
                            var parentTransform = skinMatrices.Bones[bone.ParentIndex];
                            skinMatrices.Bones[i] = (skinMatrices.Bones[i] * parentTransform);
                        }
                    }

                    // Change the bone transform from rest pose space into bone space (using the inverse of the bind/rest pose)
                    for (var i = 0; i < mesh.Bones.Count; i++)
                    {
                        skinMatrices.Bones[i] = Matrix.Transpose(mesh.Bones[i].InvBindPose * skinMatrices.Bones[i]);
                    }

                    // Check need to loop animation
                    if (!PlayOnce && CurrentAnimation.HasValue && CurrentAnimation.Value.EndTime <= time)
                    {
                        this.Clock.Restart();
                    }
                }

                // Update the constant buffer with the skin matrices for each bone
                context.UpdateSubresource(skinMatrices.Bones, PerArmatureBuffer);
            }

            // Draw sub-meshes grouped by material
            for (var mIndx = 0; mIndx < mesh.Materials.Count; mIndx++)
            {
                if (Game.app.pauseRendering) return;
                var subMeshesForMaterial =
                    (from sm in mesh.SubMeshes
                     where sm.MaterialIndex == mIndx
                     select sm).ToArray();


                // For each sub-mesh
                foreach (var subMesh in subMeshesForMaterial)
                {
                    // If the material buffer is available and there are submeshes
                    // using the material update the PerMaterialBuffer

                    if (PerMaterialBuffer != null && subMeshesForMaterial.Length > 0)
                    {
                        ConstantBuffers.PerMaterial material;
                        // update the PerMaterialBuffer constant buffer
                        if (objectOn.Materials != null && objectOn.Materials.Length > 0)
                        {
                            material = new ConstantBuffers.PerMaterial()
                            {
                                Ambient = objectOn.getByIndex(mIndx).Ambient.sharpdxcolor,
                                Diffuse = objectOn.getByIndex(mIndx).Diffuse.sharpdxcolor,
                                Emissive = objectOn.getByIndex(mIndx).Emissive.sharpdxcolor,
                                Specular = objectOn.getByIndex(mIndx).Specular.sharpdxcolor,
                                SpecularPower = objectOn.getByIndex(mIndx).SpecularPower,
                                HasTexture = objectOn.getByIndex(mIndx).Texture == null ? (uint)0 : (uint)1
                            };
                        }
                        else
                        {
                            material = new ConstantBuffers.PerMaterial()
                            {
                                Ambient = new(mesh.Materials[(int)subMesh.MaterialIndex].Ambient),
                                Diffuse = new(mesh.Materials[(int)subMesh.MaterialIndex].Diffuse),
                                Emissive = new(mesh.Materials[(int)subMesh.MaterialIndex].Emissive),
                                Specular = new(mesh.Materials[(int)subMesh.MaterialIndex].Specular),
                                SpecularPower = mesh.Materials[(int)subMesh.MaterialIndex].SpecularPower,
                                HasTexture = mesh.Materials[(int)subMesh.MaterialIndex].Textures.Length == 0 ? (uint)0 : (uint)1
                            };
                        }

                        if (objectOn.Materials != null && objectOn.Materials.Length > 0)
                        {
                            // Bind textures to the pixel shader
                            int texIndxOffset = mIndx * Mesh.MaxTextures;
                            material.HasTexture = (uint)(objectOn.getByIndex(mIndx).Texture == null || objectOn.getByIndex(mIndx).Texture.texture == null ? 0 : 1);
                            if (Game.app.pauseRendering) return;
                            if (material.HasTexture == 1) context.PixelShader.SetShaderResource(0, objectOn.getByIndex(mIndx).Texture.texture);

                            // Set texture sampler state
                            context.PixelShader.SetSampler(0, samplerState);
                        }

                        // Update material buffer
                        context.UpdateSubresource(ref material, PerMaterialBuffer);
                    }

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