using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Drawing2D;
using FPSGame.Object;

namespace FPSGame.Core
{
    public interface IMap
    {
        void OnLoad();

        void OnUnload();

        float GetElemSize();

        void AddEnemy(SimpleCharacter enemy);

        SimpleCharacter[] GetEnemies();

        void AddConstantObject(IDisplayObject obj, int x, int y);

        IDisplayObject[][] GetMatrix();

        void AddObject(IDisplayObject obj);

        void RemoveObject(IDisplayObject obj);

        IDisplayObject[] GetObjects();

        IDisplayObject[] GetFullMap();

        String GetName();

        String GetDescription();

        String GetMissionDescription();

        Graph GetGraph();

        int GetWidth();

        int GetHeight();
    }
}
