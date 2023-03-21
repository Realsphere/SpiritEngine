﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using Matrix = SharpDX.Matrix;
using TextAntialiasMode = SharpDX.Direct2D1.TextAntialiasMode;
using System.Runtime.InteropServices;
using Realsphere.Spirit.SceneManagement;

namespace Realsphere.Spirit.RenderingCommon
{
    /// <summary>
    /// Renders text using Direct2D to the back buffer
    /// </summary>
    internal class TextRenderer : RendererBase
    {
        TextFormat textFormat;
        Brush sceneColorBrush;
        protected string font;
        protected Color4 color;
        protected int lineLength;
        DisposeCollector dc;

        /// <summary>
        /// Initializes a new instance of <see cref="TextRenderer"/> class.
        /// </summary>
        internal TextRenderer(string font, Color4 color, Point location, int size = 16, int lineLength = 500)
            : base()
        {
            dc = new();
            if (!String.IsNullOrEmpty(font))
                this.font = font;
            else
                this.font = "Calibri";

            this.color = color;
            this.Location = location;
            this.Size = size;
            this.lineLength = lineLength;
        }

        internal int Size { get; set; }
        internal string Text { get; set; }
        internal Point Location { get; set; }

        /// <summary>
        /// Create any device resources
        /// </summary>
        protected override void CreateDeviceDependentResources()
        {
            base.CreateDeviceDependentResources();

            dc.RemoveAndDispose(ref sceneColorBrush);
            dc.RemoveAndDispose(ref textFormat);

            sceneColorBrush = dc.Collect(new SolidColorBrush(this.DeviceManager.Direct2DContext, this.color));
            textFormat = dc.Collect(new TextFormat(this.DeviceManager.DirectWriteFactory, font, Size) { TextAlignment = TextAlignment.Leading, ParagraphAlignment = ParagraphAlignment.Center });

            this.DeviceManager.Direct2DContext.TextAntialiasMode = TextAntialiasMode.Grayscale;
        }

        /// <summary>
        /// Render
        /// </summary>
        /// <param name="target">The target to render to (the same device manager must be used in both)</param>
        protected override void DoRender(GameObject obj)
        {
            if (String.IsNullOrEmpty(Text))
                return;

            var context2D = DeviceManager.Direct2DContext;

            context2D.BeginDraw();
            context2D.Transform = Matrix3x2.Identity;
            context2D.DrawText(Text, textFormat, new RectangleF(Location.X, Location.Y, Location.X + lineLength, Location.Y + 16), sceneColorBrush);
            context2D.EndDraw();
        }

        public override void Dispose()
        {
            dc.Dispose();
        }
    }
}
