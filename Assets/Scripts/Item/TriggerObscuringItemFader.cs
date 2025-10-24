using UnityEngine;

public class TriggerObscuringItemFader : MonoBehaviour
{
 
    private void OnTriggerEnter2D(Collider2D collision) 
    {

        //get the game object that is collided with, and then get all the fader components on it and the childs
        
        ObscuringItemFader[] obscuringItemFader = collision.gameObject.GetComponentsInChildren<ObscuringItemFader>();

        if(obscuringItemFader.Length > 0) 
        {
            for(int i = 0; i < obscuringItemFader.Length; i++)
            {
                obscuringItemFader[i].ItemFadeOut();
            }
        }

    }


    private void OnTriggerExit2D(Collider2D collision) 
    {

        //get the gameobject collided with and get all the obscuring item fader components on it and the children
        ObscuringItemFader[] obscuringItemFader = collision.gameObject.GetComponentsInChildren<ObscuringItemFader>();

        if(obscuringItemFader.Length > 0)
        {
            for(int i = 0; i < obscuringItemFader.Length; i++)
            {
                obscuringItemFader[i].ItemFadeIn();
            }
        }

    }


}
