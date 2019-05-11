using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MiningGame.Components;
using MiningGame.Systems;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Entities;
using MonoGame.Extended.TextureAtlases;
using MonoGame.Extended.ViewportAdapters;

namespace MiningGame
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch _spriteBatch;
        //private TextureAtlas spritesAtlas;
        private OrthographicCamera _camera;
        private BitmapFont _bitmapFont;
        private TileLayer _tiles;
        private World _world;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.AllowUserResizing = true;
            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            const int screenWidth = 800;
            const int screenHeight = 480;

            base.Initialize();
            var viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, screenWidth, screenHeight);
            _camera = new OrthographicCamera(viewportAdapter);
            _bitmapFont = Content.Load<BitmapFont>("debugFont");

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            var spritesAtlas = Content.Load<TextureAtlas>("sprites/SpritesData-atlas");

            _world = new WorldBuilder()
                .AddSystem(new MapRenderingSystem(20, 20, _spriteBatch, spritesAtlas, _camera))
                .AddSystem(new PlayerControlSystem())
                .AddSystem(new TextureRenderingSystem(_spriteBatch, _camera))
                .Build();

            InitialisePlayer();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var keyboardState = Keyboard.GetState();
            const float movementSpeed = 300;

            if (keyboardState.IsKeyDown(Keys.Up))
                _camera.Move(new Vector2(0, -movementSpeed) * (float)gameTime.ElapsedGameTime.TotalSeconds);

            if (keyboardState.IsKeyDown(Keys.Down))
                _camera.Move(new Vector2(0, +movementSpeed) * (float)gameTime.ElapsedGameTime.TotalSeconds);

            if (keyboardState.IsKeyDown(Keys.Left))
                _camera.Move(new Vector2(-movementSpeed, 0) * (float)gameTime.ElapsedGameTime.TotalSeconds);

            if (keyboardState.IsKeyDown(Keys.Right))
                _camera.Move(new Vector2(+movementSpeed, 0) * (float)gameTime.ElapsedGameTime.TotalSeconds);

            _world.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            MouseState mouseState = Mouse.GetState();
            var mousePosition = _camera.ScreenToWorld(mouseState.Position.ToVector2());

            var cameraPosition = _camera.Position.X.ToString() + " : " + _camera.Position.Y.ToString();

            var framerate = (1 / gameTime.ElapsedGameTime.TotalSeconds).ToString();

            var viewMatrix = _camera.GetViewMatrix();

            _world.Draw(gameTime);
            base.Draw(gameTime);
            _spriteBatch.Begin();


            //_tiles.Draw(_camera);
            _spriteBatch.DrawString(_bitmapFont, cameraPosition, new Vector2(50, 50), Color.Red);
            _spriteBatch.DrawString(_bitmapFont, framerate, new Vector2(50, 70), Color.Red);
            _spriteBatch.DrawString(_bitmapFont, mousePosition.X.ToString(), new Vector2(50, 90), Color.Red);
            _spriteBatch.DrawString(_bitmapFont, mousePosition.Y.ToString(), new Vector2(50, 110), Color.Red);

            _spriteBatch.End();
        }

        private void InitialisePlayer()
        {
            Texture2D texture = Content.Load<Texture2D>("character");

            var entity = _world.CreateEntity();
            entity.Attach(new MoveablePositionComponent { Transform = new Transform2(0, 0) });
            entity.Attach(new TextureComponent { Texture = texture });
        }
    }
}
