using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPiece : MonoBehaviour
{
    public bool Moving = false;

    [SerializeField] private Spinner m_Spinner; 
    
    [SerializeField] private Transform m_PiecePivot;
    [SerializeField] private int row = 0;
    [SerializeField] private int currentSpace = 0;
    [SerializeField] private float m_TimeBetweenSpaces = 0.25f;
    [SerializeField] private float m_DistanceBetweenSquares = 0.089f;
    private Coroutine m_Move;

    private int m_MovesAlongRow = 0;

    private Vector3 m_StartingPosition;
    
    private void Start()
    {
        m_StartingPosition = m_PiecePivot.localPosition; 
    }

    public void Move(int spaces)
    {
        Moving = true;
        m_Move = StartCoroutine(MovePiece(spaces));
    }

    public void ReturnToStart()
    {
        Debug.Log("Returning to start");
        row = 0;
        currentSpace = 0;
        m_MovesAlongRow = 0; 
        m_PiecePivot.localPosition = m_StartingPosition; 
    }

    private IEnumerator MovePiece(int spaces)
    {
        for (int i = 0; i < spaces; i++)
        {
            switch (row)
            {
                case 0:
                    //Move left
                    m_PiecePivot.localPosition = new Vector3(m_PiecePivot.localPosition.x, m_PiecePivot.localPosition.y,
                        m_PiecePivot.localPosition.z - m_DistanceBetweenSquares);
                    break;
                case 1:
                    //Move back
                    m_PiecePivot.localPosition = new Vector3(m_PiecePivot.localPosition.x + m_DistanceBetweenSquares,
                        m_PiecePivot.localPosition.y, m_PiecePivot.localPosition.z);
                    break;
                case 2:
                    //Move right
                    m_PiecePivot.localPosition = new Vector3(m_PiecePivot.localPosition.x, m_PiecePivot.localPosition.y,
                        m_PiecePivot.localPosition.z + m_DistanceBetweenSquares);
                    break;
                case 3:
                    //Move forward
                    m_PiecePivot.localPosition = new Vector3(m_PiecePivot.localPosition.x - m_DistanceBetweenSquares,
                        m_PiecePivot.localPosition.y, m_PiecePivot.localPosition.z);
                    break;
            }

            m_MovesAlongRow += 1;
            yield return new WaitForSeconds(m_TimeBetweenSpaces);

            if (m_MovesAlongRow < 9) continue;

            m_MovesAlongRow = 0;
            row++;
            if (row > 3)
            {
                row = 0;
                //Reached final square. Stop moving. 
                EventHandlerSystem.instance.FinalSquareReached = true; 
                spaces = 0; 
            }
            
            m_Spinner.Spin(row);
        }

        Moving = false;
        StopCoroutine(m_Move);
    }
}