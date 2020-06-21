using System;
using Microsoft.Xna.Framework;

namespace GamePrototypeDesktop.Graphics
{
    public class Camera
    {
        public float MaximumZoom;
        public float MinimumZoom;

        public Vector2 Position;
        public Vector2 Origin;
        public float Rotation;
        
        private float _zoom;

        public Camera()
        {
            Position = Vector2.Zero;
            MinimumZoom = 0f;
            MaximumZoom = 10f;
            Rotation = 0f;
            Zoom = 1f;
            OnWindowSizeChanged();
        }
        
        public float Zoom
        {
            get => _zoom;
            set
            {
                if (value >= MinimumZoom && value <= MaximumZoom)
                    _zoom = value;
                else if (value <= MinimumZoom)
                    _zoom = MinimumZoom;
                else
                    _zoom = MaximumZoom;
            }
        }
        
        public void Rotate(float degrees) => Rotation += degrees;

        public void Move(Vector2 directionVector) => Position += directionVector;

        public void ZoomIn(float deltaZoom) => Zoom += deltaZoom;

        public void ZoomOut(float deltaZoom) => Zoom -= deltaZoom;

        public void LookAt(Vector2 position) => Position = position - Origin;

        public void OnWindowSizeChanged()
        {
            Origin = new Vector2(
                GraphicsManager.Instance.VirtualWidth / 2f,
                GraphicsManager.Instance.VirtualHeight / 2f
            );
        }

        //TODO: Optimize view matrix usage to avoid recalculations by using bool flag
        public Vector2 ConvertScreenToVirtual(Vector2 screenPosition)
        {
            var viewport = GraphicsManager.Instance.Viewport;
            return Vector2.Transform(screenPosition - new Vector2(viewport.X, viewport.Y), 
                Matrix.Invert(GetViewMatrix()));
        }
        
        public Matrix GetViewMatrix()
        {
            return 
                Matrix.CreateTranslation(new Vector3(Position, 0f)) *
                Matrix.CreateTranslation(new Vector3(-Origin, 0f)) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(Zoom, Zoom, 1) *
                Matrix.CreateTranslation(new Vector3(Origin, 0f)) *
                GraphicsManager.Instance.GetScaleMatrix();
        }
    }
}