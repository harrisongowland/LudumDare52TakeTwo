using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Windows.Speech;

public class DiceSystem : MonoBehaviour
{
    public static DiceSystem instance;

    [SerializeField] private GameObject m_Die;
    [SerializeField] private int m_NumberOfDice = 2; //Defaults to 2
    [SerializeField] private List<Transform> m_DiceSpawnPoints;

    [SerializeField] public Vector3 m_PushDirection; 
    [SerializeField] public float m_DiceRollForce = 100;
    [SerializeField] public bool m_RandomiseRollForce = false;
    [SerializeField] public float m_RandomFactor = 10;

    [SerializeField] private List<Die> dice;

    [SerializeField] private float timeToWaitUntilCalculatingDice = 1;

    [SerializeField] private UnityEvent m_OnDiceFinishedRolling; 
    
    private bool m_TestingDie = false;
    private bool m_FirstDie = false;
    private Coroutine m_DiceWait; 

    private List<int> options; 
    
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

        for (int i = 0; i < m_NumberOfDice; i++)
        {
            GameObject newDie = Instantiate(m_Die) as GameObject;
            newDie.transform.position = m_DiceSpawnPoints[i].position;
        }
    }

    public void AddDie(Die die)
    {
        dice.Add(die);
        if (!m_FirstDie)
        {
            m_FirstDie = true;
            m_DiceWait = StartCoroutine(WaitBeforeStartingToCalculateDice());
        }
    }

    private void Update()
    {
        if (!m_TestingDie)
        {
            return;
        }

        bool diceStopped = true;
        
        foreach (Die die in dice)
        {
            if (die.velocity != Vector3.zero)
            {
                //This die has stopped
                diceStopped = false;
                break; 
            }
        }

        if (diceStopped)
        {
            m_TestingDie = false;
            m_FirstDie = false;

            //Calculate dice
            options = new List<int>();
            foreach (Die die in dice)
            {
                options.Add(die.Calculate());
            }

            if (options.Count <= 2)
            {
                //There are only two or fewer dice available. Calculate the whole amount.
            }
            else
            {
                //There are more than two dice. The player can choose which dice to take.
            }

            m_DiceWait = StartCoroutine(WaitBeforeMovingOn());
        }
    }
    
    private IEnumerator WaitBeforeMovingOn()
    {
        yield return new WaitForSeconds(timeToWaitUntilCalculatingDice);
        foreach (Die die in dice)
        {
            Destroy(die.gameObject);
        }
        m_OnDiceFinishedRolling.Invoke();
        StopCoroutine(m_DiceWait);
    }

    private IEnumerator WaitBeforeStartingToCalculateDice()
    {
        yield return new WaitForSeconds(timeToWaitUntilCalculatingDice);
        m_TestingDie = true;
        StopCoroutine(m_DiceWait);
    }
}
