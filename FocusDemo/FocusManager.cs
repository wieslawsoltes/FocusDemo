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
        if (obj.NewValue.GetValueOrDefault() is { } newValue)
        {
            if (newValue.IsAttachedToVisualTree)
            {
                newValue.Focus();
            }
            else
            {
                
                newValue.AttachedToVisualTree += NewValueOnAttachedToVisualTree;
            }
        }
    }

    private static void NewValueOnAttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
    {
        if (sender is IInputElement inputElement)
        {
            inputElement.Focus();
            inputElement.AttachedToVisualTree -= NewValueOnAttachedToVisualTree;
        }
    }
}
