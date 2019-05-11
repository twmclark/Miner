using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MiningGame.Components;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace MiningGame.Systems
{
    public class TextureRenderingSystem : EntityDrawSystem
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly OrthographicCamera _camera;

        public TextureRenderingSystem(SpriteBatch spriteBatch, OrthographicCamera camera) : base(Aspect.One(typeof(TextureComponent)))
        {
            _spriteBatch = spriteBatch;
            _camera = camera;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
        }

        public override void Draw(GameTime gameTime)
        {

            _spriteBatch.Begin(transformMatrix: _camera.GetViewMatrix());

            foreach (var entityId in ActiveEntities)
            {
                var entity = GetEntity(entityId);
                var texture = entity.Get<TextureComponent>();
                var transform = entity.Get<MoveablePositionComponent>();

                _spriteBatch.Draw(texture.Texture, transform.Transform.Position);
            }
            _spriteBatch.End();
        }
    }
}