using UnityEngine;
using UnityEngine.UIElements;

public class DropdownController : MonoBehaviour
{
    [SerializeField]
    private UIDocument uIDocument;

    protected void Start()
    {
        // Find the dropdown element
        var dropdown = uIDocument.rootVisualElement.Q<DropdownField>();

        // I can set available options
        dropdown.choices = new() { "Option 1", "Option 2", "Option 3" };

        // I can set the current value
        dropdown.index = 1;

        // But I cannot expand or collapse the dropdown

        // There is BasePopupField<string, string>.ShowMenu() method, but it is internal.
        //dropdown.ShowMenu();

        // And there is no method there to close the menu again.
    }
}
