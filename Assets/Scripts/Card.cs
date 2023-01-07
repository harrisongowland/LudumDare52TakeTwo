using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Card")]
public class Card : ScriptableObject
{
    public string CardName;
    
    //Stats
    public int Piety;
    public int Valour;
    public int Love;

    public Sprite CardImage; 
}
