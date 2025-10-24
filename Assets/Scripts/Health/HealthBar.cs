using UnityEngine;

public class HealthBar : MonoBehaviour
{

   #region Header GameObject References
   [Space(10)]
   #endregion Header GameObject References

   #region Tooltip
   [Tooltip("Populate with the child Bar gameObject")]
   #endregion Tooltip
   [SerializeField] private GameObject healthBar;

   
   //enable the health bar 
   public void EnableHealthBar()
   {
        
        gameObject.SetActive(true);
        

   }


    //disable the healthbar
    public void DisableHealthBar() 
    {

        gameObject.SetActive(false);

    }


    //set health bar value with a precentage between 0 and 1
    public void SetHealthBarValue(float healthPercent)
    {

        healthBar.transform.localScale = new Vector3(healthPercent, 1f, 1f);
       
    }



}
