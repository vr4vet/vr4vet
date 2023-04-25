Basic spec:

// A sequence of tutorial entries which are completed in order.
// The tutorial can be activated by using an area of effect, or some other trigger.
Tutorial:
	Items[]
	Current
	Dismiss()
	MoveNext()
	MoveBack()

// Something which requires user interaction to be marked as completed.
// Once complete, the popup hint will be dismissed, and the tutorial will advance.
TutorialItem:
	Dismiss() // Some action/interaction/area of effect -> dismiss this item -> move next in tutorial
	// A UI element, illustrating/documenting something to the user
	PopupHint:
		Text