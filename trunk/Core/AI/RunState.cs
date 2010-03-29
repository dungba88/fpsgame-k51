using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FPSGame.Object;
using Microsoft.Xna.Framework;
using FPSGame.Engine;
using FPSGame.Factory;
using System.Collections;

namespace FPSGame.Core.AI
{
    public class RunState : SimpleState
    {
        protected bool destReached = false;
        private Vector3 target; //the target to run to
        private Vector3 source;
        private GraphVertex[] flattened;
        private int current;
        private float elemSize;
        //private int lastGameTime;

        public RunState(SimpleCharacter character, Vector3 src, Vector3 target)
            : base (character)
        {
            this.source = src;
            this.target = target;
        }

        public Vector3 GetSource()
        {
            return source;
        }

        public Vector3 GetTarget()
        {
            return target;
        }

        public void SetSource(Vector3 source)
        {
            this.source = source;
        }

        public void SetTarget(Vector3 target)
        {
            this.target = target;
        }

        public override void Begin()
        {
            elemSize = MapLoader.GetInstance().GetMap().GetElemSize();
            source = GetCharacter().GetPosition();
            Graph p = MapLoader.GetInstance().GetMap().GetGraph();

            ArrayList list = p.GetVertices();

            String var = "Session_Run_flattened" + GetCharacter().GetId() + "_" + (int)(source.X / elemSize) + (int)(source.Z / elemSize) + "_" + (int)(target.X / elemSize) + (int)(target.Z / elemSize);
            if (StateSessionStorage.IsVarRegistered(var))
            {
                flattened = StateSessionStorage.LoadVar<GraphVertex[]>(var);
            }
            else
            {
                GraphVertex v1 = p.FindVertex(MathUtils.GetMatrixVector(source, elemSize));
                GraphVertex v2 = p.FindVertex(MathUtils.GetMatrixVector(target, elemSize));
                GraphVertex[][] vertices = p.SolveShortestPath(v1, v2);
                flattened = p.Flatten(vertices, v1, v2);
                StoreSession(var);
            }

            current = flattened.Count() - 1;

            base.Begin();
        }

        public virtual void StoreSession(String var)
        {
            StateSessionStorage.StoreVar(var, flattened);
        }

        public override void Update(GameTime gameTime)
        {
            Vector3 pos = GetCharacter().GetPosition();

            //check to see if we has already moved to the destination
            if (destReached || ReachDestination())
            {
                destReached = true;
                OnDestinationReached();
                return;
            }

            //change the rotation to the next element
            GraphVertex next = flattened[current - 1];
            Vector3 dir = MathUtils.GetWorldVector(next.GetVertex(), elemSize) - pos;
            GetCharacter().GetCamera().SetDirection(new Vector3(dir.Z, 0, dir.X));
            GetCharacter().Run();

            //update the current position based on the flatten array
            //if we are in the next 'element'
            if (MathUtils.Equal(pos, MathUtils.GetWorldVector(flattened[current - 1].GetVertex(), elemSize), elemSize))
            {
                current--;
            }

            base.Update(gameTime);
        }

        public bool IsDestinationReached()
        {
            return destReached;
        }

        public virtual void OnDestinationReached()
        {
            destReached = true;
        }

        private bool ReachDestination()
        {
            Vector3 pos = GetCharacter().GetPosition();
            return MathUtils.Equal(pos, MathUtils.GetWorldVector(flattened[0].GetVertex(), elemSize), elemSize);
        }
    }
}
