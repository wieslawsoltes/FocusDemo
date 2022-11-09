using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;

namespace FocusDemo;

public static class FocusManager
{
    public static readonly AttachedProperty<IInputElement> FocusedElementProperty = 
        AvaloniaProperty.RegisterAttached<IAvaloniaObject, IInputElement>("FocusedElement", typeof(FocusManager));

    public static TextBox GetFocusedElement(IAvaloniaObject obj)
    {
        return (TextBox)obj.GetValue(FocusedElementProperty);
    }

    public static void SetFocusedElement(IAvaloniaObject obj, TextBox value)
    {
        obj.SetValue(FocusedElementProperty, value);
    }

    static FocusManager()
    {
        FocusedElementProperty.Changed.Subscribe(FocusedElementChanged);
    }

    private static void FocusedElementChanged(AvaloniaPropertyChangedEventArgs<IInputElement> obj)
    {
        var newValue = obj.NewValue.GetValueOrDefault();
        if (newValue is { })
        {
            newValue.AttachedToVisualTree += NewValueOnAttachedToVisualTree;
            newValue.Focus();
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
