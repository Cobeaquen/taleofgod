using Microsoft.Xna.Framework;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace TheTaleOfGod
{
    [ProtoContract]
    public class Collider
    {
        [ProtoMember(1)]
        public int width;
        [ProtoMember(2)]
        public int height;

        [ProtoMember(3)]
        public string tag;

        public Raycast[] edges;

        public object owner;

        [ProtoMember(4)]
        public Vector2 position;

        public Node[] nodes;

        public int Right;
        public int Left;
        public int Top;
        public int Bottom;

        public Texture2D debugSprite;
        public Vector2 debugOrigin;

        public Collider(Vector2 position, int width, int height, string tag = null, object owner = null)
        {
            this.position = position;
            this.width = width;
            this.height = height;
            this.tag = tag;
            this.owner = owner;

            Right = (int)position.X + width/2;
            Left = (int)position.X - width/2;
            Top = (int)position.Y - height/2;
            Bottom = (int)position.Y + height/2;

            nodes = new Node[4]
            {
                new Node(new Vector2(Left - 16, Top - 16)),
                new Node(new Vector2(Right + 16, Top - 16)),
                new Node(new Vector2(Left - 16, Bottom + 16)),
                new Node(new Vector2(Right + 16, Bottom + 16))
            };
        }
        public Collider()
        {

        }
        public void Update(GameTime gameTime)
        {
            Right = (int)position.X + width / 2;
            Left = (int)position.X - width / 2;
            Top = (int)position.Y - height / 2;
            Bottom = (int)position.Y + height / 2;

            edges = new Raycast[4]
            {
                new Raycast(new Vector2(Left, Top), new Vector2(Right, Top)),
                new Raycast(new Vector2(Left, Bottom), new Vector2(Right, Bottom)),
                new Raycast(new Vector2(Left, Top), new Vector2(Left, Bottom)),
                new Raycast(new Vector2(Right, Top), new Vector2(Right, Bottom))
            };
            nodes = new Node[4]
            {
                new Node(new Vector2(Left - 32, Top - 32)),
                new Node(new Vector2(Right + 32, Top - 32)),
                new Node(new Vector2(Left - 16, Bottom + 16)),
                new Node(new Vector2(Right + 16, Bottom + 16))
            };
        }
        public void DrawDebug(SpriteBatch batch)
        {
            if (debugSprite != null)
            {
                batch.Draw(debugSprite, position, null, Color.White, 0f, debugOrigin, 1f, SpriteEffects.None, 0f); // fiiix mee
            }
            Texture2D tx = DebugTextures.GenerateRectangle(10, 10, Color.White);
            batch.Draw(tx, position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f); // fiiix mee
        }
    }
}
