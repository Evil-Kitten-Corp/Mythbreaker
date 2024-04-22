using TMPro;
using UnityEngine;

public class ObservableTSlot : TSlot
{
    public TMP_Text stacksText;

    public override void Start()
    {
        base.Start();
        
        Debug.Log(debugAbility.GetType().Name);

        if (debugAbility is AgnisAbility ab)
        {
            Debug.Log("WORKS");
            ab.OnStackChange += UpdateStacks;
        }
    }

    private void UpdateStacks()
    {
        if (debugAbility is AgnisAbility ab)
        {
            stacksText.text = ab.currentStacks.ToString();
        }
    }
}