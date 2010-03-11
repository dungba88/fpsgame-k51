using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FPSGame.Core;
using Microsoft.Xna.Framework;
using FPSGame.Engine.GameEvent;

namespace FPSGame.Engine
{
    class ObjectManager : ISubject
    {
        private ArrayList objects;
        private ArrayList addingObjects;
        private ArrayList removingObjects;

        private static ObjectManager instance = new ObjectManager();

        private ObjectManager()
        {
            objects = new ArrayList();
            addingObjects = new ArrayList();
            removingObjects = new ArrayList();
        }

        public static ObjectManager GetInstance()
        {
            return instance;
        }

        public void CleanUp()
        {
            //remove objects pending for removal
            foreach (IDisplayObject obj in removingObjects)
            {
                objects.Remove(obj);
            }
            removingObjects.Clear();

            //add objects pending for addition
            foreach (IDisplayObject obj in addingObjects)
            {
                objects.Add(obj);
            }
            addingObjects.Clear();
        }

        public void Update(GameTime gameTime)
        {
            foreach (IDisplayObject obj in objects)
            {
                obj.Update(gameTime);
            }
            CleanUp();
        }

        public void Draw(GameTime gameTime)
        {
            foreach (IDisplayObject obj in objects)
            {
                obj.Draw(gameTime);
            }
        }

        public bool IsObjectRegistered(IDisplayObject obj)
        {
            if (objects.Contains(obj))
                return true;
            return false;
        }

        public void RegisterObject(IDisplayObject obj)
        {
            if (!IsObjectRegistered(obj))
            {
                addingObjects.Add(obj);
            }
        }

        public void UnregisterObject(IDisplayObject obj)
        {
            if (IsObjectRegistered(obj))
            {
                removingObjects.Remove(obj);
            }
        }

        public void RemoveAllObjects()
        {
            removingObjects.Clear();
        }

        public void NotifyAllObservers(IGameEvent evt)
        {
            foreach (IObserver obj in objects)
            {
                obj.Notify(evt);
            }
        }
    }
}
