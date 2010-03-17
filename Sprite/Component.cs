using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FPSGame.Core;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;
using Microsoft.Xna.Framework;

namespace FPSGame.Sprite
{
    public class Component: IComponent
    {
        private Texture2D texture;
        private Texture2D textureoff;
        private Texture2D currentTexture;
        private ArrayList listeners;
        private double x;
        private double y;
        private bool dead;
        private bool centered;
        private ActionListener defAL;

        public Component(Texture2D texture, Texture2D textureoff, double x, double y, bool defaultActionListener, bool centered)
        {
            this.texture = texture;
            this.textureoff = textureoff;
            currentTexture = texture;
            this.x = x;
            this.y = y;
            this.centered = centered;
            listeners = new ArrayList();
            listeners.Add(defAL = new DefaultActionListener());
            dead = false;
        }

        public ActionListener GetDefaultActionListener()
        {
            return defAL;
        }

        public void AddActionListener(ActionListener al)
        {
            listeners.Add(al);
        }

        public double GetX()
        {
            return x;
        }

        public void SetX(double x)
        {
            this.x = x;
        }

        public double GetY()
        {
            return y;
        }

        public void SetY(double y)
        {
            this.y = y;
        }

        public double GetWidth()
        {
            return currentTexture.Width;
        }

        public double GetHeight()
        {
            return currentTexture.Height;
        }

        public void Dispose()
        {
            dead = true;
        }

        public void Draw(GameTime gameTime)
        {
            if (dead) return;
            Vector2 pos;
            if (centered)
                pos = new Vector2((float)x - currentTexture.Width / 2, (float)y-currentTexture.Height/2);
            else
                pos = new Vector2((float)x, (float)y);
            FPSGame.GetInstance().DrawSprite(currentTexture, pos, Color.Aqua);
        }

        public void TriggerActionPerformed(IActionEvent evt)
        {
            if (!CheckInside(evt.GetX(), evt.GetY()))
                return;

            foreach (ActionListener al in listeners)
            {
                al.GetActionPerformedMethod()(evt.CopyEventWithSource(this));
            }
        }

        public void TriggerMouseMove(IActionEvent evt)
        {
            foreach (ActionListener al in listeners)
            {
                if (CheckInside(evt.GetX(), evt.GetY()))
                    al.GetMouseOverMethod()(evt.CopyEventWithSource(this));
                else
                    al.GetMouseOutMethod()(evt.CopyEventWithSource(this));
            }
        }

        public void SwitchTextureOn()
        {
            this.currentTexture = texture;
        }

        public void SwitchTextureOff()
        {
            this.currentTexture = textureoff;
        }

        private bool CheckInside(double x, double y)
        {
            if (x < this.x || x > this.x + this.GetWidth())
                return false;
            if (y < this.y || y > this.y + this.GetHeight())
                return false;
            return true;
        }
    }
}
