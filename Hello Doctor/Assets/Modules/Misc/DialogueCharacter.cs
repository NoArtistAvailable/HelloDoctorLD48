using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "LD48/Character")]
public class DialogueCharacter : ScriptableObject
{
    public enum Dialogue { Greeting, IssuePressed, MemoryGood, MemoryBad, HighStress, LowStress, HighTrust, LowTrust, GameOver, Win}

    public string[] greetings;
    public string[] issuesGotPressed;
    public string[] memoryGood;
    public string[] memoryBad;
    public string[] highStress;
    public string[] lowStress;
    public string[] highTrust;
    public string[] lowTrust;
    public string[] gameOver;
    public string[] win;

    public string Get(Dialogue type)
    {
        switch (type)
        {
            case Dialogue.Greeting:
                return greetings[Random.Range(0, greetings.Length)];
            case Dialogue.IssuePressed:
                return issuesGotPressed[Random.Range(0, issuesGotPressed.Length)];
            case Dialogue.MemoryGood:
                return memoryGood[Random.Range(0, memoryGood.Length)];
            case Dialogue.MemoryBad:
                return memoryBad[Random.Range(0, memoryBad.Length)];
            case Dialogue.HighStress:
                return highStress[Random.Range(0, highStress.Length)];
            case Dialogue.LowStress:
                return lowStress[Random.Range(0, lowStress.Length)];
            case Dialogue.HighTrust:
                return highTrust[Random.Range(0, highTrust.Length)];
            case Dialogue.LowTrust:
                return lowTrust[Random.Range(0, lowTrust.Length)];
            case Dialogue.GameOver:
                return gameOver[Random.Range(0, gameOver.Length)];
            case Dialogue.Win:
                return gameOver[Random.Range(0, win.Length)];
            default: return "..";
        }
    }

    public void Show(Dialogue type)
    {
        string message = Get(type);
        TextboxManager.ShowText(message);
    }

}
