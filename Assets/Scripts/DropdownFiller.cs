using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DropdownFiller : MonoBehaviour
{

    [SerializeField, Tooltip("How many entries to generate")]
    private int dropdownEntries = 25;

    protected void OnEnable()
    {
        var document = GetComponent<UIDocument>();
        var root = document.rootVisualElement;
        var dropdown = root.Q<DropdownField>("Dropdown");

        // Generate 25 entries
        var entries = new List<string>();
        for (int i = 1; i <= dropdownEntries; i++)
        {
            entries.Add($"Entry {i}");
        }
        dropdown.choices = entries;
    }
}
