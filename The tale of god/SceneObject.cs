using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using ProtoBuf;

namespace TheTaleOfGod
{
    [ProtoContract]
    public class SceneObject
    {
        [ProtoMember(1)]
        public Vector2 position;
        public Cell cell;
        public Vector2 origin;
        [ProtoMember(2)]
        public int width;
        [ProtoMember(3)]
        public int height;
        [ProtoMember(4)]
        public bool stationary;

        public Collider collider;

        protected Texture2D sprite;

        public SceneObject(int width, int height, bool stationary)
        {
            this.width = width;
            this.height = height;
            this.stationary = stationary;
            collider = new Collider(position, width, height, "sceneobject", this);

            Game1.instance.map.colliders.Add(collider);

            sprite = DebugTextures.GenerateRectangle(width, height, Color.PowderBlue);
            origin = new Vector2(width / 2f, height / 2f);

            cell = Cell.GetCell(position);
            cell.colliders.Add(collider);
        }

        public SceneObject()
        {

        }

        public virtual void Update()
        {
            collider.position = position;
            Cell newCell = Cell.GetCell(position);
            if (cell != newCell)
            {
                cell.colliders.Remove(collider);
                cell = newCell;
                cell.colliders.Add(collider);
            }
        }

        public virtual void Draw(SpriteBatch batch)
        {
            batch.Draw(sprite, position, null, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);
        }
    }
}
