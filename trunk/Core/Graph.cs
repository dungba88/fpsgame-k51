using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Microsoft.Xna.Framework;
using LSCollections;

namespace FPSGame.Core
{
    public class Graph
    {
        private ArrayList vertices;
        private ArrayList edges;
        private int w;
        private int h;

        public Graph(int w, int h)
        {
            this.edges = new ArrayList();
            this.vertices = new ArrayList();
            this.w = w;
            this.h = h;
        }

        public Graph(ArrayList edges, ArrayList vertices, int w, int h)
        {
            this.edges = edges;
            this.vertices = vertices;
            this.w = w;
            this.h = h;
        }

        public ArrayList GetEdges()
        {
            return edges;
        }

        public ArrayList GetVertices()
        {
            return vertices;
        }

        public void AddVertex(GraphVertex v)
        {
            vertices.Add(v);
        }

        public void AddEdge(GraphEdge e)
        {
            edges.Add(e);
        }

        public void DumpVertices()
        {
            foreach (GraphVertex v in vertices)
            {
                System.Console.Write("Vertex(" + v.GetVertex().X + ", " + v.GetVertex().Y + ")\n");
            }
        }

        public void DumpEdge()
        {
            foreach (GraphEdge e in edges)
            {
                GraphVertex v1 = e.GetSource();
                GraphVertex v2 = e.GetDestination();
                System.Console.Write("Edge between Vertex(" + v1.GetVertex().X + ", " + v1.GetVertex().Y + ") and Vertex(" + v2.GetVertex().X + ", " + v2.GetVertex().Y + ")\n");
            }
        }

        public void Dump(GraphVertex[][] vertices)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                for (int j = 0; j < vertices[i].Length; j++)
                {
                    GraphVertex v = vertices[i][j];
                    if (v != null)
                        System.Console.Write("Vertex [" + i + "]" + "[" + j + "] = " + "Vertex(" + v.GetVertex().X + "," + v.GetVertex().Y + ")\n");
                    else
                        System.Console.Write("Vertex [" + i + "]" + "[" + j + "] = Null\n");
                }
            }
        }

        public GraphVertex FindVertex(Vector2 pos)
        {
            foreach (GraphVertex vertex in vertices)
            {
                if (vertex.GetVertex().X == pos.X && vertex.GetVertex().Y == pos.Y)
                    return vertex;
            }

            return null;
        }

        public void RemoveEdges(GraphEdge edge)
        {
            if (!edges.Contains(edges)) return;

            //remove the edge
            edges.Remove(edge);

            //update the vertices
            GraphVertex v1 = edge.GetSource();
            GraphVertex v2 = edge.GetDestination();
            v1.RemoveEdge(edge);
            v2.RemoveEdge(edge);
        }

        public GraphVertex[][] SolveShortestPath_Dijkstra(GraphVertex src, GraphVertex dst)
        {
            PriorityQueue q = new PriorityQueue();
            GraphVertex[][] prev = new GraphVertex[w][];

            for (int i = 0; i < prev.Length; i++)
            {
                prev[i] = new GraphVertex[h];
            }

            foreach (GraphVertex v in vertices)
            {
                int i = (int)v.GetVertex().X;
                int j = (int)v.GetVertex().Y;
                prev[i][j] = null;
                v.weight = float.MaxValue;
                q.Enqueue(v);
            }

            src.weight = 0;

            while (q.Count > 0)
            {
                GraphVertex wv = (GraphVertex)q.Dequeue();
                if (wv.weight == float.MaxValue)
                    break;

                foreach (GraphEdge edge in wv.GetAdjacentEdges())
                {
                    GraphVertex v1 = FindOtherEndpoint(edge, wv);
                    if (q.Contains(v1))
                    {
                        float f = wv.weight + 1;
                        if (f < v1.weight)
                        {
                            v1.weight = f;
                            prev[(int)v1.GetVertex().X][(int)v1.GetVertex().Y] = wv;
                        }
                    }
                }
            }

            return prev;
        }


        public GraphVertex[][] SolveShortestPath(GraphVertex src, GraphVertex dst)
        {
            bool b = false;
            Queue pq = new Queue();
            Queue q = new Queue();
            pq.Enqueue(src);
            GraphVertex[][] vertices;

            vertices = new GraphVertex[w][];
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = new GraphVertex[h];
            }

            while (true)
            {
                if (pq.Count == 0) break;
                GraphVertex v = (GraphVertex)pq.Dequeue();
                q.Enqueue(v);

                if (v == dst)
                {
                    b = true;
                    break;
                }

                foreach (GraphEdge edge in v.GetAdjacentEdges())
                {
                    GraphVertex v1 = FindOtherEndpoint(edge, v);
                    if (!q.Contains(v1) && !pq.Contains(v1))
                    {
                        vertices[(int)v1.GetVertex().X][(int)v1.GetVertex().Y] = v;
                        pq.Enqueue(v1);
                    }
                }
            }

            if (!b) return null;
            return vertices;
        }

        public GraphVertex[] Flatten(GraphVertex[][] vertices, GraphVertex src, GraphVertex dst)
        {
            ArrayList verts = new ArrayList();
            GraphVertex v = dst;
            verts.Add(v);

            while (true)
            {
                v = vertices[(int)v.GetVertex().X][(int)v.GetVertex().Y];
                if (v == null) break;
                verts.Add(v);
                if (v == src) break;
            }

            return (GraphVertex[])verts.ToArray(typeof(GraphVertex));
        }

        private GraphVertex FindOtherEndpoint(GraphEdge edge, GraphVertex v)
        {
            GraphVertex v1 = edge.GetSource();
            GraphVertex v2 = edge.GetDestination();
            return (v == v1) ? v2 : v1;
        }
    }

    class WeightedVertex
    {
        public GraphVertex v { get; protected set; }
        public float dist {get; protected set;}

        public WeightedVertex(GraphVertex v, float dist)
        {
            this.v = v;
            this.dist = dist;
        }
    }
}
