# TwitchAnalytics

TwitchAnalytics is a .NET 6 Web API project that provides analytics and insights for Twitch streamers. The project follows Clean Architecture principles and uses modern .NET practices.

## 🚀 Getting Started

### Prerequisites
- .NET 6.0 SDK
- Git
- Visual Studio Code (recommended) or Visual Studio 2022

### Installation

1. Clone the repository
```bash
git clone https://github.com/yourusername/TwitchAnalytics.git
cd TwitchAnalytics
```

2. Create required mock data files:
```bash
mkdir -p src/TwitchAnalytics/Data
# Create twitch-mock-data.json, twitch-streams-mock.json, and twitch-users-mock.json
```

### Development

1. Build the project:
```bash
dotnet build
```

2. Run the project:
```bash
cd src/TwitchAnalytics
dotnet run
```

3. Run code formatting:
```bash
dotnet format
```

4. Run linting (StyleCop analysis):
```bash
dotnet build /p:TreatWarningsAsErrors=true
```

The API will be available at:
- Swagger UI: https://localhost:7059/swagger
- API Base URL: https://localhost:7059/analytics

## 🎯 Current Features

### 1. Get Streamer Information

This use case retrieves detailed information about a Twitch streamer by their ID.

#### Flow
1. Client requests streamer information via GET endpoint
2. Service validates the streamer ID
3. Manager retrieves streamer data from `Data/twitch-mock-data.json`
4. Returns streamer information or appropriate error

#### API Details

**Endpoint:** `GET /analytics/streamer?id={streamerId}`

**Example Request:**
```bash
curl -X GET "https://localhost:7059/analytics/streamer?id=12345"
```

**Success Response (200 OK):**
```json
{
    "id": "12345",
    "login": "ninja",
    "displayName": "Ninja",
    "type": "",
    "broadcasterType": "partner",
    "description": "Professional Gamer and Streamer",
    "profileImageUrl": "https://example.com/ninja.jpg",
    "offlineImageUrl": "https://example.com/ninja-offline.jpg",
    "viewCount": 500000,
    "createdAt": "2011-11-20T00:00:00Z"
}
```
Data source: `Data/twitch-mock-data.json`

**Error Responses:**
- 400 Bad Request: Invalid or empty streamer ID
- 404 Not Found: Streamer not found
- 500 Internal Server Error: Server-side error

### 2. Get Enriched Top Streams

This use case retrieves and enriches the top live streams from Twitch, combining stream data with broadcaster information.

#### Flow
1. Get top streams from `Data/twitch-streams-mock.json`
2. Get user details from `Data/twitch-users-mock.json`
3. Combine both datasets
4. Return enriched stream information

#### API Details

**Endpoint:** `GET /analytics/streams/enriched?limit={limit}`

**Parameters:**
- `limit` (optional): Number of streams to return (default: 3, max: 100)

**Example Request:**
```bash
curl -X GET "https://localhost:7059/analytics/streams/enriched?limit=3"
```

**Success Response (200 OK):**
```json
[
  {
    "stream_id": "12345",           // from twitch-streams-mock.json
    "user_id": "12345",            // from twitch-streams-mock.json
    "title": "Playing Fortnite!",  // from twitch-streams-mock.json
    "viewer_count": 20000,         // from twitch-streams-mock.json
    "game_name": "Fortnite",       // from twitch-streams-mock.json
    "started_at": "2024-03-20T10:00:00Z",  // from twitch-streams-mock.json
    "user_display_name": "Ninja",  // from twitch-users-mock.json
    "profile_image_url": "https://example.com/ninja.jpg",  // from twitch-users-mock.json
    "broadcaster_type": "partner"  // from twitch-users-mock.json
  }
]
```

Data sources:
- Stream data: `Data/twitch-streams-mock.json`
- User data: `Data/twitch-users-mock.json`

**Error Responses:**
- 400 Bad Request: Invalid limit parameter
- 500 Internal Server Error: Server-side error

## 🏛️ Architecture

### Clean Architecture Layers

1. **API Layer** (Controllers)
   - HTTP request/response handling
   - Input validation
   - Route definitions
   - Error handling

2. **Application Layer** (Services)
   - Use case orchestration
   - Business logic coordination
   - Input validation

3. **Domain Layer** (Managers)
   - Business logic
   - Domain models
   - Interface definitions

4. **Infrastructure Layer**
   - External service implementations
   - Data access
   - Mock data providers

## 🛠️ Code Quality Tools

### StyleCop
- Configuration in `StyleCop.ruleset`
- Enforces consistent code style
- XML documentation requirements disabled
- Underscore prefix allowed for private fields

### EditorConfig
- 4 spaces indentation
- LF line endings
- Organized using directives
- Consistent C# code style rules

### Husky.Net
Pre-commit checks:
- Code formatting (`dotnet format`)
- StyleCop analysis
- Build verification
