using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CardGame : MonoBehaviour
{
    static CardGame _instance;
    public static CardGame Instance { get { if (!_instance) _instance = FindObjectOfType<CardGame>(); return _instance; } }
    public enum Ressource { Clicks, Observations, Memories, Solving, Trust, Stress }
    public int clicks, observations, memories, notes, solvings, trust, stress;

    public int stressBreakAt = 25, trustBreakAt = 25;
    public Vector2Int stressOnClick = Vector2Int.one;
    public Vector2Int stressOnMineClick = Vector2Int.one * 20;
    public Vector2Int trustOnClock = Vector2Int.one;
    public Vector2Int trustOnBonus = Vector2Int.one * 15;

    public static int Clicks
    {
        get => Instance.clicks;
        set {
            Instance.clicks = value;
            Instance.text_Clicks.text = Instance.clicks.ToString();
            Stress += Random.Range(Instance.stressOnClick.x, Instance.stressOnClick.y+1);
            Trust += Random.Range(Instance.trustOnClock.x, Instance.trustOnClock.y+1);
        }
    }
    public static int Observations
    {
        get => Instance.observations;
        set { Instance.observations = value; Instance.text_Observations.text = Instance.observations.ToString(); }
    }
    public static int Notes
    {
        get => Instance.notes;
        set { Instance.notes = value; Instance.text_Notes.text = Instance.notes.ToString(); }
    }
    public static int Solvings
    {
        get => Instance.solvings;
        set { Instance.solvings = value; Instance.text_Discuss.text = Instance.solvings.ToString(); }
    }
    public static int Stress
    {
        get => Instance.stress;
        set
        {
            var before = Instance.stress;// % Instance.trustBreakAt;
            Instance.stress = value;
            Instance.text_Stress.text = Instance.stress.ToString();
            if (Instance.stress % Instance.stressBreakAt < before % Instance.stressBreakAt)
            {
                int result = Mathf.CeilToInt((float)(Instance.stress - before) / (float)Instance.stressBreakAt);
                MineSweeperGame.MinePropagation(result);
                MineSweeperGame.Instance.character.Show(DialogueCharacter.Dialogue.MemoryBad);
            }
        }
    }
    public static int Trust
    {
        get => Instance.trust;
        set
        {
            var trustBefore = Instance.trust;// % Instance.trustBreakAt;
            Instance.trust = value;
            Instance.text_Trust.text = Instance.trust.ToString();
            if (Instance.trust % Instance.trustBreakAt < trustBefore % Instance.trustBreakAt)
            {
                int result = Mathf.CeilToInt((float)(Instance.trust - trustBefore) / (float)Instance.trustBreakAt);
                Solvings+=result;
            }
        }
    }

    public static void MineClick()
    {
        Stress += Random.Range(Instance.stressOnMineClick.x, Instance.stressOnMineClick.y + 1);
    }

    public Text text_Clicks, text_Observations, text_Notes, text_Discuss, text_Trust, text_Stress, text_Memories;

    public List<Card> deck;

    private void Start()
    {
        Clicks = clicks;
        Observations = observations;
        Notes = notes;
        Solvings = solvings;
    }

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
            case Ressource.Solving:
                return solvings;
            case Ressource.Trust:
                return trust;
            case Ressource.Stress:
                return stress;
            default: return 0;
        }
    }
}
