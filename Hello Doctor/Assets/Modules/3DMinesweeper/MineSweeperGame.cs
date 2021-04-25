using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MineSweeperGame : MonoBehaviour
{
    [System.Serializable]
    public class AudioContainer
    {
        public AudioClip clip;
        public Vector2 pitchRange = Vector2.one;
        public void Play()
        {
            Instance.blockAudio.pitch = Random.Range(pitchRange.x, pitchRange.y);
            Instance.blockAudio.clip = clip;
            Instance.blockAudio.Play();
        }
    }
    static MineSweeperGame _instance;
    public static MineSweeperGame Instance { get { if (!_instance) _instance = FindObjectOfType<MineSweeperGame>(); return _instance; } }

    public enum State { Reveal, Flag, Lookup, Dissolve}
    public State state;

    public Button buttonReveal, buttonNote, buttonObserve, buttonDiscuss;
    public int observeCooldown = 3, discussCooldown = 6;
    int currentObserveCooldown, currentDiscussCooldown;


    [Header("Character")]
    public DialogueCharacter character;

    [Header("Audio")]
    public AudioSource blockAudio;
    public AudioContainer revealAudio;

    [Header("Colors")]
    public Color colorRegular = Color.white;
    public Color colorMine = Color.red;
    public Color colorBonus = Color.blue;
    public Color colorFlagged = Color.green;

    private void OnEnable()
    {
        Block.OnBlockClicked.AddListener(HandleBlockClick);
        Application.targetFrameRate = 60;
    }

    private void OnDisable()
    {
        Block.OnBlockClicked.RemoveListener(HandleBlockClick);
    }

    private void Start()
    {
        buttonReveal.onClick.AddListener(() => { state = State.Reveal; });
        buttonNote.onClick.AddListener(() => { state = State.Flag; });
        buttonObserve.onClick.AddListener(() => { state = State.Lookup; });
        buttonDiscuss.onClick.AddListener(() => { state = State.Dissolve; });
        currentObserveCooldown = observeCooldown;
        currentDiscussCooldown = discussCooldown;
    }

    public static void MinePropagation(int amount)
    {
        var mineBlocks = MapGenerator.Instance.blockList.FindAll(x => x.value < 0);
        if (mineBlocks.Count == 0) return;
        for (int i=0; i < amount; i++)
        {
            var mine = mineBlocks[Random.Range(0, mineBlocks.Count)];
            mine.PropagateValueToNeighbours(-1, 0.5f);
            if(mineBlocks.Count < 2) mineBlocks = MapGenerator.Instance.blockList.FindAll(x => x.value < 0);
        }
    }

    void HandleBlockClick(Block block)
    {
        switch (state)
        {
            case State.Reveal:
                //Debug.Log(block.revealed);
                if (block.revealed) return;
                revealAudio.Play();
                block.Reveal();

                CardGame.Clicks++;
                currentDiscussCooldown--;
                currentObserveCooldown--;
                if (currentDiscussCooldown <= 0) buttonDiscuss.interactable = true;
                if (currentObserveCooldown <= 0) buttonObserve.interactable = true;

                if (block.value < 0) {
                    CardGame.MineClick();
                    character.Show(DialogueCharacter.Dialogue.IssuePressed);
                }
                break;
            case State.Lookup:
                revealAudio.Play();
                block.ShowColor();
                buttonObserve.interactable = false;
                currentObserveCooldown = observeCooldown;
                buttonReveal.onClick.Invoke();
                break;
            case State.Flag:
                if (!block.flagged)
                {
                    var rend = block.GetComponent<Renderer>();
                    var mtblock = new MaterialPropertyBlock();
                    rend.GetPropertyBlock(mtblock);
                    mtblock.SetColor("_Color", Color.green);
                    rend.SetPropertyBlock(mtblock);
                    block.flagged = true;
                }
                else
                {
                    if (block.revealed) block.ShowColor();
                    else
                    {
                        var rend = block.GetComponent<Renderer>();
                        var mtblock = new MaterialPropertyBlock();
                        rend.GetPropertyBlock(mtblock);
                        mtblock.SetColor("_Color", colorRegular);
                        rend.SetPropertyBlock(mtblock);
                    }

                    block.flagged = false;
                }
                break;
            case State.Dissolve:
                block.SetValueAndPropagateToSame(0);
                buttonDiscuss.interactable = false;
                currentDiscussCooldown = discussCooldown;
                buttonReveal.onClick.Invoke();
                break;
        }

        CheckIfGameEnded();
    }

    private void CheckIfGameEnded()
    {
        var neutralThemes = MapGenerator.Instance.blockList.FindAll(x => !x.revealed && x.value == 0);
        Debug.Log("Game Ending? " + neutralThemes.Count + " left.");
    }
}
