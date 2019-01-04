﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;
using ProtoBuf;

namespace TheTaleOfGod
{
    [ProtoContract]
    public class NPC
    {
        public static List<NPC> npcs = new List<NPC>();

        [ProtoMember(1)]
        public Vector2 position;
        public Vector2 origin;
        public Texture2D sprite;

        public bool interacting;

        Dialogue dialogue;

        KeyboardState prevState;

        public NPC(Vector2 position, params string[] speechLines)
        {
            interacting = false;
            this.position = position;
            dialogue = new Dialogue(speechLines, 1f, Game1.content.Load<SpriteFont>("fonts\\npc1"));
            npcs.Add(this);
        }

        public void Load(Texture2D sprite)
        {
            this.sprite = sprite;
            origin = new Vector2(sprite.Width / 2f, sprite.Height / 2f);
            dialogue.LoadSpeech();
            prevState = Keyboard.GetState();
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();

            if (interacting)
            {
                if (state.IsKeyDown(Keys.Enter) & !prevState.IsKeyDown(Keys.Enter))
                {
                    interacting = dialogue.AdvanceLine();
                    Game1.instance.character.isInteracting = interacting;
                }
            }

            prevState = state;
        }

        public void Draw(SpriteBatch batch, GameTime gameTime)
        {
            batch.Draw(sprite, position, null, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);

            if (interacting)
            {
                dialogue.DrawDialogueBox(batch);
            }
        }
        public void DrawDialogue(SpriteBatch batch, GameTime gameTime)
        {
            if (interacting)
            {
                dialogue.Draw(batch, gameTime);
            }
        }

        public void Interact()
        {
            interacting = true;
            dialogue.LoadSpeech();
        }
    }
}
