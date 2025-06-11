# Todo-Notifier

# Todo-Notifier

Todo-Notifier is a C# application that automatically populates your todo list database with specific, recurring todos for the current day.

## Features

- Automatically adds daily recurring tasks to your todo list database.
- Simple, lightweight, and easy to run.

## Requirements

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

## Installation & Usage

1. **Build the Project**  
   If you have the source code, build the project using the .NET CLI:
   ```sh
   dotnet build Todo-Notifier.csproj
   ```
2. **Run the Application**
   After building, you can run the application using:
   ```sh
   dotnet run --project Todo-Notifier.csproj
   ```
3. **Add to Startup**
   To run the application automatically at startup, you can add it to your system's startup programs. The method for doing this varies by operating system:
   - **Windows**: Place a shortcut to the executable in the `Startup` folder.
   - **Linux**: Use a cron job or systemd service.
   - **macOS**: Use `launchd` or add it to login items.

## Configuration

You can configure the application by modifying the `appsettings.json` file. This file allows you to set various parameters such as the database connection string and the list of recurring tasks.

## Contributing

Contributions are welcome! If you have suggestions for improvements or new features, feel free to open an issue or submit a pull request.

## License

This project is licensed under the Apache License 2.0. See the [LICENSE](LICENSE) file for details.
