using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEvent : MonoBehaviour
{
    [NonSerialized] public string DisplayText;
    [SerializeField][TextArea] private string _eventText;
    [SerializeField][TextArea] private string _badEndText;
    [SerializeField][TextArea] private string _goodEndText;

    public string ActionText;

    [NonSerialized] public string QuitButtonDisplayText;
    [SerializeField] private string _quitButtonGoodText;
    [SerializeField] private string _quitButtonBadText;

    public int SuceedChance;

    public void BaseDisplay()
    {
        DisplayText = _eventText;
        QuitButtonDisplayText = "";
    }

    public virtual void BadEnding()
    {
        DisplayText = _badEndText;

        QuitButtonDisplayText = _quitButtonBadText;
        SoundHandeler.Instance.PlayBumBadEvent();
    }

    public virtual void GoodEnding()
    {
        DisplayText = _goodEndText;

        QuitButtonDisplayText = _quitButtonGoodText;
        SoundHandeler.Instance.PlayBumGoodEvent();
    }
}
