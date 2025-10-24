using System.Collections;
using UnityEngine;

public class ItemNudge : MonoBehaviour
{
 
    private WaitForSeconds pause;
    private bool isAnimating = false;
    [SerializeField] public GameObject particleEffectPrefab;
    private WaitForSeconds pauseAmountForParticles;

    private void Awake() 
    {

        pause = new WaitForSeconds(0.02f);
        if(particleEffectPrefab != null)
        {
        particleEffectPrefab.SetActive(false);
        }

    }


    private void OnTriggerStay2D(Collider2D collision) 
    {
        
        if(isAnimating == false) 
        {
            if(gameObject.transform.position.x < collision.gameObject.transform.position.x)
            {
                StartCoroutine(RotateAntiClock());
            }
            else 
            {
                StartCoroutine(RotateClock());
            }
        }
            
    }


    private void OnTriggerExit2D(Collider2D collision) 
    {

        if(isAnimating == false) 
        {
            if(gameObject.transform.position.x > collision.gameObject.transform.position.x) 
            {
                StartCoroutine(RotateAntiClock());
            }
            else 
            {
                StartCoroutine(RotateClock());
            }
        }

    }


    private IEnumerator RotateAntiClock()
    {

        isAnimating = true; 
        if(particleEffectPrefab != null)
        {
            particleEffectPrefab.SetActive(true);
        }

        for(int i = 0; i < 4; i++)
        {
            gameObject.transform.GetChild(0).Rotate(0f, 0f, 1.5f);
        
            yield return pause;
        }

        for(int i = 0; i < 5; i++) //rotate in 5 steps 2* clockwise
        {
            gameObject.transform.GetChild(0).Rotate(0f, 0f, -1.5f);

            yield return pause;
        }

        gameObject.transform.GetChild(0).Rotate(0f, 0f, 1.5f);
        yield return pause;
        isAnimating = false;

    }


    //rotate clockwise
    private IEnumerator RotateClock() 
    {
        
        isAnimating = true; 
        if(particleEffectPrefab != null)
        {
        particleEffectPrefab.SetActive(true);
        }
        for(int i = 0; i < 4; i++)
        {
            gameObject.transform.GetChild(0).Rotate(0f, 0f, -1.5f);

            yield return pause;
        }

        for(int i = 0; i < 5; i++) //rotate in 5 steps 1.5* clockwise
        {
            gameObject.transform.GetChild(0).Rotate(0f, 0f, 1.5f);

            yield return pause;
        }

        gameObject.transform.GetChild(0).Rotate(0f, 0f, -1.5f);
        yield return pause;
        isAnimating = false;

    }

    

}
