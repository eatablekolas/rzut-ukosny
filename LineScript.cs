using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineScript : MonoBehaviour
{
    // Constants
    const float g = 10f; // m/s^2
    const float default_velocity = 0f;
    const float default_angle = 0f;
    [SerializeField] int positionsPerUnit = 100;
    [SerializeField] int maxPosition = 10;

    // Variables
    float velocity0 = default_velocity;
    float angle0 = default_angle;
    
    // Objects
    [SerializeField] LineRenderer line;
    [SerializeField] Text rangeText;
    [SerializeField] Text heightText;
    
    void Reset(string valueToReset)
    {
        if (valueToReset == "velocity0")
        {
            velocity0 = default_velocity;
        }
        else
        {
            angle0 = default_angle;
        }
        rangeText.text = string.Empty;
        heightText.text = string.Empty;
        line.positionCount = 0;
    }

    void GenerateLine()
    {
        int posCount = maxPosition * positionsPerUnit;
        Vector3[] positions = new Vector3[posCount];

        float v0x = velocity0 * Mathf.Cos(angle0 * Mathf.Deg2Rad);
        float v0y = velocity0 * Mathf.Sin(angle0 * Mathf.Deg2Rad);

        float tk = 2 * v0y / g;

        float Z = Mathf.Round((v0x * tk) * 100) / 100;
        float H = Mathf.Round((v0y * tk/2 - 1f/2f * g * Mathf.Pow(tk/2, 2)) * 100) / 100;

        rangeText.text = "Z = " + Z + "m";
        heightText.text = "H = " + H + "m";

        for (int i = 0; i < posCount; i++) 
        {
            float t = (float)i / positionsPerUnit;
            
            float x = v0x * t;
            float y = v0y * t - 1f/2f * g * Mathf.Pow(t, 2);

            if (x > maxPosition)
            {
                posCount = i + 1;
            }
            else if (y < 0) 
            {
                x = v0x * tk;
                y = 0;

                posCount = i + 1;
            }
            
            positions[i] = new Vector3(x, y, 0);
        }

        line.positionCount = posCount;
        line.SetPositions(positions);
    }

    public void UpdateLine(GameObject inputField)
    {
        GameObject parent = inputField.transform.parent.gameObject;
        string input = inputField.GetComponent<InputField>().text;

        if (parent.name == "Velocity0")
        {
            if (input == "" || input.Contains(".") || input.Contains("-")) 
            {
                Reset("velocity0");
                return;
            }

            velocity0 = float.Parse(input);

            if (angle0 != default_angle && angle0 >= 0 && angle0 <= 90)
            {
                GenerateLine();
            }
        } 
        else 
        {
            if (input == "" || input.Contains(".") || input.Contains("-")) 
            {
                Reset("angle0");
                return;
            }

            angle0 = float.Parse(input);

            if (velocity0 != default_velocity && angle0 >= 0 && angle0 <= 90)
            {
                GenerateLine();
            }
            else
            {
                Reset("angle0");
            }
        }
    }
}
