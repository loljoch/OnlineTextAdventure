using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


[RequireComponent(typeof(Selectable))]
public class UINavigation : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private const KeyCode downKey = KeyCode.DownArrow;
    private const KeyCode upKey = KeyCode.UpArrow;

    public Selectable onSelectUp, onSelectDown;

    [HideInInspector] public Selectable selectable;
    private bool isFocused = false;
    private static WaitForSeconds waitTime = new WaitForSeconds(0f);

    private void Awake()
    {
        selectable = GetComponent<Selectable>();
    }

    private void Update()
    {
        if (!isFocused) return;

        if (Input.GetKeyDown(upKey))
        {
            DeselectSelf();
            onSelectUp.Select();

        } else if (Input.GetKeyDown(downKey))
        {
            DeselectSelf();
            onSelectDown.Select();
        }

    }

    private void DeselectSelf()
    {
        selectable.interactable = false;
        selectable.interactable = true;
    }

    public void OnSelect(BaseEventData eventData)
    {
        StartCoroutine(SelectWaitTime());
    }

    private IEnumerator SelectWaitTime()
    {
        yield return waitTime;
        isFocused = true;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        isFocused = false;
    }
}
