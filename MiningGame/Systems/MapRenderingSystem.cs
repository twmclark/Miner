using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Entities.Systems;
using MonoGame.Extended.TextureAtlases;

namespace MiningGame.Systems
{
    public class MapRenderingSystem : DrawSystem
    {
        public int Width { get; set; }
        public int Height { get; set; }
        private readonly SpriteBatch _spriteBatch;
        private readonly TextureAtlas _textureAtlas;
        private readonly int[] _tiles;
        private const int TileSize = 32;
        private readonly OrthographicCamera _camera;


        public MapRenderingSystem(int width, int height, SpriteBatch spriteBatch, TextureAtlas textureAtlas, OrthographicCamera camera)
        {
            Width = width;
            Height = height;
            _spriteBatch = spriteBatch;
            _textureAtlas = textureAtlas;
            _camera = camera;
            _tiles = new int[width * height];

            for (int i = 0; i < width * height; i++)
            {
                _tiles[i] = 4;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            var cameraWidthCellCount = (int)Math.Ceiling(_camera.BoundingRectangle.Width) / TileSize;
            var cameraHeightCellCount = (int)Math.Ceiling(_camera.BoundingRectangle.Height) / TileSize;

            var startingHorizontalCell = Clamp((int)Math.Floor(_camera.BoundingRectangle.TopLeft.X / TileSize), 0, cameraWidthCellCount);
            var startingVerticalCell = Clamp((int)Math.Floor(_camera.BoundingRectangle.TopLeft.Y / TileSize), 0, cameraHeightCellCount);
            var viewMatrix = _camera.GetViewMatrix();
            _spriteBatch.Begin(transformMatrix: viewMatrix);

            for (int y = startingVerticalCell; y < cameraHeightCellCount || y < Height-1; y++)
            {
                for (int x = startingHorizontalCell; x < cameraWidthCellCount || x < Width-1; x++)
                {
                    var tile = _tiles[x + y * Width];

                    //var xPos = _camera.WorldToScreen(new Vector2(TileToWorldPositionX(x)));
                    _spriteBatch.Draw(_textureAtlas[tile], new Rectangle(TileToWorldPositionX(x), TileToWorldPositionY(y), TileSize, TileSize), Color.White);
                }

            }
            _spriteBatch.End();
        }


        private static int Clamp(int value, int min, int max)
        {
            return (value < min) ? min : (value > max) ? max : value;
        }

        public int TileToWorldPositionX(int x)
        {
            return x * TileSize;
        }

        public int TileToWorldPositionY(int y)
        {
            return y * TileSize;
        }
    }
}