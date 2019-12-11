using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public Color startColor;
    public Color highLightColor;


    public void Pressed()
    {
        /*
        Color CurrentColor = GetComponent<Renderer>().material.GetColor("_Albedo");
        if(CurrentColor==startColor)
            GetComponent<Renderer>().material.SetColor("_Albedo", highLightColor);

        if (CurrentColor == highLightColor)
            GetComponent<Renderer>().material.SetColor("_Albedo", startColor);
        */
    }

    public void Swiped()
    {
        Vector2 touchPos = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad, OVRInput.GetActiveController());



        if (OVRInput.Get(OVRInput.Button.One))
        {
            if (gameObject.CompareTag("Block"))
            {
                if (touchPos.x > 0.0f)
                {
                    transform.Rotate(Vector3.up, -1.0f);
                }
                else if (touchPos.x < 0.0f)
                {
                    transform.Rotate(Vector3.up, 1.0f);
                }
                if (touchPos.y > 0.0f)
                {
                    transform.Translate(Vector3.forward * -0.5f);
                }
                else if (touchPos.y < 0.0f)
                {
                    transform.Translate(Vector3.forward * 0.5f);
                }
            }
            if (gameObject.CompareTag("GrabbableBlock"))
            {
                Color CurrentColor = GetComponent<Renderer>().material.GetColor("_Albedo");
                if (CurrentColor == startColor)
                    GetComponent<Renderer>().material.SetColor("_Albedo", highLightColor);

                if (CurrentColor == highLightColor)
                    GetComponent<Renderer>().material.SetColor("_Albedo", startColor);
            }

        }
    }

    
}
