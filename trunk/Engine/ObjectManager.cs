using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FPSGame.Core;

namespace FPSGame.Engine
{
    class ObjectManager : ISubject
    {
        private ArrayList objects;

        private static ObjectManager instance = new ObjectManager();

        private ObjectManager()
        {
            objects = new ArrayList();
        }

        public static ObjectManager getInstance()
        {
            return instance;
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
                objects.Add(obj);
            }
        }

        public void UnregisterObject(IDisplayObject obj)
        {
            if (IsObjectRegistered(obj))
            {
                objects.Remove(obj);
            }
        }

        public void RemoveAllObjects()
        {
            objects.Clear();
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
