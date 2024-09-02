using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Demo control.
/// </summary>
[UxmlElement]
public partial class CounterButton : Button {

    private int count;

    [EventInterest(typeof(ClickEvent))]
    protected override void HandleEventBubbleUp(EventBase evt) {
        count++;
        text = $"Counter: {count}";

        if(evt is not ClickEvent)
            Debug.LogWarning($"Received {evt} although no interest in it was declared.");
    }
}
