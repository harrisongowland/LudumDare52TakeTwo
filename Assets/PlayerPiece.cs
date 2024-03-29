using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPiece : MonoBehaviour
{
    public bool Moving = false;

    [SerializeField] private Transform m_PiecePivot; 
    [SerializeField] private int row = 0;
    [SerializeField] private int currentSpace = 0;
    [SerializeField] private float m_TimeBetweenSpaces = 0.25f;
    [SerializeField] private float m_DistanceBetweenSquares = 0.089f;
    private Coroutine m_Move;

    private int m_MovesAlongRow = 0; 
    
    public void Move(int spaces)
    {
        Moving = true; 
        m_Move = StartCoroutine(MovePiece(spaces));
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
            }
        }

        Moving = false;
        StopCoroutine(m_Move);
    }
}
