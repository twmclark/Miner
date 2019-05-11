using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MiningGame.Components;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace MiningGame.Systems
{
    public class PlayerControlSystem: EntityProcessingSystem
    {
        public PlayerControlSystem() : base(Aspect.All(typeof(MoveablePositionComponent)))
        {

        }

        public override void Initialize(IComponentMapperService mapperService)
        {
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            var entity = GetEntity(entityId);
            var movementComponent = entity.Get<MoveablePositionComponent>();

            var keyboard = Keyboard.GetState();
            var direction = Vector2.Zero;

            if (keyboard.IsKeyDown(Keys.W) || keyboard.IsKeyDown(Keys.Up))
                direction -= Vector2.UnitY;

            if (keyboard.IsKeyDown(Keys.A) || keyboard.IsKeyDown(Keys.Left))
                direction -= Vector2.UnitX;

            if (keyboard.IsKeyDown(Keys.S) || keyboard.IsKeyDown(Keys.Down))
                direction += Vector2.UnitY;

            if (keyboard.IsKeyDown(Keys.D) || keyboard.IsKeyDown(Keys.Right))
                direction += Vector2.UnitX;

            var isMoving = direction != Vector2.Zero;
            if (isMoving)
            {
                direction.Normalize();
            }

            var speed = 400;
            movementComponent.Transform.Position += direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}