using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace TheTaleOfGod
{
    public class Wall : SceneObject
    {
        public Wall(Vector2 position, int width, int height) : base(width, height)
        {
            this.position = position;
        }
        public override void Load()
        {
            base.Load();
        }
        public override void Update()
        {
            base.Update();
        }
        public override void Draw(SpriteBatch batch)
        {
            base.Draw(batch);
        }
    }
}
