using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class SimpleScript : MonoBehaviour
{
    public Color[] colors = new Color[] { Color.black, Color.magenta };

    public TextMeshPro status;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        status.text = SRHydraInput.CurrentMouse.ToString();

        RaycastHit hit;
        Ray r = Camera.main.ViewportPointToRay(SRHydraInput.CurrentMouse, Camera.MonoOrStereoscopicEye.Left);
        if (Physics.Raycast(r, out hit))
        {
            if (SRHydraInput.GetKey(0x01)) //Left Mouse Button
            {
                Debug.Log("LeftMouse Button ");
                status.text = SRHydraInput.CurrentMouse.ToString()+ "LeftMouse Button ";

                if (GetComponent<Renderer>() != null)
                {
                    GetComponent<Renderer>().material.SetColor("_Color", colors[0]);//SetColor("_Color", UnityEngine.Color.magenta);
                }
            }

            else if (SRHydraInput.GetKey(0x02)) //Right Mouse Button
            {
                Debug.Log("RightMouse Button ");
                status.text = SRHydraInput.CurrentMouse.ToString() + "RightMouse Button ";

                if (GetComponent<Renderer>() != null)
                {
                    GetComponent<Renderer>().material.SetColor("_Color", colors[1]);
                }
            }

            else if (SRHydraInput.GetKey(0x04)) //Middle Mouse Button
            {

                Debug.Log("MiddleMouse Button ");
                status.text = SRHydraInput.CurrentMouse.ToString() + "MiddleMouse Button ";

                if (GetComponent<Renderer>() != null)
                {
                    GetComponent<Renderer>().material.SetColor("_Color", colors[0]);
                }
            }
        }

        UnityEngine.Debug.DrawRay(r.origin, r.origin + r.direction * 100, UnityEngine.Color.red);

        if (SRHydraInput.GetKey(0x20))
        {

            Debug.Log("SpaceKey ");
            status.text = SRHydraInput.CurrentMouse.ToString() + "SpaceKey ";

            if (GetComponent<Renderer>() != null)
            {
                GetComponent<Renderer>().material.SetColor("_Color", colors[1]);
            }
        }
    }
}
