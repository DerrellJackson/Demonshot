using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(MaterializeEffect))]
public class ChestItem : MonoBehaviour
{
    
    private SpriteRenderer spriteRenderer;
    private TextMeshPro textTMP;
    private MaterializeEffect materializeEffect;
    [HideInInspector] public bool isItemMaterialized = false;                                                                                                                 


    private void Awake() 
    {

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        textTMP = GetComponentInChildren<TextMeshPro>();
        materializeEffect = GetComponent<MaterializeEffect>();

    }


    //initialize the chest item
    public void Initialize(Sprite sprite, string text, Vector3 spawnPosition, Color materializeColor)
    {

        spriteRenderer.sprite = sprite;
        transform.position = spawnPosition;

        StartCoroutine(MaterializeItem(materializeColor, text));

    }


    //materialize the chest item
    private IEnumerator MaterializeItem(Color materializeColor, string text) 
    {

        SpriteRenderer[] spriteRendererArray = new SpriteRenderer[] { spriteRenderer };

        yield return StartCoroutine(materializeEffect.MaterializeRoutine(GameResources.Instance.materializeShader, materializeColor, 1f, spriteRendererArray, GameResources.Instance.litMaterial));
        //may use dif shader depending on chest

        isItemMaterialized = true;

        textTMP.text = text;

    }

}
