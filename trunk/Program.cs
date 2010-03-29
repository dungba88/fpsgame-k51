using System;
using FPSGame.Core;
using Microsoft.Xna.Framework;
using System.Collections;

namespace FPSGame
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (FPSGame game = FPSGame.GetInstance())
            {
                game.Run();
            }
            //System.Console.Write("Begin...");
            //ArrayList vertices = new ArrayList();
            //for (int i = 0; i < 16; i++)
            //{
            //    vertices.Add(new GraphVertex(new Vector2(i / 4, i % 4)));
            //}

            //ArrayList list = new ArrayList();
            //list.Add(new GraphEdge((GraphVertex)vertices[0], (GraphVertex)vertices[1]));
            //list.Add(new GraphEdge((GraphVertex)vertices[1], (GraphVertex)vertices[2]));
            //list.Add(new GraphEdge((GraphVertex)vertices[2], (GraphVertex)vertices[6]));
            //list.Add(new GraphEdge((GraphVertex)vertices[6], (GraphVertex)vertices[7]));
            //list.Add(new GraphEdge((GraphVertex)vertices[0], (GraphVertex)vertices[4]));
            //list.Add(new GraphEdge((GraphVertex)vertices[4], (GraphVertex)vertices[8]));
            //list.Add(new GraphEdge((GraphVertex)vertices[8], (GraphVertex)vertices[12]));
            //list.Add(new GraphEdge((GraphVertex)vertices[12], (GraphVertex)vertices[13]));
            //list.Add(new GraphEdge((GraphVertex)vertices[13], (GraphVertex)vertices[14]));
            //list.Add(new GraphEdge((GraphVertex)vertices[14], (GraphVertex)vertices[10]));
            //list.Add(new GraphEdge((GraphVertex)vertices[10], (GraphVertex)vertices[6]));

            //Graph g = new Graph(list, vertices, 4, 4);
            //GraphVertex[][] gva = g.SolveShortestPath((GraphVertex)vertices[4], (GraphVertex)vertices[7]);
            //g.Dump(gva);
        }
    }
}

