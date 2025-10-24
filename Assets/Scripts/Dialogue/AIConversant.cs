using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Dialogue;

    public class AIConversant : MonoBehaviour
    {
        
        PlayerConversant player;
        PlayerConversant playerConversant;
 
        [SerializeField] string conversantName;
        [SerializeField] DialogueSO dialogue = null;

        public void StartTheDialogue()
        {

            player.StartDialogue(this, dialogue);

        }

    private bool canActivate;//simple bool to determine if the dialogue box can be activated


    void Awake()
    {

     player = GameObject.FindWithTag("Player").GetComponent<PlayerConversant>();
     canActivate = false;

    }
 

    public string GetName() 
    {

        return conversantName;

    }


    // Update is called once per frame
    void Update()
    {
  
        //if the bool is true AND the button is pressed then call the dialogue box
        if (canActivate && Input.GetKeyDown(KeyCode.E))
        {
            StartTheDialogue();
            canActivate = false;
        }
    }
    //allows dialogue to be activated
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            canActivate = true;
        }
    }
    //prevents dialogue from being activated
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            canActivate = false;
        }
        
    }

    }



    