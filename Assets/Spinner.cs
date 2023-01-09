using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Spinner : MonoBehaviour
{

    public List<float> seasonRotations;
    public float spinTime = 1f; 
    
    public void Spin(int season)
    {
        transform.DORotate(new Vector3(-90, 0, seasonRotations[season]), spinTime);
    }
}
