using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHideUI : MonoBehaviour
{
    [SerializeField] GameObject[] uiGOs;
    bool active = true;

    void Update()
    {
        if (Input.GetButtonDown("HideUI"))
        {
            foreach (GameObject go in uiGOs)
            {
                if (active)
                    go.SetActive(!go.active);
                else
                    go.SetActive(go.active);
            }
        }
    }
}
