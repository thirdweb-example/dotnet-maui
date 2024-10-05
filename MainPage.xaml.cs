using Thirdweb;

namespace dotnet_maui;

public partial class MainPage : ContentPage
{
    private ThirdwebClient? client;
    private InAppWallet? wallet;

    public MainPage()
    {
        InitializeComponent();
        InitializeThirdweb();
    }

    private void InitializeThirdweb()
    {
        client = ThirdwebClient.Create(
            clientId: "7e7c070be896e27ffd5352e264df9bcc",
            bundleId: "com.thirdweb.maui"
        );
    }

    private void LoginGoogleButton_Clicked(object sender, EventArgs e)
    {
        LoginAsync(AuthProvider.Google);
    }

    private void LoginDiscordButton_Clicked(object sender, EventArgs e)
    {
        LoginAsync(AuthProvider.Discord);
    }

    private void LoginTelegramButton_Clicked(object sender, EventArgs e)
    {
        LoginAsync(AuthProvider.Telegram);
    }

    private async void LoginAsync(AuthProvider authProvider)
    {
        if (client == null)
            return;

        var walletLabel = this.FindByName<Label>("WalletLabel");
        var instructionLabel = this.FindByName<Label>("InstructionLabel");
        var signMessageButton = this.FindByName<Button>("SignMessageButton");
        var disconnectButton = this.FindByName<Button>("DisconnectButton");
        var loadingIndicator = this.FindByName<ActivityIndicator>("LoadingIndicator");
        var signedMessageLabel = this.FindByName<Label>("SignedMessageLabel");
        var loginButtons = new[]
        {
            this.FindByName<Button>("LoginGoogleButton"),
            this.FindByName<Button>("LoginDiscordButton"),
            this.FindByName<Button>("LoginTelegramButton")
        };

        // Hide elements and show loading
        walletLabel.IsVisible = false;
        instructionLabel.IsVisible = false;
        signMessageButton.IsVisible = false;
        disconnectButton.IsVisible = false;
        loadingIndicator.IsVisible = true;
        signedMessageLabel.IsVisible = false;

        foreach (var button in loginButtons)
        {
            button.IsVisible = false;
        }

        try
        {
            wallet = await InAppWallet.Create(client, authProvider: authProvider);

            // Login only if not connected
            if (!await wallet.IsConnected())
            {
                _ = await wallet.LoginWithOauth(
                    isMobile: DeviceInfo.Platform == DevicePlatform.iOS
                        || DeviceInfo.Platform == DevicePlatform.Android,
                    browserOpenAction: OpenBrowser,
                    mobileRedirectScheme: "com.thirdweb.maui"
                );
            }

            var walletAddress = await wallet.GetAddress();
            walletLabel.Text = $"Wallet: {walletAddress}";
            walletLabel.TextColor = Colors.White;
            walletLabel.IsVisible = true;
            instructionLabel.IsVisible = true;
            signMessageButton.IsVisible = true;
            disconnectButton.IsVisible = true;

            walletLabel.GestureRecognizers.Clear();
            walletLabel.GestureRecognizers.Add(
                new TapGestureRecognizer
                {
                    Command = new Command(
                        () => OpenBrowser($"https://sepolia.arbiscan.io/address/{walletAddress}")
                    )
                }
            );
        }
        catch (Exception ex)
        {
            walletLabel.Text = $"Error: {ex.Message}";
            walletLabel.IsVisible = true;
        }
        finally
        {
            loadingIndicator.IsVisible = false;
        }
    }

    private void SignMessageButton_Clicked(object sender, EventArgs e)
    {
        SignMessageAsync();
    }

    private async void SignMessageAsync()
    {
        if (wallet == null)
            return;

        var signedMessageLabel = this.FindByName<Label>("SignedMessageLabel");

        try
        {
            var message = "Hello, Thirdweb!";
            var signature = await wallet.PersonalSign(message);
            signedMessageLabel.Text = $"Signed message: {signature}";
            signedMessageLabel.IsVisible = true;
        }
        catch (Exception ex)
        {
            signedMessageLabel.Text = $"Error during signing: {ex.Message}";
            signedMessageLabel.IsVisible = true;
        }
    }

    private void DisconnectButton_Clicked(object sender, EventArgs e)
    {
        DisconnectWallet();
    }

    private async void DisconnectWallet()
    {
        if (wallet == null || !await wallet.IsConnected())
            return;

        await wallet.Disconnect();

        var walletLabel = this.FindByName<Label>("WalletLabel");
        var instructionLabel = this.FindByName<Label>("InstructionLabel");
        var signMessageButton = this.FindByName<Button>("SignMessageButton");
        var disconnectButton = this.FindByName<Button>("DisconnectButton");
        var signedMessageLabel = this.FindByName<Label>("SignedMessageLabel");
        var loginButtons = new[]
        {
            this.FindByName<Button>("LoginGoogleButton"),
            this.FindByName<Button>("LoginDiscordButton"),
            this.FindByName<Button>("LoginTelegramButton")
        };

        walletLabel.Text = "Disconnected.";
        signMessageButton.IsVisible = false;
        instructionLabel.IsVisible = false;
        disconnectButton.IsVisible = false;
        signedMessageLabel.IsVisible = false;

        foreach (var button in loginButtons)
        {
            button.IsVisible = true;
        }
    }

    private void OpenBrowser(string url)
    {
        try
        {
            Browser.Default.OpenAsync(new Uri(url), BrowserLaunchMode.SystemPreferred);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to open browser: {ex.Message}");
        }
    }
}
