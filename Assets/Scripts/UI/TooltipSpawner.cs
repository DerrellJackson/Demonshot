using UnityEngine;
using UnityEngine.EventSystems;

namespace Demonshot.UI.TooltipsInvUI
{


    // Override the abstract functions to create a tooltip spawner for your own data
    
    public abstract class TooltipSpawner : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
       

        [Tooltip("The prefab of the tooltip to spawn.")]
        [SerializeField] GameObject tooltipPrefab = null;

        GameObject tooltip = null;

        // The spawned tooltip prefab for updating.
        public abstract void UpdateTooltip(GameObject tooltip);

        // Return true when the tooltip spawner should be allowed to create a tooltip.
        public abstract bool CanCreateTooltip();


        private void OnDestroy()
        {
            ClearTooltip();
        }

        private void OnDisable()
        {
            ClearTooltip();
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            var parentCanvas = GetComponentInParent<Canvas>();

            if (eventData.button != PointerEventData.InputButton.Right)
            {
            if (tooltip && !CanCreateTooltip())
            {
                ClearTooltip();
            }

            if (!tooltip && CanCreateTooltip())
            {
                tooltip = Instantiate(tooltipPrefab, parentCanvas.transform);
            }

            if (tooltip)
            {
                UpdateTooltip(tooltip);
                PositionTooltip();
            }
            }
        }


        private void PositionTooltip()
        {
            // Required to ensure corners are updated by positioning elements.
            Canvas.ForceUpdateCanvases();

            tooltip.transform.position = new Vector2(960f, 730f);
        }


        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {

            ClearTooltip();

        }

        private void ClearTooltip()
        {
            if (tooltip)
            {
                Destroy(tooltip.gameObject);
            }
        }
    }
}