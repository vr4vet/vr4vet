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
// Give and object the TutorialEntry.cs component
TutorialEntry:
	SetCompleted()

// the SetCompleted() method of the TutorialEntry.cs component should always be used over the MoveNext()