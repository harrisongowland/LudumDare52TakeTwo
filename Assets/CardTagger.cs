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
    
    public void SetCard(Card _card)
    {
        card = _card;
        cardImage.sprite = card.CardImage;
        devotion.text = _card.Love.ToString();
        piety.text = _card.Piety.ToString();
        valour.text = _card.Valour.ToString();
    }
    
    public void Tag()
    {
        tagged = !tagged;
        cardImage.color = tagged ? Color.green : Color.white; 
    }
}
