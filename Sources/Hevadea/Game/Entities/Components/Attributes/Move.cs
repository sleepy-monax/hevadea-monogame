﻿using Hevadea.Framework.Utils;
using Hevadea.Game.Tiles;
using Maker.Rise.Utils;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Hevadea.Game.Entities.Components.Attributes
{
    public class Move : EntityComponent, IEntityComponentUpdatable
    {
        public bool IsMoving { get; private set; }
        public bool NoClip { get; set; } = false;

        public void Update(GameTime gameTime)
        {
            IsMoving = false;
        }

        public void MoveTo(float x, float y, Direction? direction = null, float speed = 1f)
        {
            var dir = new Vector2(x - AttachedEntity.X, y - AttachedEntity.Y);

            
            if (dir.Length() > 1f)
            {
                dir.Normalize();
                dir = dir * speed;
            }

            Do(dir.X, dir.Y, direction ?? dir.ToDirection());
        }

        public void MoveTo(TilePosition tilePosition, Direction? direction = null, float speed = 1f)
        {
            var destination = tilePosition.GetCenter();
            MoveTo(destination.X, destination.Y, direction, speed);
        }


        public bool Do(float sx, float sy, Direction facing)
        {
            if (AttachedEntity.Removed) return false;

            if (AttachedEntity.Level.GetTile(AttachedEntity.GetTilePosition()).HasTag<Tags.Ground>())
            {
                var ground = AttachedEntity.Level.GetTile(AttachedEntity.GetTilePosition()).Tag<Tags.Ground>();

                sx *= ground.MoveSpeed;
                sy *= ground.MoveSpeed;
            }

            // Stop the entity on world borders.
            if (AttachedEntity.X + sx >= AttachedEntity.Level.Width * ConstVal.TileSize) sx = 0;
            if (AttachedEntity.Y + sy >= AttachedEntity.Level.Height * ConstVal.TileSize) sy = 0;
            if (AttachedEntity.X + sx < 0) sx = 0;
            if (AttachedEntity.Y + sy < 0) sy = 0;

            var level = AttachedEntity.Level;
            var ownerColider = AttachedEntity.Get<Colider>();

            if (ownerColider != null)
            {
                var ownerhitbox = ownerColider.GetHitBox();
                var futurPositionX = new RectangleF(ownerhitbox.X + sx, ownerhitbox.Y, ownerhitbox.Width, ownerhitbox.Height);
                var futurPositionY = new RectangleF(ownerhitbox.X, ownerhitbox.Y + sy, ownerhitbox.Width, ownerhitbox.Height);
                var futurPosition = new RectangleF(ownerhitbox.X + sx, ownerhitbox.Y + sy, ownerhitbox.Width, ownerhitbox.Height);

                var colidingEntity = new HashSet<Entity>();

                colidingEntity.UnionWith(level.GetEntitiesOnArea(futurPosition));
                colidingEntity.UnionWith(level.GetEntitiesOnArea(futurPositionX));
                colidingEntity.UnionWith(level.GetEntitiesOnArea(futurPositionY));

                foreach (var e in colidingEntity)
                {
                    var eColider = e.Get<Colider>();
                    if (e == AttachedEntity || !(eColider?.CanCollideWith(e) ?? false)) continue;

                    var eHitbox = eColider.GetHitBox();

                    if (Colision.Check(eHitbox.X, eHitbox.Y, eHitbox.Width, eHitbox.Height, ownerhitbox.X, ownerhitbox.Y + sy, ownerhitbox.Width, ownerhitbox.Height)) { e.Get<Pushable>()?.Push(AttachedEntity, 0f, sy); IsMoving = true; }
                    eHitbox = eColider.GetHitBox();
                    if (Colision.Check(eHitbox.X, eHitbox.Y, eHitbox.Width, eHitbox.Height, ownerhitbox.X, ownerhitbox.Y + sy, ownerhitbox.Width, ownerhitbox.Height)) { sy = 0; }

                    if (Colision.Check(eHitbox.X, eHitbox.Y, eHitbox.Width, eHitbox.Height, ownerhitbox.X + sx, ownerhitbox.Y, ownerhitbox.Width, ownerhitbox.Height)) { e.Get<Pushable>()?.Push(AttachedEntity, sx, 0f); IsMoving = true;}
                    eHitbox = eColider.GetHitBox();
                    if (Colision.Check(eHitbox.X, eHitbox.Y, eHitbox.Width, eHitbox.Height, ownerhitbox.X + sx, ownerhitbox.Y, ownerhitbox.Width, ownerhitbox.Height)) { sx = 0; }
                }

                var entityTilePosition = AttachedEntity.GetTilePosition();

                for (var x = -1; x <= 1; x++)
                {
                    for (var y = -1; y <= 1; y++)
                    {
                        var tile = new TilePosition(entityTilePosition.X + x, entityTilePosition.Y + y);
                        var isPassableTile = level.GetTile(tile).Tag<Tags.Solide>()?.CanPassThrought(AttachedEntity) ?? true;

                        if (!isPassableTile)
                        {
                            var tileRect = tile.ToRectangle();
                            if (Colision.Check(tileRect.X, tileRect.Y, tileRect.Width, tileRect.Height, ownerhitbox.X, ownerhitbox.Y + sy, ownerhitbox.Width, ownerhitbox.Height)) { sy = 0; }
                            if (Colision.Check(tileRect.X, tileRect.Y, tileRect.Width, tileRect.Height, ownerhitbox.X + sx, ownerhitbox.Y, ownerhitbox.Width, ownerhitbox.Height)) { sx = 0; }
                        }
                    }
                }
            }

            if (sx != 0 || sy != 0)
            {
                AttachedEntity.SetPosition(AttachedEntity.X + sx, AttachedEntity.Y + sy);
                AttachedEntity.Level.GetTile(AttachedEntity.GetTilePosition()).Tag<Tags.Ground>()?.SteppedOn(AttachedEntity, AttachedEntity.GetTilePosition());
                AttachedEntity.Facing = facing;
                IsMoving = true;
                return true;
            }

            return false;
        }
    }
}