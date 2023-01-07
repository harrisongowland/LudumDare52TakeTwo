using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceSystem : MonoBehaviour
{
    public static DiceSystem instance;

    [SerializeField] private GameObject m_Die;
    [SerializeField] private int m_NumberOfDice = 2; //Defaults to 2
    [SerializeField] private List<Transform> m_DiceSpawnPoints; 

    [SerializeField] private Vector3 m_PushDirection; 
    [SerializeField] private float m_DiceRollForce = 100;
    [SerializeField] private bool m_RandomiseRollForce = false;
    [SerializeField] private float m_RandomFactor = 10; 

    public void Start()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void Roll()
    {
        int index = 0;
        List<GameObject> allDice = new List<GameObject>();

        foreach (Transform t in m_DiceSpawnPoints)
        {
            GameObject newDie = Instantiate(m_Die) as GameObject;
            newDie.transform.position = t.position; 
            allDice.Add(newDie);
        }
        
        //All dice placed. 

        foreach (GameObject o in allDice)
        {
            //Push!
            if (m_RandomiseRollForce)
            {
                o.GetComponent<Rigidbody>().AddForce(m_PushDirection * Random.Range(m_DiceRollForce - m_RandomFactor, m_DiceRollForce + m_RandomFactor));
                return;
            }
            o.GetComponent<Rigidbody>().AddForce(m_PushDirection * m_DiceRollForce);
        }
    }
}
