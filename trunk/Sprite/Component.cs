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
    public class Component: IActionDispatcher, IComponent
    {
        private Texture2D texture;
        private Texture2D textureoff;
        private Texture2D currentTexture;
        private ArrayList listeners;
        private double x;
        private double y;
        private bool dead;

        public Component(Texture2D texture, Texture2D textureoff, bool defaultActionListener)
        {
            this.texture = texture;
            this.textureoff = textureoff;
            currentTexture = texture;
            listeners = new ArrayList();
            listeners.Add(new DefaultActionListener());
            dead = false;
        }

        public void AddActionListener(IActionListener al)
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
            return texture.Width;
        }

        public double GetHeight()
        {
            return texture.Height;
        }

        public void Dispose()
        {
            dead = true;
        }

        public void Draw(GameTime gameTime)
        {
            if (dead) return;
            FPSGame.GetInstance().DrawSprite(currentTexture, new Vector2((float)x, (float)y), Color.TransparentBlack);
        }

        public void TriggerActionPerformed(IActionEvent evt)
        {
            if (!CheckInside(evt.GetX(), evt.GetY()))
                return;
            foreach (IActionListener al in listeners)
            {
                al.OnActionPerformed(evt.CopyEventWithSource(this));
            }
        }

        public void TriggerMouseMove(IActionEvent evt)
        {
            foreach (IActionListener al in listeners)
            {
                if (CheckInside(evt.GetX(), evt.GetY()))
                    al.OnMouseOver(evt.CopyEventWithSource(this));
                else
                    al.OnMouseOut(evt.CopyEventWithSource(this));
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

    class DefaultActionListener : IActionListener
    {
        public void OnMouseOver(IActionEvent evt)
        {
            evt.GetSource().SwitchTextureOff();   
        }

        public void OnMouseOut(IActionEvent evt)
        {
            evt.GetSource().SwitchTextureOn();
        }

        public void OnActionPerformed(IActionEvent evt)
        {
        }
    }
}
