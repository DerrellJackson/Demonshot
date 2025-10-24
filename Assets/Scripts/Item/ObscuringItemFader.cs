using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ObscuringItemFader : MonoBehaviour
{

    private Coroutine currentCoroutine;
    private SpriteRenderer spriteRenderer;

    private void Awake() 
    {

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

    }


    public void ItemFadeOut() 
    {

        StopCurrentCoroutine();
        currentCoroutine = StartCoroutine(ItemFadeOutRoutine());

    }


    public void ItemFadeIn() 
    {

        StopCurrentCoroutine();
        currentCoroutine = StartCoroutine(ItemFadeInRoutine());    

    }


    private void StopCurrentCoroutine() 
    {

        if(currentCoroutine != null) 
        {
            StopCoroutine(currentCoroutine);
        }

    }


    private IEnumerator ItemFadeInRoutine() 
    {

        float currentAlpha = spriteRenderer.color.a;
        float distance = 1f - currentAlpha;

        while(1f - currentAlpha > 0.01f)
        {
            currentAlpha = currentAlpha + distance/Settings.itemFadeInSeconds * Time.deltaTime;
            spriteRenderer.color = new Color(1f, 1f, 1f, currentAlpha);
            yield return null;
        }
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);

    }


    private IEnumerator ItemFadeOutRoutine() 
    {

        float currentAlpha = spriteRenderer.color.a;
        float distance = currentAlpha - Settings.itemTargetAlpha;

        while(currentAlpha - Settings.itemTargetAlpha > 0.01f)
        {
            currentAlpha = currentAlpha - distance/Settings.itemFadeOutSeconds * Time.deltaTime;
            spriteRenderer.color = new Color(1f, 1f, 1f, currentAlpha);
            yield return null; 
        }
        spriteRenderer.color = new Color(1f, 1f, 1f, Settings.itemTargetAlpha);

    }


}
