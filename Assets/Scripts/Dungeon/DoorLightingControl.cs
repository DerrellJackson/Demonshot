using System.Collections;
using UnityEngine;

//THIS WILL BE ACTIVATED LATER BUT FOR OTHER LEVELS
[DisallowMultipleComponent]
public class DoorLightingControl : MonoBehaviour
{
    
    //keep track if it is lit or not
    private bool isLit = false;
    private Door door;

    private void Awake() 
    {

        //get components
        door = GetComponentInParent<Door>();

    }


    //fade in the door
    public void FadeInDoor(Door door) 
    {

        //create new material to fade in
        Material material = new Material(GameResources.Instance.variableLitShader);

        //check if lit or not
        if(!isLit)
        {
            SpriteRenderer[] spriteRendererArray = GetComponentsInParent<SpriteRenderer>();

            foreach(SpriteRenderer spriteRenderer in spriteRendererArray)
            {
                StartCoroutine(FadeInDoorRoutine(spriteRenderer, material));
            }

            isLit = true;
        }

    }


    //fade in door coroutine
    private IEnumerator FadeInDoorRoutine(SpriteRenderer spriteRenderer, Material material)
    {

        spriteRenderer.material = material;

        for(float i = 0.05f; i <= 1f; i += Time.deltaTime / Settings.fadeInTime)
        {
            material.SetFloat("Alpha_Slider", i);
            yield return null;
        }

        spriteRenderer.material = GameResources.Instance.litMaterial;

    }


    //fade door in if triggered
    private void OnTriggerEnter2D(Collider2D collision) 
    {

        FadeInDoor(door);

    }

}
