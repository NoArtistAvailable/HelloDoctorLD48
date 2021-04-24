using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public enum State { Reveal, Flag}
    public State state;

    [Header("Character")]
    public DialogueCharacter character;

    [Header("Audio")]
    public AudioSource blockAudio;
    public AudioContainer revealAudio;

    private void OnEnable()
    {
        Block.OnBlockClicked.AddListener(HandleBlockClick);
        Application.targetFrameRate = 60;
    }

    private void OnDisable()
    {
        Block.OnBlockClicked.RemoveListener(HandleBlockClick);
    }

    void HandleBlockClick(Block block)
    {
        switch (state)
        {
            case State.Reveal:
                revealAudio.Play();
                block.Reveal();
                if (block.value < 0) character.Show(DialogueCharacter.Dialogue.IssuePressed);
                break;
        }
    }
}
