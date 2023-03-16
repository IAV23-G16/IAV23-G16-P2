/*    
   Copyright (C) 2020-2023 Federico Peinado
   http://www.federicopeinado.com
   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).
   Autor: Federico Peinado 
   Contacto: email@federicopeinado.com
*/
namespace UCM.IAV.Navegacion
{

    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
    using System;
    using TMPro;

    public enum Heur
    {
        Euclidean, Manhattan
    }

    /// <summary>
    /// Abstract class for graphs
    /// </summary>
    public abstract class Graph : MonoBehaviour
    {
        public GameObject vertexPrefab;
        protected List<Vertex> vertices;
        protected List<List<Vertex>> neighbourVertex;
        protected List<List<float>> costs;
        protected bool[,] mapVertices;
        protected float[,] costsVertices;
        protected int numCols, numRows;
        [SerializeField] LayerMask layerMask;
        [SerializeField] TextMeshProUGUI baldCaminoText;
        [SerializeField] TextMeshProUGUI lengthCamino;
        [SerializeField] TextMeshProUGUI costCamino;

        // this is for informed search like A*
        public delegate float Heuristic(Vertex a, Vertex b);

        

        // Used for getting path in frames
        public List<Vertex> path;


        public virtual void Start()
        {
            Load();
        }

        public virtual void Load() { }

        public virtual int GetSize()
        {
            if (ReferenceEquals(vertices, null))
                return 0;
            return vertices.Count;
        }

        public virtual void UpdateVertexCost(Vector3 position, float costMultipliyer) { }

        public virtual Vertex GetNearestVertex(Vector3 position)
        {
            return null;
        }

        public virtual GameObject GetRandomPos()
        {
            return null;
        }

        public virtual Vertex[] GetNeighbours(Vertex v)
        {
            if (ReferenceEquals(neighbourVertex, null) || neighbourVertex.Count == 0 ||
                v.id < 0 || v.id >= neighbourVertex.Count)
                return new Vertex[0];
            return neighbourVertex[v.id].ToArray();
        }

        public virtual float[] GetNeighboursCosts(Vertex v)
        {
            if (ReferenceEquals(neighbourVertex, null) || neighbourVertex.Count == 0 ||
                v.id < 0 || v.id >= neighbourVertex.Count)
                return new float[0];

            Vertex[] neighs = neighbourVertex[v.id].ToArray();
            float[] costsV = new float[neighs.Length];
            for (int neighbour = 0; neighbour < neighs.Length; neighbour++) {
                int j = (int)Mathf.Floor(neighs[neighbour].id / numCols);
                int i = (int)Mathf.Floor(neighs[neighbour].id % numCols);
                costsV[neighbour] = costsVertices[j, i];
            }

            return costsV;
        }

        public List<Vertex> GetPathAstar(GameObject srcO, GameObject dstO, Heur h = Heur.Euclidean)
        {

            Vertex srcVertex = this.GetNearestVertex(srcO.transform.position);
            Vertex dstVertex = this.GetNearestVertex(dstO.transform.position);

            List<Vertex> openSet = new List<Vertex>();
            HashSet<Vertex> closedSet = new HashSet<Vertex>();

            openSet.Add(srcVertex);

            while(openSet.Count > 0)
            {
                Vertex currentVertex = openSet[0];
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].cost < currentVertex.cost)
                    {
                        currentVertex = openSet[i];
                    }
                }

                if (currentVertex == dstVertex)
                {
                    return BuildPath(currentVertex);
                }

                openSet.Remove(currentVertex);
                closedSet.Add(currentVertex);

                foreach(Vertex neighbor in GetNeighbours(currentVertex))
                {
                    if (closedSet.Contains(neighbor))
                    {
                        continue;
                    }

                    float gCostPrediction = currentVertex.gCost + Vector3.Distance(currentVertex.transform.position, neighbor.transform.position);

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                    else if (gCostPrediction >= neighbor.gCost)
                    {
                        continue;
                    }

                    neighbor.prevVert = currentVertex;
                    neighbor.gCost = gCostPrediction;
                    neighbor.hCost = GetDistance(neighbor.transform.position, dstVertex.transform.position, h);
                    neighbor.cost = neighbor.gCost + neighbor.hCost;
                }
            }
            return new List<Vertex>();
        }

        private float GetDistance(Vector3 position1, Vector3 position2, Heur h)
        {
            if (h == Heur.Euclidean)
            {
                return new Vector3(position1.x - position2.x, position1.y - position2.y, position1.z - position2.z).magnitude;
            }
            else if (h == Heur.Manhattan)
            {
                return Math.Abs(position1.x - position2.x) + Math.Abs(position1.y - position2.y) + Math.Abs(position1.z - position2.z);
            }
            else
                return 0;
        }

        public List<Vertex> Smooth(List<Vertex> inputPath)
        {
            if (inputPath.Count == 2)
            {
                lengthCamino.text = "2";
                return inputPath;
            }

            List<Vertex> outputPath = new List<Vertex>();
            outputPath.Add(inputPath[0]);

            int inputIndex = 2;
            while (inputIndex < inputPath.Count - 1)
            {
                Vector3 dir = inputPath[inputIndex].transform.position - outputPath[outputPath.Count - 1].transform.position;
                Vector3 startingPos = outputPath[outputPath.Count - 1].transform.position;
                startingPos.y += 0.5f;

                if (Physics.Raycast(startingPos, dir))
                {
                    outputPath.Add(inputPath[inputIndex - 1]);
                }

                inputIndex++;
            }

            outputPath.Add(inputPath[inputPath.Count - 1]);

            lengthCamino.text = outputPath.Count.ToString();
            return outputPath; 
        }

        // Reconstruir el camino, dando la vuelta a la lista de nodos 'padres' /previos que hemos ido anotando
        private List<Vertex> BuildPath(Vertex vertex)
        {
            List<Vertex> path = new List<Vertex>();
            float totalCost = 0;

            while (vertex.prevVert != null)
            {
                path.Add(vertex);
                totalCost += vertex.cost;
                Vertex prev = vertex.prevVert;
                vertex.prevVert = null;
                vertex = prev;

            }

            // path.Reverse();
            baldCaminoText.text = path.Count.ToString();
            lengthCamino.text = path.Count.ToString();
            costCamino.text = totalCost.ToString();
            return path;
        }

    }
}