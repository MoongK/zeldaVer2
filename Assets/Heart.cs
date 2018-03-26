using System;
using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{
    public Image foregroundImage;
    public Image[] glowImages;
    public RectTransform glow;

    //public Heart Setglow(int maxHealth)
    //{
    //    glowImages = new Image[maxHealth];

    //    return this;
    //}

    public Heart(int maxHealth)
    {
        glowImages = new Image[maxHealth];
    }

    public void TurnOnGlowAt(int i)
    {
        glowImages[i].enabled = true;
        glowImages[i].fillAmount = 1;
    }

    public void TurnOffGlowAt(int i)
    {
        glowImages[i].enabled = false;
        glowImages[i].fillAmount = 0;
    }

    public void TurnOnAllGlow()
    {
        for (int i = 0; i < glowImages.Length; i++)
        {
            TurnOnGlowAt(i);
        }
    }

    public void TurnOffAllGlow()
    {
        for (int i = 0; i < glowImages.Length; i++)
        {
            TurnOffGlowAt(i);
        }
    }
}