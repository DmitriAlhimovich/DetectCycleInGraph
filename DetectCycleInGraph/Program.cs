using System;
using System.Collections.Generic;
using System.Linq;

namespace DetectCycleInGraph
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var c1 = new Company { Id = 1, Name = "c1" };
            var c2 = new Company { Id = 2, Name = "c2" };
            var c3 = new Company { Id = 3, Name = "c3" };
            var c4 = new Company { Id = 4, Name = "c4" };
            var c5 = new Company { Id = 5, Name = "c5" };
            var c6 = new Company { Id = 6, Name = "c6" };
            var c7 = new Company { Id = 7, Name = "c7" };

            var companies = new List<Company> { c1, c2, c3, c4, c5, c6, c7 };

            var debts = new List<DebtInfo>
            {
                new DebtInfo {Id = 1, Debitor = c1, Creditor = c2, Amount = 100},
                new DebtInfo {Id = 2, Debitor = c2, Creditor = c3, Amount = 100},
                new DebtInfo {Id = 3, Debitor = c3, Creditor = c4, Amount = 100},
                new DebtInfo {Id = 4, Debitor = c4, Creditor = c5, Amount = 100},
                new DebtInfo {Id = 5, Debitor = c5, Creditor = c6, Amount = 100},
                new DebtInfo {Id = 6, Debitor = c6, Creditor = c4, Amount = 100},

            };


            var companiesCount = debts.Select(c => c.Creditor).Union(debts.Select(c => c.Debitor)).Distinct().Count();

            var graph = new Graph(companiesCount);
            foreach (var debt in debts)
            {
                graph.AddEdge(companies.IndexOf(debt.Debitor), companies.IndexOf(debt.Creditor));
            }

            var checkResult = graph.IsCyclic();

            var vertexInCycle = checkResult.Item2;

            var cycle = new List<DebtInfo>();

            while (true)
            {

            }

            Console.WriteLine($"Is Cyclic = {checkResult.Item1}, vertex={checkResult.Item2}");
        }

        public class Graph
        {
            public int VeticesCount;    // No. of vertices
            public List<List<int>> Adjacencies = new();

            public int? VertexInCycle;

            public Graph(int verticesCount)
            {
                VeticesCount = verticesCount;

                for (int i = 0; i < verticesCount; i++)
                    Adjacencies.Add(new List<int>());
            }

            public void AddEdge(int source, int destination)
            {
                Adjacencies[source].Add(destination);
            }

            private (bool,int) IsCyclicInternal(int vertex, bool[] visited, bool[] stack)
            {
                if (visited[vertex] == false)
                {
                    // Mark the current node as visited and part of recursion stack
                    visited[vertex] = true;
                    stack[vertex] = true;

                    // Recur for all the vertices adjacent to this vertex

                    foreach (var innerVertex in Adjacencies[vertex])
                    {
                        if ((visited[innerVertex] || !IsCyclicInternal(innerVertex, visited, stack).Item1) &&
                            !stack[innerVertex]) continue;
                        
                        VertexInCycle ??= innerVertex;
                        return (true, VertexInCycle.Value);
                    }

                }
                stack[vertex] = false;  // remove the vertex from recursion stack
                return (false, -1);
            }

            public (bool, int) IsCyclic()
            {
                // Mark all the vertices as not visited and not part of recursion
                // stack
                var visited = new bool[VeticesCount];
                var recStack = new bool[VeticesCount];
                for (int i = 0; i < VeticesCount; i++)
                {
                    visited[i] = false;
                    recStack[i] = false;
                }

                for (int i = 0; i < VeticesCount; i++)
                {
                    var result = IsCyclicInternal(i, visited, recStack);
                    if (result.Item1)
                    {
                        return (true, result.Item2);
                    }
                }

                return (false, -1);
            }
        }


        public class DebtInfo
        {
            public int Id { get; set; }
            public Company Creditor { get; set; }
            public Company Debitor { get; set; }
            public decimal Amount { get; set; }
        }

        public class Company
        {
            public int Id { get; set; }
            public string UNP { get; set; }
            public string Name { get; set; }
        }
    }
}
