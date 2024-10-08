# Discord .Net Core Bot

This repository contains a Discord bot built with .Net Core. The bot is containerized using Docker and requires specific environment variables to be set in a `.env` file.

## Prerequisites

- Docker
- Docker Compose
- .Net Core SDK

### Environment Variables

The bot requires the following environment variables to be set in a `.env` file:

```env
MONGODB_URI - MongoURI
MONGODB_DB - MongoDBName
BOT_TOKEN - Discord Bot Token
DISCORD_SERVER - ServerID of Discord
```

### Appsettings.json Configuration

The `appsettings.json` file should include the following entries:

```json
{
    "MongoSettings": {
        "ConnectionString": "${MONGODB_URI}",
        "Database": "${MONGODB_DB}"
    }
}
```

### Docker Setup

To build and run the bot using Docker, follow these steps:

1. Build the Docker image:
        ```sh
        docker-compose build
        ```

2. Run the Docker container:
        ```sh
        docker-compose up
        ```

### Running Locally

To run the bot locally, ensure you have the .Net Core SDK installed and follow these steps:

1. Restore the dependencies:
        ```sh
        dotnet restore
        ```

2. Run the bot:
        ```sh
        dotnet run
        ```

### Contributing

Feel free to open issues or submit pull requests if you have any improvements or bug fixes.
