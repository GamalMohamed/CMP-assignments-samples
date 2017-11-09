using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace CircuitSimulationEngine
{
    class Program
    {
        public static Dictionary<string, Node> Nodes = new Dictionary<string, Node>();
        public static Dictionary<string, double> ComponentsValues = new Dictionary<string, double>();
        public static Dictionary<string, double> ComponentsInitialValues = new Dictionary<string, double>();
        public static Dictionary<string, string> ComponentsTerminals = new Dictionary<string, string>();
        public static Dictionary<string, List<string>> ComponentsCalc = new Dictionary<string, List<string>>();
        public static DenseMatrix A, G, B, C, D;
        public static DenseVector Z, X;
        public static int N, M, L, Iterations;
        public static double H;
        public static bool RCircuit = true;

        public static void ManipulateNodes(string[] componentData, ref int vCount, ref int rCount,
                                                ref int currCount, ref int capCount, ref int indCount)
        {
            for (var i = 1; i <= 2; i++)
            {
                var k = componentData[i][1].ToString();
                if (!Nodes.ContainsKey(k))
                {
                    var node = new Node();
                    if (componentData[0].Contains("Vsrc"))
                    {
                        if (i == 1)
                        {
                            componentData[0] = componentData[0] + (++vCount);
                            node.VoltageSources.Add('+' + componentData[0]);
                        }
                        else if (i == 2)
                        {
                            node.VoltageSources.Add('-' + componentData[0]);
                        }
                    }
                    else if (componentData[0].Contains("Isrc"))
                    {
                        if (i == 1)
                        {
                            componentData[0] = componentData[0] + (++currCount);
                            node.CurrentSources.Add('+' + componentData[0]);
                        }
                        else if (i == 2)
                        {
                            node.CurrentSources.Add('-' + componentData[0]);
                        }
                    }
                    else if (componentData[0].StartsWith("R"))
                    {
                        if (i == 1)
                        {
                            componentData[0] = componentData[0] + (++rCount);
                        }
                        node.Resistances.Add(componentData[0]);
                    }
                    else if (componentData[0].StartsWith("C"))
                    {
                        if (i == 1)
                        {
                            componentData[0] = componentData[0] + (++capCount);
                            node.CurrentSources.Add('+' + componentData[0]);
                        }
                        else if (i == 2)
                        {
                            node.CurrentSources.Add('-' + componentData[0]);
                        }
                        node.Resistances.Add(componentData[0]);
                        RCircuit = false;
                    }
                    else if (componentData[0].StartsWith("I"))
                    {
                        if (i == 1)
                        {
                            componentData[0] = componentData[0] + (++indCount);
                            node.VoltageSources.Add('+' + componentData[0]);
                        }
                        else if (i == 2)
                        {
                            node.VoltageSources.Add('-' + componentData[0]);
                        }
                        RCircuit = false;
                    }
                    Nodes.Add(componentData[i][1].ToString(), node);

                }
                else
                {
                    if (componentData[0].Contains("Vsrc"))
                    {
                        if (i == 1)
                        {
                            componentData[0] = componentData[0] + (++vCount);
                            Nodes[componentData[i][1].ToString()].VoltageSources.Add('+' + componentData[0]);
                        }
                        else if (i == 2)
                        {
                            Nodes[componentData[i][1].ToString()].VoltageSources.Add('-' + componentData[0]);
                        }
                    }
                    else if (componentData[0].Contains("Isrc"))
                    {
                        if (i == 1)
                        {
                            componentData[0] = componentData[0] + (++currCount);
                            Nodes[componentData[i][1].ToString()].CurrentSources.Add('+' + componentData[0]);
                        }
                        else if (i == 2)
                        {
                            Nodes[componentData[i][1].ToString()].CurrentSources.Add('-' + componentData[0]);
                        }
                    }
                    else if (componentData[0].StartsWith("R"))
                    {
                        if (i == 1)
                        {
                            componentData[0] = componentData[0] + (++rCount);
                        }
                        Nodes[componentData[i][1].ToString()].Resistances.Add(componentData[0]);
                    }
                    else if (componentData[0].StartsWith("C"))
                    {
                        if (i == 1)
                        {
                            componentData[0] = componentData[0] + (++capCount);
                            Nodes[componentData[i][1].ToString()].CurrentSources.Add('+' + componentData[0]);
                        }
                        else if (i == 2)
                        {
                            Nodes[componentData[i][1].ToString()].CurrentSources.Add('-' + componentData[0]);
                        }
                        Nodes[componentData[i][1].ToString()].Resistances.Add(componentData[0]);
                        RCircuit = false;
                    }
                    else if (componentData[0].StartsWith("I"))
                    {
                        if (i == 1)
                        {
                            componentData[0] = componentData[0] + (++indCount);
                            Nodes[componentData[i][1].ToString()].VoltageSources.Add('+' + componentData[0]);
                        }
                        else if (i == 2)
                        {
                            Nodes[componentData[i][1].ToString()].VoltageSources.Add('-' + componentData[0]);
                        }
                        RCircuit = false;
                    }
                }
            }
        }

        public static void Init(string path)
        {
            var netlist = File.ReadAllLines(path);

            H = Convert.ToDouble(netlist[0]);
            Iterations = Convert.ToInt32(netlist[1]);

            int vCount, currCount, capCount, indCount;
            var rCount = vCount = currCount = capCount = indCount = 0;
            for (var i = 2; i < netlist.Length; i++)
            {
                var componentData = netlist[i].Split(' ');

                ManipulateNodes(componentData, ref vCount, ref rCount, ref currCount, ref capCount, ref indCount);

                ComponentsValues.Add(componentData[0], Convert.ToDouble(componentData[3]));

                ComponentsInitialValues.Add(componentData[0], Convert.ToDouble(componentData[4]));

                ComponentsTerminals.Add(componentData[0], componentData[1][1].ToString() + ' ' + componentData[2][1]);
            }

            N = Nodes.Count - 1;
            M = vCount;
            L = indCount;
        }

        public static void FillMatrixZ(bool regenerated = false, List<double> vi = null, List<double> ii = null)
        {
            Z = new DenseVector(N + M + L);
            int v = 0;
            for (var i = 0; i < N + M + L; i++)
            {
                // Current sources
                if (i < N)
                {
                    var currentsources = Nodes[(i + 1).ToString()].CurrentSources;
                    if (currentsources.Count > 0)
                    {
                        foreach (var currentsource in currentsources)
                        {
                            if (currentsource.Contains("Isrc"))
                            {
                                if (currentsource[0] == '+')
                                {
                                    Z[i] += ComponentsValues[currentsource.Remove(0, 1)];
                                }
                                else if (currentsource[0] == '-')
                                {
                                    Z[i] += -ComponentsValues[currentsource.Remove(0, 1)];
                                }
                            }
                            else if (currentsource.Contains("C"))
                            {
                                if (currentsource[0] == '+')
                                {
                                    if (regenerated && vi != null)
                                    {
                                        var capacitorTerminals = ComponentsTerminals[currentsource.Remove(0, 1)].Split(' ');

                                        var vtsrc = vi[Convert.ToInt32(capacitorTerminals[0]) - 1];
                                        var vtdst = Convert.ToInt32(capacitorTerminals[1]) == 0 ?
                                                            0 : vi[Convert.ToInt32(capacitorTerminals[0]) - 1];

                                        Z[i] += (ComponentsValues[currentsource.Remove(0, 1)] / H)
                                            * (vtsrc - vtdst);
                                    }
                                    else
                                    {
                                        Z[i] += (ComponentsValues[currentsource.Remove(0, 1)] / H) * ComponentsInitialValues[currentsource.Remove(0, 1)];
                                    }

                                }
                                else if (currentsource[0] == '-')
                                {
                                    if (!regenerated)
                                    {
                                        Z[i] += -(ComponentsValues[currentsource.Remove(0, 1)] / H) * ComponentsInitialValues[currentsource.Remove(0, 1)];
                                    }
                                    else
                                    {
                                        //Z[i] += -(ComponentsValues[currentsource.Remove(0, 1)] / H) * vi;
                                    }

                                }
                            }
                        }
                    }
                    else
                    {
                        Z[i] = 0;
                    }
                }
                // Independant Voltage sources
                else if (i < N + M)
                {
                    Z[i] = ComponentsValues["Vsrc" + (++v)];
                }
                // Dependant Voltage sources (inductors)
                else
                {
                    if (i == N + M)
                    {
                        v = 1; //reset v!
                    }
                    if (regenerated && ii != null)
                    {
                        Z[i] = -(ComponentsValues["I" + v] / H) * ii[v++];
                    }
                    else
                    {
                        Z[i] = -(ComponentsValues["I" + v] / H) * ComponentsInitialValues["I" + (v++)];
                    }

                }
            }
        }

        public static void FillMatrices()
        {
            // 1. Fill G Matrix
            G = new DenseMatrix(N, N);
            for (var i = 0; i < N; i++)
            {
                for (var j = 0; j < N; j++)
                {
                    if (i == j)
                    {
                        foreach (var resistance in Nodes[(i + 1).ToString()].Resistances)
                        {
                            if (resistance[0] == 'R')
                            {
                                G[i, j] += 1 / ComponentsValues[resistance];
                            }
                            else if (resistance[0] == 'C')
                            {
                                G[i, j] += ComponentsValues[resistance] / H;
                            }
                        }
                    }
                    else
                    {
                        var nr1 = Nodes[(i + 1).ToString()].Resistances;
                        var nr2 = Nodes[(j + 1).ToString()].Resistances;
                        var ir = nr1.Intersect(nr2).ToList();
                        foreach (var resistance in ir)
                        {
                            if (resistance[0] == 'R')
                            {
                                G[i, j] += -1 / ComponentsValues[resistance];
                            }
                            else if (resistance[0] == 'C')
                            {
                                G[i, j] += -ComponentsValues[resistance] / H;
                            }
                        }
                    }
                }
            }

            // 2. Fill Matrix B
            B = new DenseMatrix(N, M + L);
            for (var i = 0; i < N; i++)
            {
                var vl = Nodes[(i + 1).ToString()].VoltageSources;
                int co = 1;
                string rq = "Vsrc";
                for (var j = 0; j < M + L; j++)
                {
                    if (j == M)
                    {
                        co = 1;
                        rq = "I";
                    }
                    var vsrc = vl.Find(e => e.Contains(rq + co));
                    if (vsrc != null)
                    {
                        if (vsrc[0] == '+')
                        {
                            B[i, j] = 1;
                        }
                        else if (vsrc[0] == '-')
                        {
                            B[i, j] = -1;
                        }
                    }
                    else
                    {
                        B[i, j] = 0;
                    }
                    co++;
                }
            }

            // 3. Fill Matrix C
            C = B.Transpose() as DenseMatrix;

            // 4. Fill Matrix D
            D = new DenseMatrix(M + L, M + L);
            if (L > 0)
            {
                for (int i = 0; i < M + L; i++)
                {
                    if (i >= M)
                    {
                        D[i, i] = -ComponentsValues["I" + i] / H;
                    }
                }
            }

            // 5. Combine to form Matrix A
            A = new DenseMatrix(M + N + L, M + N + L);
            int r = 0;
            for (var i = 0; i < M + N + L; i++)
            {
                int c = 0;
                for (var j = 0; j < M + N + L; j++)
                {
                    if (i < N)
                    {
                        if (j < N)
                        {
                            A[i, j] = G[i, j];
                        }
                        else
                        {
                            A[i, j] = B[i, c++];
                        }
                        r = 0; //HACK
                    }
                    else
                    {
                        if (j < N)
                        {
                            if (C != null)
                                A[i, j] = C[r - 1, j];
                        }
                        else
                        {
                            A[i, j] = D[r - 1, c++];
                        }
                    }
                }
                r++;
            }

            //6. Fill Vector Z
            FillMatrixZ();
        }

        public static DenseVector VoltageIterations()
        {
            var results = new DenseVector(N + M + L);
            var aInv = A.Inverse() as DenseMatrix;
            var h = H;
            for (var i = 0; i < Iterations; i++, h += H)
            {
                results = aInv?.Multiply(Z) as DenseVector;
                if (results == null)
                    break;

                var vi = new List<double>();
                var ii = new List<double>();
                for (var j = 0; j < N + M + L; j++)
                {
                    if (j < N)
                    {
                        vi.Add(results[j]);
                    }
                    else
                    {
                        ii.Add(results[j]);
                    }
                }

                FillMatrixZ(true, vi, ii);
                SaveCalculations(results, h);
            }

            return results;
        }

        public static void SaveCalculations(DenseVector results, double h)
        {
            int c = 1;
            for (var i = 0; i < results.Count; i++)
            {
                if (i < N)
                {
                    if (!ComponentsCalc.ContainsKey("V" + (i + 1)))
                    {
                        ComponentsCalc.Add("V" + (i + 1), new List<string>() { h.ToString(CultureInfo.InvariantCulture) + ' ' + results[i] });
                    }
                    else
                    {
                        ComponentsCalc["V" + (i + 1)].Add(h.ToString(CultureInfo.InvariantCulture) + ' ' + results[i]);
                    }
                }
                else
                {
                    if (!ComponentsCalc.ContainsKey("I" + c))
                    {
                        ComponentsCalc.Add("I" + c, new List<string>() { h.ToString(CultureInfo.InvariantCulture) + ' ' + results[i] });
                    }
                    else
                    {
                        ComponentsCalc["I" + c].Add(h.ToString(CultureInfo.InvariantCulture) + ' ' + results[i]);
                    }
                    c++;
                }
            }
        }

        public static void PrintResults()
        {
            Console.WriteLine();
            using (var outputFile = new StreamWriter("output.txt"))
            {
                foreach (var r in ComponentsCalc)
                {
                    outputFile.WriteLine(r.Key);
                    Console.WriteLine(r.Key);
                    foreach (var rr in r.Value)
                    {
                        outputFile.WriteLine(rr);
                        Console.WriteLine(rr);
                    }
                    outputFile.WriteLine();
                    Console.WriteLine();
                }
            }
        }

        public static DenseVector ComputeResults()
        {
            if (RCircuit)
            {
                var x = A.Inverse().Multiply(Z) as DenseVector;
                SaveCalculations(x, H);
                return x;
            }
            else
            {
                return VoltageIterations();
            }
        }

        private static void Main(string[] args)
        {
            Console.WriteLine("Enter file name:");
            Init(Console.ReadLine() + ".txt");

            FillMatrices();

            X = ComputeResults();

            PrintResults();

            Console.ReadKey();
        }
    }
}
