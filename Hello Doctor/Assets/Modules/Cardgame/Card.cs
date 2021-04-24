using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="LD48/Card")]
public class Card : ScriptableObject
{
    [System.Serializable]
    public struct Cost
    {
        public CardGame.Ressource ressource;
        public int amount;
    }

    [Header("Settings")]
    public Cost[] costs;
    public int charges = 5;
    public CardEffect[] effects;
    public string description;
    [System.NonSerialized]
    public Card original;
    
    public Card GetInstance()
    {
        var card = ScriptableObject.CreateInstance<Card>();
        card.name = name;
        card.costs = costs;
        card.charges = charges;
        card.effects = effects;
        card.description = description;
        card.original = this;
        return card;
    }

    public bool IsPlayable()
    {
        foreach(var c in costs)
        {
            if (CardGame.Instance.GetRessource(c.ressource) < c.amount)
                return false;
        }
        return true;
    }

    public void Play()
    {
        foreach(var effect in effects)
        {
            effect.Invoke();
        }
        charges--;
        if (charges <= 0) ChargesDepleted();
    }

    public void ChargesDepleted()
    {

    }
    
}
