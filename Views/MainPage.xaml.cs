using calculator.ViewModels;
using Microsoft.UI.Xaml.Input;
using Windows.System;

namespace calculator.Views;

/// <summary>
/// Main calculator page with keyboard handling and ViewModel binding.
/// </summary>
public sealed partial class MainPage : Page
{
    public CalculatorViewModel ViewModel { get; }

    public MainPage()
    {
        this.InitializeComponent();
        ViewModel = new CalculatorViewModel();
    }

    #region Keyboard Accelerator Handlers

    private void OnDigitAccelerator(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
    {
        var digit = sender.Key switch
        {
            VirtualKey.Number0 or VirtualKey.NumberPad0 => "0",
            VirtualKey.Number1 or VirtualKey.NumberPad1 => "1",
            VirtualKey.Number2 or VirtualKey.NumberPad2 => "2",
            VirtualKey.Number3 or VirtualKey.NumberPad3 => "3",
            VirtualKey.Number4 or VirtualKey.NumberPad4 => "4",
            VirtualKey.Number5 or VirtualKey.NumberPad5 => "5",
            VirtualKey.Number6 or VirtualKey.NumberPad6 => "6",
            VirtualKey.Number7 or VirtualKey.NumberPad7 => "7",
            VirtualKey.Number8 or VirtualKey.NumberPad8 => "8",
            VirtualKey.Number9 or VirtualKey.NumberPad9 => "9",
            _ => null
        };

        if (digit != null && ViewModel.DigitInputCommand.CanExecute(digit))
        {
            ViewModel.DigitInputCommand.Execute(digit);
        }

        args.Handled = true;
    }

    private void OnDecimalAccelerator(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
    {
        if (ViewModel.DigitInputCommand.CanExecute("."))
        {
            ViewModel.DigitInputCommand.Execute(".");
        }
        args.Handled = true;
    }

    private void OnOperatorAccelerator(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
    {
        var op = sender.Key switch
        {
            VirtualKey.Add => "+",
            VirtualKey.Subtract => "−",
            VirtualKey.Multiply => "×",
            VirtualKey.Divide => "÷",
            _ => null
        };

        if (op != null && ViewModel.OperationCommand.CanExecute(op))
        {
            ViewModel.OperationCommand.Execute(op);
        }

        args.Handled = true;
    }

    private void OnEqualsAccelerator(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
    {
        if (ViewModel.EqualsCommand.CanExecute(null))
        {
            ViewModel.EqualsCommand.Execute(null);
        }
        args.Handled = true;
    }

    private void OnBackspaceAccelerator(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
    {
        if (ViewModel.BackspaceCommand.CanExecute(null))
        {
            ViewModel.BackspaceCommand.Execute(null);
        }
        args.Handled = true;
    }

    private void OnClearAccelerator(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
    {
        if (ViewModel.ClearCommand.CanExecute(null))
        {
            ViewModel.ClearCommand.Execute(null);
        }
        args.Handled = true;
    }

    private void OnClearEntryAccelerator(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
    {
        if (ViewModel.ClearEntryCommand.CanExecute(null))
        {
            ViewModel.ClearEntryCommand.Execute(null);
        }
        args.Handled = true;
    }

    #endregion
}
