using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class HoverOverDisplay : MonoBehaviour
{
    static HoverOverDisplay _instance;
    public static HoverOverDisplay Instance { get { if (!_instance) _instance = FindObjectOfType<HoverOverDisplay>(); return _instance; } }

    [System.NonSerialized]
    public HoverOver displayed;

    public RectTransform hoverBoard;
    public Text tooltipInfo;

    private void Update()
    {
        if (displayed)
        {
            hoverBoard.position = Input.mousePosition;
        }
    }

    public static void ShowTooltip(HoverOver hoverOver)
    {
        Instance.hoverBoard.gameObject.SetActive(true);
        Instance.displayed = hoverOver;
        Instance.tooltipInfo.text = Regex.Unescape(hoverOver.message);
        if (hoverOver.transform.position.x > Screen.width / 2f)
            Instance.hoverBoard.pivot = new Vector2(1f, Instance.hoverBoard.pivot.y);
        else
            Instance.hoverBoard.pivot = new Vector2(0f, Instance.hoverBoard.pivot.y);
    }

    public static void HideTooltip(HoverOver hoverOver)
    {
        if (Instance.displayed != hoverOver) return;
        Instance.hoverBoard.gameObject.SetActive(false);
        Instance.displayed = null;
    }
}
