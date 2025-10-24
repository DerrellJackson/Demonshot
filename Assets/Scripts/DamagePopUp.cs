using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//REMEMBER TO ADD A CRIT SYSTEM!!
public class DamagePopUp : MonoBehaviour
{   
    public GameObject pfDamagePopup;

    public static DamagePopUp Create(Vector3 position, int damageAmount)
    {   
        Transform DamagePopUpTransform = Instantiate(GameAssets.i.pfDamagePopup, position, Quaternion.identity);
        DamagePopUp damagePopup = DamagePopUpTransform.GetComponent<DamagePopUp>();
        damagePopup.Setup(damageAmount);

        return damagePopup;  
    }

   // private const float DISAPPEAR_TIMER_MAX = 1f;

    private TextMeshPro textMesh;
    private float disappearTimer;
    private Color textColor;

    private void Awake() 
    {
        textMesh = transform.GetComponent<TextMeshPro>();
        textColor = textMesh.color;                                                                                                             
    }

   public void Setup(int damageAmount)
   {
    textMesh.SetText(damageAmount.ToString());
    textColor = textMesh.color;
   // disappearTimer = DISAPPEAR_TIMER_MAX;
   }

   private void Update() {
    float moveYSpeed = 1f; // speed it moves up by
    transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;//how to get it to move up
//if i want it to bob up and down just remove the note
    //if(disappearTimer > DISAPPEAR_TIMER_MAX * 4f)
    //{
        //first half of the popup
       // float increaseScaleAmount = 0.10f;
       // transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;

   // }// else
   // {
        //second half of the lifetime
      //   float decreaseScaleAmount = 0.05f;
       // transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
        
   // }
    disappearTimer -= Time.deltaTime;
    if(disappearTimer < 0)
    {   
        float disappearSpeed = 2.5f;
        textColor.a -= disappearSpeed * Time.deltaTime;
        textMesh.color = textColor;
        if(textColor.a < 0)
        {
            Destroy(gameObject);
        }
    }
   }
}
