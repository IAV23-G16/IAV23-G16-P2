/*    
   Copyright (C) 2020-2023 Federico Peinado
   http://www.federicopeinado.com
   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).
   Autor: Federico Peinado 
   Contacto: email@federicopeinado.com
*/
using System.Collections;
using System.Collections.Generic;
using UCM.IAV.Navegacion;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Transform target;
    [SerializeField] Transform generalTargetTr;
    [SerializeField] Transform characterTargetTr;

    public float smoothSpeed = 0.125f;
    float offset = 3.15f;

    private float zoom = 10f;
    private float zoomAmount = 40f;

    private void Start()
    {
        target = characterTargetTr;
    }

    void FixedUpdate()
    {
        if (target != null)
        {
            //Alterna el objetivo de la cámara según input
            if (Input.GetButtonDown("CameraToggle"))
            {
                if (target == characterTargetTr)
                {
                    target = generalTargetTr;
                    zoom = 2f + Mathf.Max(generalTargetTr.parent.GetComponent<GraphGrid>().GetNumRows(), generalTargetTr.parent.GetComponent<GraphGrid>().GetNumCols()) * generalTargetTr.parent.GetComponent<GraphGrid>().cellSize;
                }
                else
                {
                    target = characterTargetTr;
                    zoom = 10f;
                }
            }
            
            Vector3 pos = target.position;
            Vector3 smoothPos = Vector3.Lerp(transform.position, pos, smoothSpeed);

            //Desliza la posición de la cámara hacia el target
            transform.position = new Vector3(smoothPos.x, transform.position.y, smoothPos.z - offset);

            //Alterna la distancia al target en función del input de la rueda del ratón
            HandleZoom();
        }
    }

    private void HandleZoom()
    {
        if (Input.mouseScrollDelta.y > 0)
            zoom -= zoomAmount * Time.deltaTime;
        if (Input.mouseScrollDelta.y < 0)
            zoom += zoomAmount * Time.deltaTime;

        zoom = Mathf.Clamp(zoom, 5f, 50f);
        Camera.main.fieldOfView = zoom;
    }
}