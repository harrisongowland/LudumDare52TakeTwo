using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class EventHandlerSystem : MonoBehaviour
{
    public static EventHandlerSystem instance;

    public List<Card> currentCards;
    public int maxCards = 9;
    public bool DiscardRequired = false;
    public List<Card> gatherableCards;
    public List<Card> taggedCards;

    private BoardEvent m_CurrentEvent;

    public int maxPersonnel = 4;
    public int availablePersonnel = 4;
    public int temporaryPersonnelPenalty = 0;

    //Event UI
    public CanvasGroup EventCanvas;
    public TextMeshProUGUI EventTitle;
    public TextMeshProUGUI EventDescription;
    public TextMeshProUGUI ChoiceA;
    public TextMeshProUGUI ChoiceB;

    //Gather UI
    public CanvasGroup GatherCanvas;
    public Transform CardContainer;
    public GameObject CardPrefab;

    public void Start()
    {
        taggedCards = new List<Card>();
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
        EventTitle.text = boardEvent.EventName;
        EventDescription.text = boardEvent.NegativeOutcomeDescription;
        ChoiceA.gameObject.SetActive(false);
        ChoiceB.gameObject.SetActive(false);

        if (boardEvent.FailureEvent == BoardEvent.EventOnFailure.LOSE_CARDS)
        {
            LoseCards(boardEvent.CardsLostOnFailure);
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

    private void LoseCards(int numberOfCards)
    {
        if (currentCards.Count == 0)
        {
            return;
        }

        for (var i = 0; i < numberOfCards; i++)
        {
            currentCards.RemoveAt(Random.Range(0, currentCards.Count));
            if (currentCards.Count == 0)
            {
                break;
            }
        }
    }

    public void LoseCard(int index)
    {
        currentCards.RemoveAt(index);
    }

    public void UpdateMaxPersonnel(int personnelChange)
    {
        maxPersonnel += personnelChange;
    }

    public void SetGatherCanvasVisibility(bool visible)
    {
        GatherCanvas.alpha = visible ? 1f : 0f;
        GatherCanvas.interactable = visible;
        GatherCanvas.blocksRaycasts = visible; 
    }
    
    public void Gather()
    {
        taggedCards.Clear();
        SetGatherCanvasVisibility(true);
        
        List<Card> gatheredCards = new List<Card>();
        for (int i = 0; i < availablePersonnel; i++)
        {
            gatheredCards.Add(gatherableCards[Random.Range(0, gatherableCards.Count)]);
        }

        foreach (Card card in gatheredCards)
        {
            GameObject newCardTagger = Instantiate(CardPrefab) as GameObject;
            newCardTagger.transform.SetParent(CardContainer, true);
            
            newCardTagger.transform.localPosition = new Vector3(newCardTagger.transform.localPosition.x,
                newCardTagger.transform.localPosition.y, 0);
            newCardTagger.transform.localEulerAngles = Vector3.zero;
            newCardTagger.transform.localScale = Vector3.one; 
            
            newCardTagger.GetComponent<CardTagger>().SetCard(card);
        }
    }

    public void ProcessGather()
    {
        SetGatherCanvasVisibility(false);
        List<Card> addedCards = new List<Card>();
        foreach (CardTagger c in CardContainer.GetComponentsInChildren<CardTagger>())
        {
            if (c.tagged)
            {
                addedCards.Add(c.card);
            }
        }

        if (addedCards.Count == 0)
        {
            return;
        }

        AddCards(addedCards);
    }
}