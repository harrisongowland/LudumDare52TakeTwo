using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EventHandlerSystem : MonoBehaviour
{

    public static EventHandlerSystem instance;

    public List<Card> currentCards;
    public int maxCards = 9;
    public bool DiscardRequired = false;

    private BoardEvent m_CurrentEvent; 
    
    public int maxPersonnel = 4; 
    public int availablePersonnel = 4;
    public int temporaryPersonnelPenalty = 0;

    public CanvasGroup EventCanvas; 
    public TextMeshProUGUI EventTitle;
    public TextMeshProUGUI EventDescription;
    public TextMeshProUGUI ChoiceA;
    public TextMeshProUGUI ChoiceB;

    public void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this; 
        DontDestroyOnLoad(this.gameObject);
        currentCards = new List<Card>();
    }

    public void Reset()
    {
        availablePersonnel = maxPersonnel - temporaryPersonnelPenalty;
        temporaryPersonnelPenalty = 0; 
        ShowEventCanvas(false);
    }

    public void ShowEventCanvas(bool show)
    {
        EventCanvas.alpha = show ? 1 : 0;
        EventCanvas.interactable = show;
        EventCanvas.blocksRaycasts = show;
    }
    
    public void DisplayEvent(BoardEvent boardEvent)
    {
        ShowEventCanvas(true);
        m_CurrentEvent = boardEvent;
        
        EventTitle.text = boardEvent.EventName;
        EventDescription.text = boardEvent.Description;
        ChoiceA.text = boardEvent.ChoiceA;
        ChoiceB.text = boardEvent.ChoiceB; 
        
        ChoiceA.gameObject.SetActive(true);
        ChoiceB.gameObject.SetActive(true);
    }

    public void AttemptEvent()
    {
        AttemptEvent(m_CurrentEvent);
    }
    
    public void AttemptEvent(BoardEvent boardEvent)
    {
        if (boardEvent.AreAgentsRequired)
        {
            availablePersonnel = availablePersonnel - boardEvent.AgentsRequired;
        }

        float testSuccess = Random.Range(0, 100);
        if (testSuccess >= boardEvent.chanceOfSuccess)
        {
            AddCards(WinEvent(boardEvent));
            return;
        }
        
        LoseEvent(boardEvent);
    }
    
    public List<Card> WinEvent(BoardEvent boardEvent)
    {
        List<Card> newCards = new List<Card>();
        for (int i = 0; i < boardEvent.CardsReceivedOnSuccess; i++)
        {
            newCards.Add(boardEvent.CardPool[Random.Range(0, boardEvent.CardPool.Count)]);
        }

        EventTitle.text = boardEvent.EventName;
        EventDescription.text = boardEvent.PositiveOutcomeDescription;
        ChoiceA.gameObject.SetActive(false);
        ChoiceB.gameObject.SetActive(false);
        
        return newCards; 
    }

    public void LoseEvent(BoardEvent boardEvent)
    {
        if (boardEvent.FailureEvent == BoardEvent.EventOnFailure.LOSE_CARDS)
        {
            //Handle discard logic
        }
        else
        {
            //Freeze personnel
            temporaryPersonnelPenalty = boardEvent.AgentsRequired;
        }
    }

    public void AddCards(List<Card> newCards)
    {
        foreach (Card card in newCards)
        {
            currentCards.Add(card);
        }

        if (currentCards.Count > maxCards)
        {
            Debug.Log("Player has too many cards. They must discard some.");
            DiscardRequired = true; 
        }
        else
        {
            DiscardRequired = false; 
        }
    }

    public void UpdateMaxPersonnel(int personnelChange)
    {
        maxPersonnel += personnelChange; 
    }
}