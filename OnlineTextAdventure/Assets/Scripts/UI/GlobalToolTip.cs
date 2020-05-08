using UnityEngine;
using TMPro;
using Extensions.Generics.Singleton;

public class GlobalToolTip : GenericSingleton<GlobalToolTip, IToolTip>, IToolTip
{
    private TextMeshProUGUI textField;

    protected override void Awake()
    {
        base.Awake();
        textField = GetComponent<TextMeshProUGUI>();
        textField.raycastTarget = false;
        DeactivateToolTip();
    }

    public void ActivateToolTip(string text, Transform trans)
    {
        textField.text = text;
        this.transform.position = trans.position;
        gameObject.SetActive(true);
    }

    public void DeactivateToolTip()
    {
        gameObject.SetActive(false);
    }
}

public interface IToolTip
{
    void ActivateToolTip(string text, Transform trans);

    void DeactivateToolTip();
}
