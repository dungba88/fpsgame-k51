using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace FPSGame.Core
{
    public class GraphEdge
    {
        private GraphVertex src;
        private GraphVertex dst;

        public GraphEdge(GraphVertex src, GraphVertex dst)
        {
            this.src = src;
            this.dst = dst;
            src.AddEdge(this);
            dst.AddEdge(this);
        }

        public GraphVertex GetSource()
        {
            return src;
        }

        public GraphVertex GetDestination()
        {
            return dst;
        }
    }
}
