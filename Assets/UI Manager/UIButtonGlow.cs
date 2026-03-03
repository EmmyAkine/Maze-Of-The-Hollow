using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UIButtonGlow : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject glowObject;

    private void OnEnable()
    {
        Refresh();
    }
    private void Update()
    {
        // Safety net for selection changes that don't fire events
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            if (!glowObject.activeSelf)
                glowObject.SetActive(true);
        }
        else
        {
            if (glowObject.activeSelf)
                glowObject.SetActive(false);
        }
    }
    public void OnDeselect(BaseEventData eventData)
    {
        glowObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        glowObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (EventSystem.current == null || glowObject == null) return;

        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            glowObject.SetActive(false);
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        glowObject.SetActive(true);
    }

    private void Refresh()
    {
        glowObject.SetActive(
            EventSystem.current.currentSelectedGameObject == gameObject
        );
    }
}
