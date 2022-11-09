using System;
using Avalonia;
using Avalonia.Input;

namespace FocusDemo;

public static class FocusManager
{
    public static readonly AttachedProperty<IInputElement> FocusedElementProperty = 
        AvaloniaProperty.RegisterAttached<AvaloniaObject, IInputElement>("FocusedElement", typeof(FocusManager));

    public static IInputElement GetFocusedElement(AvaloniaObject obj)
    {
        return obj.GetValue(FocusedElementProperty);
    }

    public static void SetFocusedElement(AvaloniaObject obj, IInputElement value)
    {
        obj.SetValue(FocusedElementProperty, value);
    }

    static FocusManager()
    {
        FocusedElementProperty.Changed.Subscribe(FocusedElementChanged);
    }

    private static void FocusedElementChanged(AvaloniaPropertyChangedEventArgs<IInputElement> obj)
    {
        var oldValue = obj.OldValue.GetValueOrDefault();
        var newValue = obj.NewValue.GetValueOrDefault();

        if (oldValue is { })
        {
            oldValue.AttachedToVisualTree -= FocusedElement_OnAttachedToVisualTree;
        }

        if (newValue is { })
        {
            newValue.AttachedToVisualTree += FocusedElement_OnAttachedToVisualTree;
            newValue.Focus();
        }
    }

    private static void FocusedElement_OnAttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
    {
        if (sender is IInputElement inputElement)
        {
            inputElement.Focus();
        }
    }
}
