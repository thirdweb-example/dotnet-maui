# Thirdweb .NET MAUI App

This is a .NET MAUI application that integrates with Thirdweb’s .NET SDK to interact with blockchain wallets and smart contracts. This project demonstrates in-app wallet login and fetching wallet addresses using Google, Discord, and Telegram.

## Requirements

- **.NET SDK 8.0** or higher ([Install .NET SDK](https://dotnet.microsoft.com/download))
- **Visual Studio 2022** with the following workloads:
  - .NET Multi-platform App UI development (MAUI)
  - Windows App SDK (for Windows-specific features)
- **Windows SDK** version 10.0.19041.0 or later ([Download Windows SDK](https://developer.microsoft.com/en-us/windows/downloads/windows-10-sdk))
- **Thirdweb .NET SDK** version 2.4.0 or later

## Installation

1. **Clone the repository**:

   ```bash
   git clone https://github.com/your-username/your-repo-name.git
   cd your-repo-name
   ```

2. **Install .NET dependencies**:

   Run the following command to restore the required .NET dependencies:

   ```bash
   dotnet restore
   ```

3. **Set up Thirdweb Client**:

   Ensure that you have your Thirdweb Client ID in place. Open `MainPage.xaml.cs` and set your Client ID for the Thirdweb client.

   ```csharp
   client = ThirdwebClient.Create(
      clientId: "your-client-id", // Replace with your actual Thirdweb client ID
      bundleId: "com.thirdweb.maui"
   );
   ```

## Running the Project

### Visual Studio

1. **Open the solution** in Visual Studio 2022.
2. **Select the target platform** (Windows, Android, iOS, etc.) depending on your development environment.
3. **Run the project** by pressing `F5` or using the `Run` button in Visual Studio.

### Command Line

To run the project from the command line for Windows:

```bash
dotnet build -f net8.0-windows10.0.19041.0 -c Debug -p:PublishReadyToRun=true -p:WindowsPackageType=None
dotnet run -f net8.0-windows10.0.19041.0 -c Debug -p:PublishReadyToRun=true -p:WindowsPackageType=None
```

For Android, replace `net8.0-windows` with `net8.0-android`.

## Features

- **Login with Google, Discord, and Telegram** via in-app wallets using Thirdweb SDK.
- **Fetch and display wallet address** with link to Arbiscan for verification.

## Troubleshooting

1. Ensure you have the correct versions of .NET, Visual Studio, and the Windows SDK installed.
2. If the app fails to start, check for missing dependencies or SDKs. You may need to install the **Windows App SDK** or update your `.csproj` file to reference the correct version.
3. If you encounter `TypeInitializationException`, ensure the **WindowsAppSDK** is installed and updated.

## License

This project is licensed under the MIT License.
