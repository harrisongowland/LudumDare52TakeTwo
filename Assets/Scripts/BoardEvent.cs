using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Board Event", menuName = "Board Event")]
public class BoardEvent : ScriptableObject
{
    public string EventName;
    [TextArea(3,10)]
    public string Description;

    public string ChoiceA;
    public string ChoiceB;
    
    [TextArea(3,10)]
    public string PositiveOutcomeDescription;
    
    [TextArea(3,10)]
    public string NegativeOutcomeDescription;

    public int CardsReceivedOnSuccess = 2;

    [Tooltip("The player can win x number of cards from this pool")]
    public List<Card> CardPool; 

    public bool AreAgentsRequired = false; 
    public int AgentsRequired = 0;

    public float chanceOfSuccess = 50;

    public enum EventOnFailure
    {
        LOSE_CARDS, FREEZE_PERSONNEL
    }

    public EventOnFailure FailureEvent;

    public int CardsLostOnFailure = 1;
}
