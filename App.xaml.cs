using calculator.Helpers;
using calculator.Services;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml.Navigation;
using Windows.Graphics;
using WinRT.Interop;

namespace calculator;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : Application
{
    private Window? _window;
    private AppWindow? _appWindow;

    /// <summary>
    /// Initializes the singleton application object.
    /// </summary>
    public App()
    {
        this.InitializeComponent();
        
        // Register services
        ServiceLocator.Register<ICalculatorEngine>(new CalculatorEngine());
    }

    /// <summary>
    /// Invoked when the application is launched normally by the end user.
    /// </summary>
    /// <param name="e">Details about the launch request and process.</param>
    protected override void OnLaunched(LaunchActivatedEventArgs e)
    {
        _window = new Window();

        if (_window.Content is not Frame rootFrame)
        {
            rootFrame = new Frame();
            rootFrame.NavigationFailed += OnNavigationFailed;
            _window.Content = rootFrame;
        }

        // Configure window appearance
        ConfigureWindow();

        _ = rootFrame.Navigate(typeof(MainPage), e.Arguments);
        _window.Activate();
    }

    /// <summary>
    /// Configures the window with Mica backdrop, minimum size, and title.
    /// </summary>
    private void ConfigureWindow()
    {
        if (_window == null) return;

        // Get the AppWindow for advanced configuration
        var hwnd = WindowNative.GetWindowHandle(_window);
        var windowId = Win32Interop.GetWindowIdFromWindow(hwnd);
        _appWindow = AppWindow.GetFromWindowId(windowId);

        if (_appWindow != null)
        {
            // Set window title
            _appWindow.Title = "Calculator";

            // Set minimum window size (400x500)
            _appWindow.Changed += AppWindow_Changed;
            
            // Set initial size
            _appWindow.Resize(new SizeInt32(400, 600));

            // Configure title bar for immersive appearance
            if (AppWindowTitleBar.IsCustomizationSupported())
            {
                var titleBar = _appWindow.TitleBar;
                titleBar.ExtendsContentIntoTitleBar = false;
                
                // Set title bar colors to match theme
                titleBar.ButtonBackgroundColor = Colors.Transparent;
                titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            }
        }

        // Apply Mica backdrop
        TrySetMicaBackdrop();
    }

    /// <summary>
    /// Enforces minimum window size.
    /// </summary>
    private void AppWindow_Changed(AppWindow sender, AppWindowChangedEventArgs args)
    {
        if (args.DidSizeChange && _appWindow != null)
        {
            var size = _appWindow.Size;
            bool needsResize = false;

            int width = size.Width;
            int height = size.Height;

            // Enforce minimum width of 400
            if (width < 400)
            {
                width = 400;
                needsResize = true;
            }

            // Enforce minimum height of 500
            if (height < 500)
            {
                height = 500;
                needsResize = true;
            }

            if (needsResize)
            {
                _appWindow.Resize(new SizeInt32(width, height));
            }
        }
    }

    /// <summary>
    /// Attempts to set the Mica backdrop for the window.
    /// </summary>
    private void TrySetMicaBackdrop()
    {
        if (_window == null) return;

        // Try to use Mica backdrop (Windows 11)
        if (Microsoft.UI.Composition.SystemBackdrops.MicaController.IsSupported())
        {
            var micaBackdrop = new Microsoft.UI.Xaml.Media.MicaBackdrop();
            _window.SystemBackdrop = micaBackdrop;
        }
        // Fallback to Desktop Acrylic (Windows 10)
        else if (Microsoft.UI.Composition.SystemBackdrops.DesktopAcrylicController.IsSupported())
        {
            var acrylicBackdrop = new Microsoft.UI.Xaml.Media.DesktopAcrylicBackdrop();
            _window.SystemBackdrop = acrylicBackdrop;
        }
    }

    /// <summary>
    /// Invoked when Navigation to a certain page fails.
    /// </summary>
    void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
    {
        throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
    }
}
