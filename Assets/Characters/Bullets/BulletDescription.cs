using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class BulletDescription : MonoBehaviour, IPointerEnterHandler, ISelectHandler
{
    public bulletDescriptions bulletdescriptions;
    public int i;

    public void OnPointerEnter(PointerEventData eventData)
    {
        //do your stuff when highlighted
        bulletdescriptions.newDescription(i);
    }

    public void OnSelect(BaseEventData eventData)
    {
        //do your stuff when selected
    }
}
