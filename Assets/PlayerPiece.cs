using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPiece : MonoBehaviour
{
    [SerializeField] private int row = 0;
    [SerializeField] private int currentSpace = 0; 

    public void Move(int spaces)
    {
        for (int i = 0; i < spaces; i++)
        {
            switch (row)
            {
                case 0:
                    //Move left
            }
        }
    }
}
