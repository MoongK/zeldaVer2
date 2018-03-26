using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartTest : MonoBehaviour {

    public Button takeDamageButton, healButton;

    public HeartBar bar;

    private void Awake()
    {
        takeDamageButton.onClick.AddListener(() => bar.DecreaseHp(4));
        healButton.onClick.AddListener(() => bar.IncreaseHp(3));
    }
}
