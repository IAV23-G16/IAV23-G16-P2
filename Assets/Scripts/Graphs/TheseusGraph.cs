﻿/*    
   Copyright (C) 2020-2023 Federico Peinado
   http://www.federicopeinado.com
   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).
   Autor: Federico Peinado 
   Contacto: email@federicopeinado.com
*/
using UCM.IAV.Movimiento;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace UCM.IAV.Navegacion
{


    // Posibles algoritmos para buscar caminos en grafos
    public enum TesterGraphAlgorithm
    {
        BFS, DFS, ASTAR
    }

    public class TheseusGraph : MonoBehaviour
    {
        [SerializeField]
        protected Graph graph;

        [SerializeField]
        protected Teseo theseus;

        [SerializeField]
        private TesterGraphAlgorithm algorithm;

        [SerializeField]
        private bool smoothPath;

        [SerializeField]
        private string vertexTag = "Vertex"; // Etiqueta de un nodo normal

        [SerializeField]
        private string obstacleTag = "Wall"; // Etiqueta de un obstáculo, tipo pared...

        [SerializeField]
        private Color pathColor;

        [SerializeField]
        [Range(0.1f, 1f)]
        private float pathNodeRadius = .3f;

        private bool ariadna;

        Heur currentHeuristic = Heur.Euclidean;

        bool firstHeuristic = true;
        Camera mainCamera;
        protected GameObject srcObj;
        protected GameObject dstObj;
        protected List<Vertex> path; // La variable con el camino calculado

        protected LineRenderer hilo;
        protected float hiloOffset = 0.2f;

        // Despertar inicializando esto
        public virtual void Awake()
        {
            mainCamera = Camera.main;
            srcObj = GameManager.instance.GetPlayer();
            dstObj = null;
            path = new List<Vertex>();
            hilo = GetComponent<LineRenderer>();
            ariadna = false;

            hilo.startWidth = 0.15f;
            hilo.endWidth = 0.15f;
            hilo.positionCount = 0;
        }

        // Update is called once per frame
        public virtual void Update()
        {
            if (Input.GetKey(KeyCode.Mouse1))
            {
                if (!ariadna)
                    updateAriadna(true);
            }
            else
            {
                if (ariadna)
                    updateAriadna(false);
            }


            if (Input.GetKeyDown(KeyCode.S))
                smoothPath = !smoothPath;

            if (ariadna)
            {
                //Source jugador y destino el nodo final
                if (srcObj == null) srcObj = GameManager.instance.GetPlayer();
                if (dstObj == null) dstObj = GameManager.instance.GetExitNode();

                //path = new List<Vertex>();

                switch (algorithm)
                {
                    case TesterGraphAlgorithm.ASTAR:
                    default:
                        path = graph.GetPathAstar(srcObj, dstObj, currentHeuristic); // COMO SEGUNDO ARGUMENTO SE DEBERÍA PASAR LA HEURÍSTICA
                        break;
                }
                if (smoothPath)
                    path = graph.Smooth(path); // Suavizar el camino, una vez calculado

                if (path.Count > 0)
                {
                    // GameManager.instance.SetPlayerNode(path[path.Count - 1].transform);

                    DibujaHilo();
                }
            }
        }

        public virtual Transform GetNextNode()
        {
            if (path.Count > 0)
                return path[path.Count - 1].transform;

            return null;
        }

        // Dibujado de artilugios en el editor
        // OJO, ESTO SÓLO SE PUEDE VER EN LA PESTAÑA DE SCENE DE UNITY
        virtual public void OnDrawGizmos()
        {
            if (!Application.isPlaying)
                return;

            if (ReferenceEquals(graph, null))
                return;

            Vertex v;
            if (!ReferenceEquals(srcObj, null))
            {
                Gizmos.color = Color.green; // Verde es el nodo inicial
                v = graph.GetNearestVertex(srcObj.transform.position);
                Gizmos.DrawSphere(v.transform.position, pathNodeRadius);
            }
            if (!ReferenceEquals(dstObj, null))
            {
                Gizmos.color = Color.red; // Rojo es el color del nodo de destino
                v = graph.GetNearestVertex(dstObj.transform.position);
                Gizmos.DrawSphere(v.transform.position, pathNodeRadius);
            }
            int i;
            Gizmos.color = pathColor;
            for (i = 0; i < path.Count; i++)
            {
                v = path[i];
                Gizmos.DrawSphere(v.transform.position, pathNodeRadius);
                if (smoothPath && i != 0)
                {
                    Vertex prev = path[i - 1];
                    Gizmos.DrawLine(v.transform.position, prev.transform.position);
                }
            }
        }

        // Mostrar el camino calculado
        public void ShowPathVertices(List<Vertex> path, Color color)
        {
            int i;
            for (i = 0; i < path.Count; i++)
            {
                Vertex v = path[i];
                Renderer r = v.GetComponent<Renderer>();
                if (ReferenceEquals(r, null))
                    continue;
                r.material.color = color;
            }
        }

        // Cuantificación, cómo traduce de posiciones del espacio (la pantalla) a nodos
        private GameObject GetNodeFromScreen(Vector3 screenPosition)
        {
            GameObject node = null;
            Ray ray = mainCamera.ScreenPointToRay(screenPosition);
            RaycastHit[] hits = Physics.RaycastAll(ray);
            foreach (RaycastHit h in hits)
            {
                if (!h.collider.CompareTag(vertexTag) && !h.collider.CompareTag(obstacleTag))
                    continue;
                node = h.collider.gameObject;
                break;
            }
            return node;
        }

        // Dibuja el hilo de Ariadna
        public virtual void DibujaHilo()
        {
            hilo.positionCount = path.Count + 1;
            hilo.SetPosition(0, new Vector3(srcObj.transform.position.x, srcObj.transform.position.y + hiloOffset, srcObj.transform.position.z));

            for (int i = path.Count - 1; i >= 0; i--)
            {
                Vector3 vertexPos = new Vector3(path[i].transform.position.x, path[i].transform.position.y + hiloOffset, path[i].transform.position.z);
                hilo.SetPosition(path.Count - i, vertexPos);
            }
        }

        void updateAriadna(bool ar)
        {
            ariadna = ar;
            hilo.enabled = ariadna;
            theseus.GetComponent<SeguirCamino>().enabled = ariadna;
        }

        public string ChangeHeuristic()
        {
            if (currentHeuristic == Heur.Euclidean)
                currentHeuristic = Heur.Manhattan;
            else
                currentHeuristic = Heur.Euclidean;
            return currentHeuristic == Heur.Euclidean ? "Euclidean" : "Manhattan";
        }

        public virtual void ResetPath()
        {
            path = null;
        }

    }
}
