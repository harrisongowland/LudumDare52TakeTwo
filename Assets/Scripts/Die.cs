using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Die : MonoBehaviour
{

    public Vector3 velocity; 
    
    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = Random.rotation;
        DiceSystem.instance.AddDie(this);
        float force = DiceSystem.instance.m_DiceRollForce;
        if (DiceSystem.instance.m_RandomiseRollForce)
        {
            force = DiceSystem.instance.m_DiceRollForce +
                    Random.Range(-DiceSystem.instance.m_RandomFactor, DiceSystem.instance.m_RandomFactor);
        }
        GetComponent<Rigidbody>().AddForce(DiceSystem.instance.m_PushDirection * force);
    }

    public int Calculate()
    {
        List<float> angles = new List<float>();
        float currentAngle = 0; 
        for (int i = 0; i < 6; i++)
        {
            switch (i)
            {
                case 0:
                    currentAngle = Vector3.Angle(transform.up, Vector3.up);
                    break;
                case 1:
                    currentAngle = Vector3.Angle(-transform.up, Vector3.up);
                    break; 
                case 2:
                    currentAngle = Vector3.Angle(transform.right, Vector3.up);
                    break;
                case 3:
                    currentAngle = Vector3.Angle(-transform.right, Vector3.up);
                    break;
                case 4:
                    currentAngle = Vector3.Angle(transform.forward, Vector3.up);
                    break;
                case 5:
                    currentAngle = Vector3.Angle(-transform.forward, Vector3.up);
                    break;
            }
            angles.Add(currentAngle);
        }

        float minVal = angles.Min();
        int index = angles.IndexOf(minVal);
        
        //We now know which side is up. 

        switch (index)
        {
            case 0:
                index = 5;
                break;
            case 1:
                index = 2;
                break;
            case 2:
                index = 6;
                break;
            case 3:
                index = 1;
                break;
            case 4:
                index = 4;
                break;
            case 5:
                index = 3;
                break; 
        }
        
        return index; 
    }

    void Update()
    {
        velocity = GetComponent<Rigidbody>().velocity; 
    }

    private void OnCollisionEnter(Collision collision)
    {
        GetComponent<AudioSource>().Play();
    }
}
