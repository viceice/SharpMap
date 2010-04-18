﻿using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ProjNet.CoordinateSystems.Transformations;
using SharpMap.Data;
using SharpMap.Data.Providers;
using SharpMap.Styles;

namespace SharpMap.Rendering
{
    public class GdiRenderer : IRenderer
    {
        Graphics graphics;
        Image image;

        public Graphics Graphics
        {
            set { graphics = value; }
        }

        public GdiRenderer()
        {
        }

        public void Render(IView view, IProvider DataSource, Func<IFeature, IStyle> getStyle, ICoordinateTransformation coordinateTransformation)
        {
            if (graphics == null) throw new ApplicationException("Graphics was not initialized");
            RendererHelper.Render(graphics, DataSource, getStyle, coordinateTransformation, view);
        }

        private void Initialize(IView view)
        {
            image = new System.Drawing.Bitmap((int)view.Width, (int)view.Height);
            graphics = System.Drawing.Graphics.FromImage(image);
        }

        public Image GetMapAsImage(IView view, Map map)
        {
            Initialize(view); //TODO: only initilize when needed
            graphics.Clear(map.BackColor.Convert());
            graphics.PageUnit = System.Drawing.GraphicsUnit.Pixel;
            map.Render(this, view);
            return image;
        }

        public byte[] GetMapAsByteArray(IView view, Map map)
        {
            Image image = GetMapAsImage(view, map);
            MemoryStream memoryStream = new MemoryStream();
            image.Save(memoryStream, ImageFormat.Bmp);
            return memoryStream.ToArray();
        }
    }
}
