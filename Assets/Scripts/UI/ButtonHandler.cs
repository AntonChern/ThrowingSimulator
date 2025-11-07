using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Sprite highlight;
    [SerializeField] private Sprite lowlight;
    private Animator animator;
    private Image image;

    private void Start()
    {
        animator = GetComponent<Animator>();
        image = GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        animator.SetBool("highlighted", true);
        image.sprite = highlight;
        //Debug.Log("Enter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetBool("highlighted", false);
        image.sprite = lowlight;
        //Debug.Log("Out");
    }
}
