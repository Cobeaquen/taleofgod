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

        float bulletRotationOffset = MathHelper.PiOver2;

        public PeasantSoldier (float speed, float turnSpeed, float maxHealth, float attackRange, float targetRange, Vector2 position, Texture2D sprite, Character target) : base(speed, turnSpeed, maxHealth, attackRange, targetRange, position, sprite, target)
        {
            gun = new Gun(10f, 2f, true, position, new Bullet(10f, BulletType.Normal), this);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            gun.Update(gameTime, position, rotation + bulletRotationOffset, lookDirection);

            if (gun.CanFire())
            {
                gun.Fire(lookDirection);
            }
        }
        public override void Draw(SpriteBatch batch)
        {
            gun.Draw(batch);
            base.Draw(batch);
        }
        public override void OnCollisionEnter(object colinfo)
        {
            base.OnCollisionEnter(colinfo);
        }
    }
}
