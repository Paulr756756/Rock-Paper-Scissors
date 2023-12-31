# Description
Blazor server application that allows users to play Rock-Paper-Scissors. This web application allows users to play the classic game of Rock, Paper, Scissors against a computer opponent. Includes a computer opponent, a controller to handle external requests and a demo webpage for testing the application.

# Getting Started
1. Clone the repository

```bash
git clone https://github.com/yourusername/rock-paper-scissors.git
cd rock-paper-scissors
```

2. Run the Blazor Server Application

```bash
dotnet run
```

# Dependencies

This project relies on the following dependencies:

- .NET 7 : Ensure that you have .NET 7 installed on your system. You can download it from [https://dotnet.microsoft.com/en-us/download]

# API Endpoints

This application has several API endpoints to handle game interactions

## Start a new game

- Endpoint: `/api/game/start`
- Method: **POST**
- Description: Creates a new game session. Returns a session ID; use this Id to identify your game's session.
- Example Request:
```bash
curl -X POST http://localhost:5140/api/game/post/session
```
- Response Body:
```json
{
    "sessionId":"550e8400-e29b-41d4-a716-446655440000"
}
``` 

## Send a User action

- Endpoint: `/api/post/action`
- Method: **POST**
- Description: Sends user's action. Returns the match results.
- Request Body:
```json
{
    "sessionId":"550e8400-e29b-41d4-a716-446655440000",
    "action": 2 //1->Rock, 2->Paper, 3-> Scissor
}
```
- Example Request:
```bash
curl -X POST http://localhost:5140/api/game/post/action
```
- Response Body: Match results
```json
{
    "userAction": "Paper",
    "computerAction": "Paper",
    "userWon": false,
    "isDraw": true
}
```

## Get session statistics

- Endpoint: `/api/get/stats`
- Query parameters:
    - sessionId : Guid
- Method: **GET**
- Description: Gets the current session statistics.
- Example Request:
```bash
curl -X POST http://localhost:5140/api/game/get/stats?sessionId=550e8400-e29b-41d4-a716-446655440000
```
- Response Body:
```json
{
    "wins": 2,
    "losses": 7,
    "draws": 8
}
```

## Get all matches

- Endpoint: `/api/get/matches`
- Query Parameters:
    - sessionId: GUID
- Method: **GET**
- Description: Returns all the matches of a single session.
- Example Request:
```bash
curl -X POST http://localhost:5140/api/game/get/matches?sessionId=550e8400-e29b-41d4-a716-446655440000
```
- Response Body:
```json
{
    {
        "userAction": "Rock",
        "computerAction": "Scissor",
        "userWon": true,
        "isDraw": false
    },
    {
        "userAction": "Paper",
        "computerAction": "Scissor",
        "userWon": false,
        "isDraw": false
    }
}
```

## Terminate the session
- Endpoint: `/api/delete/terminate`
- Method: **DELETE**
- Description: Deletes the session , removing all your matches from memory.
- Request Body:
```json
{
    "sessionId": "550e8400-e29b-41d4-a716-446655440000"
}
```
- Example Request:
```bash
curl -X POST http://localhost:5140/api/game/delete/terminate
```

# Notes
- The API follows **RESTful** conventions for simplicity and ease of use.
- The game logic is implemented server side.
- **Frequency analysis** of User's previous actions is also done in order to introduce bias and increase difficulty.
- The server uses an **in-memory store** to save user sessions.Sessions that remain inactive for more than a day are deleted automatically.
