using elZach.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class TextboxManager : MonoBehaviour
{
    [System.Serializable]
    public class Voice
    {
        public AnimationCurve[] curves = new AnimationCurve[] { new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(0.3f, 1.05f), new Keyframe(1f, 1f)) };
        float basePitch = 1f;
        public Vector2 baseRange = new Vector2(1f, 1f);

        public AudioSource output;

        public int GetRandomIndex()
        {
            basePitch = Random.Range(baseRange.x, baseRange.y);
            return Random.Range(0, curves.Length);
        }

        public void Invoke(int curveIndex, float progress)
        {
            output.pitch = basePitch * curves[curveIndex].Evaluate(progress);
            output.Play();
        }
    }

    static TextboxManager _instance;
    public static TextboxManager Instance { get { if (!_instance) _instance = FindObjectOfType<TextboxManager>(); return _instance; } }

    public ClickableUI textBox;
    public Text text;

    public int waitFrames = 6;

    public string sampleText;
    public UnityEvent OnLetter;
    
    public Voice voice;

    bool abortClick = false;
    Coroutine typeRoutine;

    private void Start()
    {
        textBox.gameObject.SetActive(false);
        textBox.OnClick.AddListener(HandleAbort);
    }

    void HandleAbort()
    {
        if (typeRoutine != null)
            abortClick = true;
        else textBox.gameObject.SetActive(false);
    }

    [Button("Sample Text")]
    void Sample()
    {
        ShowText(sampleText);
    }

    public static void ShowText(string message, System.Action doAfter=null)
    {
        if (Instance.typeRoutine != null) Instance.StopCoroutine(Instance.typeRoutine);
        Instance.typeRoutine = Instance.StartCoroutine(Instance.TypeText(message, doAfter));
    }

    IEnumerator TypeText(string message, Action action)
    {
        textBox.gameObject.SetActive(true);
        int tonal = voice.GetRandomIndex();
        float progress = 0f;
        for (int letter = 0; letter <= message.Length; letter++)
        {
            progress = (float)letter / (float)message.Length;
            text.text = message.Substring(0, letter);
            voice.Invoke(tonal, progress);
            OnLetter.Invoke();
            for (int i = 0; i < waitFrames; i++)
            {
                if (abortClick) break;
                yield return null;
            }
            if (abortClick) break;
        }
        text.text = message;
        abortClick = false;
        if (action != null) {
            while (!abortClick) yield return null;
            action.Invoke();
            textBox.gameObject.SetActive(false);
        }
        typeRoutine = null;
    }
}
