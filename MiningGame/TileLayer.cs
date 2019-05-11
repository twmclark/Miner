using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.TextureAtlases;

namespace MiningGame
{
    public class TileLayer
    {
        public int Width { get; set; }
        public int Height { get; set; }
        private readonly SpriteBatch _spriteBatch;
        private readonly TextureAtlas _textureAtlas;
        private readonly int[] _tiles;
        private const int TileSize = 32;

        public TileLayer(int width, int height, SpriteBatch spriteBatch, TextureAtlas textureAtlas)
        {
            Width = width;
            Height = height;
            _spriteBatch = spriteBatch;
            _textureAtlas = textureAtlas;

            _tiles = new int[width * height];

            for (int i = 0; i < width * height; i++)
            {
                _tiles[i] = 4;
            }
        }


        public void Draw(OrthographicCamera camera)
        {
            var cameraWidthCellCount = (int)camera.BoundingRectangle.Width / TileSize;
            var cameraHeightCellCount = (int)Math.Ceiling(camera.BoundingRectangle.Height / TileSize);

            var startingHorizontalCell = Clamp((int)Math.Floor(camera.BoundingRectangle.TopLeft.X / Width), 0, cameraWidthCellCount);
            var startingVerticalCell = Clamp((int)Math.Floor(camera.BoundingRectangle.TopLeft.Y / Height), 0, cameraHeightCellCount);




            for (int y = startingVerticalCell; y < cameraHeightCellCount; y++)
            {
                for (int x = startingHorizontalCell; x < cameraWidthCellCount; x++)
                {

                    var tile = _tiles[x + y * Width];
                    _spriteBatch.Draw(_textureAtlas[tile], new Rectangle(TileToWorldPositionX(x), TileToWorldPositionY(y), TileSize, TileSize), Color.White);


                }

            }
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

        //_spriteBatch.Draw(_textureAtlas[3], new Rectangle(0, 0, 32, 32), Color.White);
    }
}