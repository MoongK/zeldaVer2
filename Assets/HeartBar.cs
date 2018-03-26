using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartBar : MonoBehaviour {
    public GameObject heartPrefab;
    public Transform heartHolder;

    public int totalHealth;
    public int CurrentHealth { get; private set; }

    const int healthPerHeart = 4;

    public List<Heart> list;

    private void Awake() {
        list = new List<Heart> ();
        CurrentHealth = totalHealth;
        int j = 0;
        int remainHealth = totalHealth;
        while(remainHealth > 0) {

            Heart Element;
            if (remainHealth < healthPerHeart ) {
                //list.Add(new Heart(remainHealth));
                //print(list[j++].glowImages.Length);

                Element = GameObject.Find("HeartHolder").AddComponent<Heart>();
                Element.glowImages = new Image[remainHealth];
                list.Add(Element);
                print(Element.glowImages.Length);

            } else {
                //list.Add(new Heart(healthPerHeart));
                //print(list[j++].glowImages.Length);

                Element = GameObject.Find("HeartHolder").AddComponent<Heart>();
                Element.glowImages = new Image[healthPerHeart];
                list.Add(Element);
                print(Element.glowImages.Length);

            }
            remainHealth -= healthPerHeart;
        }

        foreach(var h in list) {
            GameObject clone = Instantiate ( heartPrefab , heartHolder );
            h.foregroundImage = clone.transform.Find ( "Background/Foreground" ).GetComponent<Image> ();
            h.glow = ( RectTransform ) clone.transform.Find ( "Glow" );
            h.glow.localScale = h.glow.localScale * 1.5f;
            for(int i = 0 ; i < h.glowImages.Length ; i++ ) {
                h.glowImages [ i ] = h.glow.GetChild ( i ).GetComponent<Image> ();
            }

            h.TurnOffAllGlow ();
        }

        FillAllHeart ();
    }

    private void FillAllHeart() {
        int i = 0;
        foreach(var h in list) {
            if( i / healthPerHeart < CurrentHealth / healthPerHeart) {
                h.foregroundImage.fillAmount = 1f;
                i += healthPerHeart;
                continue;
            }
            if(i / healthPerHeart == CurrentHealth / healthPerHeart) {
                h.foregroundImage.fillAmount = ( float ) ( CurrentHealth - i ) / healthPerHeart;
                i += healthPerHeart;
                continue;
            }
            if(i / healthPerHeart > CurrentHealth / healthPerHeart ) {
                h.foregroundImage.fillAmount = 0f;
                i += healthPerHeart;
                continue;
            }
        }
    }

    private void FillGlow( int from , int to ) {
        for ( int i = from ; i <= to ; i++ ) {
            Heart h = GetHeartAtHealthPoint ( i );
            h.TurnOnGlowAt ( i % healthPerHeart );
        }
    }

    private Heart GetHeartAtHealthPoint( int i ) {
        return list [ i / healthPerHeart ];
    }

    public void DecreaseHp( int amount ) {
        StartCoroutine ( AnimDecreaseHP ( amount ) );
    }

    private IEnumerator AnimDecreaseHP( int amount ) {
        if(amount <= 0 || CurrentHealth == 0) {
            yield break;
        }

        int last = CurrentHealth-1;
        int first = CurrentHealth > amount ? CurrentHealth - amount : 0;

        FillGlow ( first , last );

        yield return StartCoroutine ( AnimGlow () );

        DecHP ( amount );
        FillAllHeart ();

        for(int i = last ; i >= first ; i-- ) {
            Image glow = GetGlowImageAtHealthPoint ( i );

            glow.fillClockwise = false; 
            float begin = 0;
            float f = 1f;
            while ( f > begin ) {
                glow.fillAmount = f;
                f -= 5f * ( last - first +1 ) * Time.deltaTime;
                yield return null;
            }
            glow.fillAmount = begin;
        }
    }

    private Image GetGlowImageAtHealthPoint( int i ) {
        return GetHeartAtHealthPoint ( i ).glowImages [ i % healthPerHeart ];
    }

    private void DecHP( int amount ) {
        CurrentHealth -= amount;
        CurrentHealth = Mathf.Clamp ( CurrentHealth , 0 , totalHealth );
    }
    private void IncHP( int amount ) {
        CurrentHealth += amount;
        CurrentHealth = Mathf.Clamp ( CurrentHealth , 0 , totalHealth );
    }

    private IEnumerator AnimGlow() {
        foreach ( var h in list ) {
            h.glow.localScale = Vector3.one * 1.9f;
        }

        yield return new WaitForSeconds ( 0.6f );

        foreach ( var h in list ) {
            h.glow.localScale = Vector3.one * 1.45f;
        }
        yield return null;
    }

    public void IncreaseHp( int amount ) {
        StartCoroutine ( AnimIncreaseHP ( amount ) );
    }

    private IEnumerator AnimIncreaseHP( int amount ) {
        if ( amount <= 0 || CurrentHealth == totalHealth ) {
            yield break;
        }

        int first = CurrentHealth;
        int last = CurrentHealth + amount <= totalHealth ? CurrentHealth + amount - 1 : totalHealth - 1;

        FillGlow ( first , last );

        yield return StartCoroutine ( AnimGlow() );

        IncHP ( amount );
        FillAllHeart ();

        for (int i = first ; i <= last ; i++ ) {
            Image glow = GetGlowImageAtHealthPoint ( i );

            glow.fillClockwise = true;
            float end = 0;
            float f = 1f;
            while(f > end) {
                glow.fillAmount = f;
                f -= 5f * ( last - first + 1 ) * Time.deltaTime;
                yield return null;
            }
            glow.fillAmount = end;
        }

        yield return null;
    }
}

//public class Heart
//{
//    public Image foregroundImage;
//    public Image[] glowImages;
//    public RectTransform glow;

//    //public Heart Setglow(int maxHealth)
//    //{
//    //    glowImages = new Image[maxHealth];

//    //    return this;
//    //}

//    public Heart(int maxHealth)
//    {
//        glowImages = new Image[maxHealth];
//    }

//    public void TurnOnGlowAt(int i)
//    {
//        glowImages[i].enabled = true;
//        glowImages[i].fillAmount = 1;
//    }

//    public void TurnOffGlowAt(int i)
//    {
//        glowImages[i].enabled = false;
//        glowImages[i].fillAmount = 0;
//    }

//    public void TurnOnAllGlow()
//    {
//        for (int i = 0; i < glowImages.Length; i++)
//        {
//            TurnOnGlowAt(i);
//        }
//    }

//    public void TurnOffAllGlow()
//    {
//        for (int i = 0; i < glowImages.Length; i++)
//        {
//            TurnOffGlowAt(i);
//        }
//    }
//}
