using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class EventHandlerSystem : MonoBehaviour
{
    public static EventHandlerSystem instance;

    public List<Card> currentCards;
    public int maxCards = 9;
    
    public bool DiscardRequired = false;
    public bool FinalSquareReached = false; 

    public List<Card> gatherableCards;
    public List<Card> taggedCards;
    public List<Card> placedCards;

    private BoardEvent m_CurrentEvent;
    public List<BoardEvent> allEvents;
    public List<BoardEvent> playedEvents; 

    public int maxPersonnel = 4;
    public int availablePersonnel = 4;
    public int temporaryPersonnelPenalty = 0;

    public UnityEvent OnCardScreenContinued;

    //Card screen mode data
    public enum CardScreenMode
    {
        ADD,
        TRADE,
        REMOVE,
        DISCARD
    }

    public CardScreenMode CurrentCardScreenMode;

    //Card screen UI
    public CanvasGroup CardScreen;
    public Transform CardScreenCardContainerA;
    public Transform CardScreenCardContainerB;

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

    //Prepare UI
    public CanvasGroup PrepareCanvas;
    public GameObject CardsInHandRequired;
    public GameObject CardsInMiddleRequired;

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

    public void FullReset()
    {
        currentCards.Clear();
        taggedCards.Clear();
        placedCards.Clear(); 
        
        availablePersonnel = maxPersonnel;
        temporaryPersonnelPenalty = 0; 
        FindObjectOfType<PlayerPiece>().ReturnToStart();
    }

    public void ShowEventCanvas(bool show)
    {
        EventCanvas.alpha = show ? 1 : 0;
        EventCanvas.interactable = show;
        EventCanvas.blocksRaycasts = show;
    }

    public void DisplayEvent(bool ignorePlayedEvents)
    {
        BoardEvent testEvent = allEvents[Random.Range(0, allEvents.Count)];
        if (ignorePlayedEvents && allEvents.Count != playedEvents.Count)
        {
            if (playedEvents.Contains(testEvent))
            {
                DisplayEvent(ignorePlayedEvents);
                return;
            }
        }
        DisplayEvent(testEvent);
    }

    public void DisplayEvent(BoardEvent boardEvent)
    {
        playedEvents.Add(boardEvent);
        
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
            availablePersonnel -= boardEvent.AgentsRequired;
        }

        float testSuccess = Random.Range(0, 100);
        if (testSuccess <= boardEvent.chanceOfSuccess)
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

        TestCardAmount();
    }

    public void TestCardAmount()
    {
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

    public void SetCanvasVisibility(CanvasGroup canvas, bool visible)
    {
        canvas.alpha = visible ? 1f : 0f;
        canvas.interactable = visible;
        canvas.blocksRaycasts = visible;
    }

    public void SetGatherCanvasVisibility(bool visible)
    {
        SetCanvasVisibility(GatherCanvas, visible);
    }

    public void SetPrepareCanvasVisibility(bool visible)
    {
        SetCanvasVisibility(PrepareCanvas, visible);
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

            NormaliseCardPrefab(newCardTagger);

            newCardTagger.GetComponent<CardTagger>().SetCard(card, 0);
        }
    }

    private void NormaliseCardPrefab(GameObject _prefab)
    {
        _prefab.transform.localPosition = new Vector3(_prefab.transform.localPosition.x,
            _prefab.transform.localPosition.y, 0);
        _prefab.transform.localEulerAngles = Vector3.zero;
        _prefab.transform.localScale = Vector3.one;
    }

    public void ProcessGather()
    {
        foreach (Transform o in CardContainer)
        {
            Destroy(o.gameObject);
        }
        
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

    public void Prepare()
    {
        SetPrepareCanvasVisibility(true);
        CardsInHandRequired.SetActive(currentCards.Count != 0);
        CardsInMiddleRequired.SetActive(placedCards.Count != 0);
    }

    public void DisplayCardScreen(bool mode)
    {
        SetCanvasVisibility(CardScreen, true);
        SetPrepareCanvasVisibility(false);
        if (CurrentCardScreenMode is not CardScreenMode.REMOVE)
        {
            UpdateHand(CurrentCardScreenMode == CardScreenMode.ADD ? 1 : CurrentCardScreenMode == CardScreenMode.TRADE ? 2 : 3);
            return;
        }

        DisplayPlacedCards();
    }

    private void ClearCardScreenContainers()
    {
        foreach (Transform o in CardScreenCardContainerA)
        {
            Destroy(o.gameObject);
        }

        foreach (Transform o in CardScreenCardContainerB)
        {
            Destroy(o.gameObject);
        }
    }

    private void UpdateHand(int mode)
    {
        ClearCardScreenContainers();

        for (int i = 0; i < currentCards.Count; i++)
        {
            InstantiateCard(currentCards[i], i, mode);
        }
    }

    private void DisplayPlacedCards()
    {
        ClearCardScreenContainers();

        for (int i = 0; i < placedCards.Count; i++)
        {
            InstantiateCard(placedCards[i], i, 3);
        }
    }

    private void InstantiateCard(Card _card, int index, int mode)
    {
        GameObject newCard =
            Instantiate(CardPrefab, index < 4 ? CardScreenCardContainerA : CardScreenCardContainerB,
                true) as GameObject;
        newCard.GetComponent<CardTagger>().SetCard(_card, mode);
        NormaliseCardPrefab(newCard);
    }

    public void PlaceCard(Card _card)
    {
        placedCards.Add(_card);
        currentCards.Remove(_card);
        OnCardScreenContinue();
    }

    public void RemoveCard(Card _card)
    {
        placedCards.Remove(_card);
        OnCardScreenContinue();
    }

    public void TradeCards()
    {
        List<Card> tradedCards = new List<Card>();
        tradedCards.Clear(); 

        foreach (CardTagger c in CardScreenCardContainerA.GetComponentsInChildren<CardTagger>())
        {
            if (!c.tagged) continue;

            tradedCards.Add(c.card);
            currentCards.Remove(c.card);
        }

        foreach (CardTagger c in CardScreenCardContainerB.GetComponentsInChildren<CardTagger>())
        {
            if (!c.tagged) continue;

            tradedCards.Add(c.card);
            currentCards.Remove(c.card);
        }

        Debug.Log("Number of traded cards: " + tradedCards.Count);
        
        for (int i = 0; i < tradedCards.Count; i++)
        {
            currentCards.Add(gatherableCards[Random.Range(0, gatherableCards.Count)]);
        }

        OnCardScreenContinue(); 
    }

    public void DiscardCard(Card _card)
    {
        bool found = false; 
        foreach (CardTagger c in CardScreenCardContainerA.GetComponentsInChildren<CardTagger>())
        {
            if (c.card == _card)
            {
                RemoveCardFromCardTagger(c);

                found = true; 
                break; 
            }
        }

        if (!found)
        {
            foreach (CardTagger c in CardScreenCardContainerB.GetComponentsInChildren<CardTagger>())
            {
                if (c.card == _card)
                {
                    RemoveCardFromCardTagger(c);
                    break; 
                }
            }
        }

        if (!DiscardRequired)
        {
            //Continue
            OnCardScreenContinue();
        }
    }

    private void RemoveCardFromCardTagger(CardTagger cardTagger)
    {
        currentCards.Remove(cardTagger.card);
        Destroy(cardTagger.gameObject);
        TestCardAmount();
    }

    public void OnCardScreenContinue()
    {
        OnCardScreenContinued.Invoke();
    }
}