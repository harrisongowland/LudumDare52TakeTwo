using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardTagger : MonoBehaviour
{
    public Image cardImage; 
    
    public Card card; 
    public bool tagged = false;

    public TextMeshProUGUI devotion;
    public TextMeshProUGUI piety;
    public TextMeshProUGUI valour;

    public TextMeshProUGUI buttonContent; 

    private enum CardTaggerMode
    {
        TAG, ADD, PLACE, REMOVE, DISCARD
    }

    private CardTaggerMode mode; 
    
    public void SetCard(Card _card, int _mode)
    {
        card = _card;
        cardImage.sprite = card.CardImage;
        devotion.text = $"Devotion: {_card.Love}";
        piety.text = $"Piety: {_card.Piety}";
        valour.text = $"Valour: {_card.Valour}";

        mode = _mode switch
        {
            0 => CardTaggerMode.TAG,
            1 => CardTaggerMode.ADD,
            2 => CardTaggerMode.PLACE,
            3 => CardTaggerMode.REMOVE,
            _ => CardTaggerMode.DISCARD
        };
        
        buttonContent.text = _mode switch
        {
            0 => "Collect",
            1 => "Add",
            2 => "Trade",
            3 => "Remove",
            _ => "Discard"
        };
    }
    
    public void Tag()
    {
        switch (mode)
        {
            case CardTaggerMode.TAG or CardTaggerMode.PLACE:
                tagged = !tagged;
                cardImage.color = tagged ? Color.green : Color.white;
                break;
            case CardTaggerMode.ADD:
                EventHandlerSystem.instance.PlaceCard(this.card);
                break;
            case CardTaggerMode.REMOVE:
                EventHandlerSystem.instance.RemoveCard(this.card);
                break; 
            case CardTaggerMode.DISCARD:
                EventHandlerSystem.instance.DiscardCard(this.card);
                break; 
        }
    }
}
