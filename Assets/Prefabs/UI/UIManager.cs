using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] CanvasGroup gameplayControl;
    [SerializeField] CanvasGroup gameplayMenu;

    void SetCanvasGroupEnabled(CanvasGroup group,bool enabled)
    {
        group.blocksRaycasts = enabled;
        group.interactable = enabled;
    }

    public void SetGameplayControlEnabled(bool enabled)
    {
        SetCanvasGroupEnabled (gameplayControl, enabled);
    }

    public void SetGameplayMenuEnabled(bool enabled)
    {
        SetCanvasGroupEnabled (gameplayMenu, enabled);
    }
}
