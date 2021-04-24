using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGame : MonoBehaviour
{
    static CardGame _instance;
    public static CardGame Instance { get { if (!_instance) _instance = FindObjectOfType<CardGame>(); return _instance; } }
    public enum Ressource { Clicks, Observations, Memories, Questions, Trust, Stress }
    public int clicks, observations, memories, questions, trust, stress;

    public List<Card> deck;

    public int GetRessource(Ressource type)
    {
        switch (type)
        {
            case Ressource.Clicks:
                return clicks;
            case Ressource.Observations:
                return observations;
            case Ressource.Memories:
                return memories;
            case Ressource.Questions:
                return questions;
            case Ressource.Trust:
                return trust;
            case Ressource.Stress:
                return stress;
            default: return 0;
        }
    }
}
