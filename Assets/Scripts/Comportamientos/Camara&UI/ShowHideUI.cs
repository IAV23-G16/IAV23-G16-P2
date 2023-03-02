using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHideUI : MonoBehaviour
{
    [SerializeField] GameObject[] uiGOs;

    void Update()
    {
        if (Input.GetButtonDown("HideUI"))
        {
            foreach (GameObject go in uiGOs)
            {
                go.SetActive(!go.active);
            }
        }
    }
}
