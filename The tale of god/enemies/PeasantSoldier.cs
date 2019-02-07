using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace TheTaleOfGod.enemies
{
    public class PeasantSoldier : Enemy
    {
        public Gun gun;

        private bool readyToFire;

        float bulletRotationOffset = MathHelper.PiOver2;

        public PeasantSoldier (float speed, float turnSpeed, float maxHealth, float attackRange, float targetRange, Vector2 position, Texture2D sprite, Character target) : base(speed, turnSpeed, maxHealth, attackRange, targetRange, position, sprite, target)
        {
            gun = new Gun(10f, 2f, true, position, new Bullet(10f, BulletType.Normal), "enemy");
        }
        public override void Update(GameTime gameTime)
        {
            readyToFire = true;

            base.Update(gameTime);

            gun.Update(gameTime, position, rotation + bulletRotationOffset, lookDirection);

            if (gun.CanFire() && readyToFire)
            {
                gun.Fire(lookDirection);
            }
        }

        public override void OnTargetBlocked(Collider col, Vector2 point)
        {
            readyToFire = false;

            base.OnTargetBlocked(col, point);
        }

        public override void Draw(SpriteBatch batch)
        {
            gun.Draw(batch);
            base.Draw(batch);
        }
        public override void OnCollisionEnter(Collider colinfo)
        {
            base.OnCollisionEnter(colinfo);
        }
    }
}
