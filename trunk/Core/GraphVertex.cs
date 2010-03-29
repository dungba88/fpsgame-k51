using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Collections;

namespace FPSGame.Core
{
    public class GraphVertex
    {
        private Vector2 vertex;
        private ArrayList edges;    //list of adjacent edges
        public float weight { set; get; }

        public GraphVertex(Vector2 vertex)
        {
            this.vertex = vertex;
            edges = new ArrayList();
        }

        public GraphEdge[] GetAdjacentEdges()
        {
            return (GraphEdge[])edges.ToArray(typeof(GraphEdge));
        }

        public void AddEdge(GraphEdge edge)
        {
            edges.Add(edge);
        }

        public void RemoveEdge(GraphEdge edge)
        {
            edges.Remove(edge);
        }

        public Vector2 GetVertex()
        {
            return vertex;
        }

        public GraphEdge FindEdge(Vector2 dst)
        {
            foreach (GraphEdge edge in edges)
            {
                GraphVertex other;
                GraphVertex src = edge.GetSource();
                GraphVertex dest = edge.GetDestination();
                if (src == this) other = dest;
                else other = src;

                if (other.GetVertex().X == dst.X && other.GetVertex().Y == dst.Y)
                    return edge;
            }

            return null;
        }
    }
}
