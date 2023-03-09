/*    
   Copyright (C) 2020-2023 Federico Peinado
   http://www.federicopeinado.com
   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).
   Autor: Federico Peinado 
   Contacto: email@federicopeinado.com
*/
namespace UCM.IAV.Movimiento
{

    using UnityEngine;
    using UnityEngine.EventSystems;

    /// <summary>
    /// Clara para el comportamiento de agente que consiste en ser el jugador
    /// </summary>
    public class ControlJugador: ComportamientoAgente
    {
        [SerializeField] Camera mainCam;
        [SerializeField] float distanceWalkCloseLimit;
        [SerializeField] LayerMask layerMask;
        Vector3 targetPos;

        private void Start()
        {
            targetPos = transform.position;
        }

        /// <summary>
        /// Obtiene la dirección
        /// </summary>
        /// <returns></returns>
        public override Direccion GetDireccion()
        {
            Direccion direccion = new Direccion();

            // Al pulsar ratón
            if (Input.GetButton("WalkToPoint"))
            {
                // Si no pulsamos la UI
                if (EventSystem.current.IsPointerOverGameObject()) return direccion;

                // Pillamos el punto del suelo en el que pulsamos
                Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                Physics.Raycast(ray, out hit, 100, layerMask);

                targetPos = hit.point;
            }

            // Direccion actual
            direccion.lineal.x = targetPos.x - transform.position.x;
            direccion.lineal.z = targetPos.z - transform.position.z;

            // Deadzone, para que no ande alante y atrás al llegar a su destino
            if (Mathf.Abs(direccion.lineal.x) < distanceWalkCloseLimit && Mathf.Abs(direccion.lineal.z) < distanceWalkCloseLimit)
            {
                direccion.lineal.x = 0;
                direccion.lineal.z = 0;
            }

            //Resto de cálculo de movimiento
            direccion.lineal.Normalize();
            direccion.lineal *= agente.aceleracionMax;

            return direccion;
        }
    }
}