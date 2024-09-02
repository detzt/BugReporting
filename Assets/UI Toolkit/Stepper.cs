using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// A Stepper control.
/// </summary>
[UxmlElement]
public partial class Stepper : BaseField<int> {

    // BEM class names
#pragma warning disable IDE1006 // lowercase name is predefined by Unity
    public static new readonly string ussClassName = "stepper";
    public static new readonly string inputUssClassName = ussClassName + "__input";
    public static readonly string optionUssClassName = ussClassName + "__option";
    public static readonly string arrowUssClassName = ussClassName + "__arrow";
#pragma warning restore IDE1006

    // References

    private readonly Label optionLabel = new();
    private readonly List<Label> arrows = new();

    // Properties

    [UxmlAttribute, Tooltip("The options to step through.")]
    public List<string> Options {
        get => options;
        set {
            options = value;
            UpdateVisuals();
            NotifyPropertyChanged(nameof(Options));
        }
    }
    private List<string> options = new();


    // Public API

    /// <summary>Constructs a Stepper without a label.</summary>
    public Stepper() : this(null) { }

    /// <summary>Constructs a Stepper with a label.</summary>
    public Stepper(string label) : base(label, null) {
        AddToClassList(ussClassName);

        // Use the BaseField's input field as container for the interactive elements
        var input = this.Q(className: BaseField<bool>.inputUssClassName);
        input.AddToClassList(inputUssClassName);

        // Add the arrows
        for(int i = 0; i < 2; i++) {
            var arrow = new Label();
            arrow.text = i == 0 ? "<" : ">";
            arrow.AddToClassList(arrowUssClassName);
            arrow.usageHints = UsageHints.DynamicColor;
            arrows.Add(arrow);

            input.Add(arrow);
        }

        // Add the options container
        optionLabel = new Label();
        optionLabel.AddToClassList(optionUssClassName);
        input.Add(optionLabel);

        // Set right arrow as last child
        arrows[1].BringToFront();

        input.style.flexDirection = FlexDirection.Row;

        // Add the options
        UpdateVisuals();
    }

    // Private methods

    [EventInterest(typeof(ClickEvent), typeof(NavigationMoveEvent))]
    protected override void HandleEventBubbleUp(EventBase evt) {

        // Handle clicks on the arrows
        if(evt.eventTypeId == ClickEvent.TypeId()) {
            var target = evt.target as VisualElement;
            if(target == arrows[0]) {
                HandleValueChange(-1);
            } else if(target == arrows[1]) {
                HandleValueChange(1);
            }
        }

        // Handle navigation events
        else if(evt.eventTypeId == NavigationMoveEvent.TypeId()) {
            var navEvent = evt as NavigationMoveEvent;
            if(navEvent.direction == NavigationMoveEvent.Direction.Left) {
                HandleValueChange(-1);
                evt.StopPropagation();
            } else if(navEvent.direction == NavigationMoveEvent.Direction.Right) {
                HandleValueChange(1);
                evt.StopPropagation();
            }
        }

        // Unexpected event
        else {
            Debug.LogWarning($"Received event: {evt} although no interest in it was declared.");
        }
    }
    private void HandleValueChange(int delta) {
        value = Mathf.Clamp(value + delta, 0, options.Count - 1);
    }

    // Triggered by HandleValueChange (from user UI interaction) or by script.
    public override void SetValueWithoutNotify(int newValue) {
        base.SetValueWithoutNotify(newValue);
        UpdateVisuals();
    }

    /// <summary>Updates the arrows and option label text according to the current value.</summary>
    private void UpdateVisuals() {
        // Disable arrows when at the outermost options
        arrows[0].SetEnabled(value > 0);
        arrows[1].SetEnabled(value < options.Count - 1);

        // Set the option text
        if(value >= 0 && value < options.Count)
            optionLabel.text = options[value];
        else
            optionLabel.text = "";
    }
}
