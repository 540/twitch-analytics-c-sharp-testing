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

### Development

1. Build the project:
```bash
dotnet build
```

2. Run the project:
```bash
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
3. Manager retrieves streamer data (currently from mock data)
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

**Error Responses:**
- 400 Bad Request: Invalid or empty streamer ID
- 404 Not Found: Streamer not found
- 500 Internal Server Error: Server-side error

#### Implementation Details
- `StreamerController`: Handles HTTP requests and responses
- `GetStreamerService`: Validates input and orchestrates the operation
- `StreamerManager`: Contains business logic for retrieving streamer data
- `FakeTwitchApiClient`: Provides mock data (will be replaced with real Twitch API)

### 2. Get Enriched Top Streams

This use case retrieves and enriches the top live streams from Twitch, combining stream data with broadcaster information.

#### Flow
1. Get top streams ordered by viewer count
2. Get user details for the streamers
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
    "stream_id": "987654321",
    "user_id": "111111111",
    "title": "Epic Gaming Session",
    "viewer_count": 34567,
    "game_name": "Fortnite",
    "started_at": "2024-01-10T08:00:00Z",
    "user_display_name": "TopStreamer1",
    "profile_image_url": "https://static-cdn.jtvnw.net/jtv_user_pictures/topstreamer1-profile_image.png",
    "broadcaster_type": "partner"
  }
]
```

**Error Responses:**
- 400 Bad Request: Invalid limit parameter
- 500 Internal Server Error: Server-side error

#### Implementation Details
- Currently using mock data from two sources:
  - `twitch-streams-mock.json`: Simulates Twitch's `/streams` endpoint
  - `twitch-users-mock.json`: Simulates Twitch's `/users` endpoint
- Streams are ordered by viewer count
- Each stream is enriched with broadcaster information

## ��️ Technical Stack & Tools

### Core Technologies
- **.NET 6**: Modern, cross-platform framework
- **ASP.NET Core**: Web API framework
- **Swagger/OpenAPI**: API documentation

### Code Quality Tools

#### StyleCop
StyleCop ensures consistent code style across the project. Key configurations in `StyleCop.ruleset`:
- No XML documentation required
- Underscore prefix allowed for private fields
- System using directives first
- Flexible comment rules

#### EditorConfig
Maintains consistent coding styles. Key settings:
- 4 spaces indentation
- LF line endings
- Organized using directives
- Consistent C# code style rules

#### Husky.Net
Git hooks for code quality:
- Pre-commit checks:
  - Code formatting (`dotnet format`)
  - StyleCop analysis
  - Build verification

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

3. **Domain Layer** (Managers & Models)
   - Core business logic
   - Domain models
   - Business rules

4. **Infrastructure Layer**
   - External service communication
   - Currently using mock data for development

### Domain-Driven Design
- Bounded Contexts for domain separation
- Rich domain models
- Value Objects (coming soon)
- Domain Events (coming soon)

## 🧪 Development Guidelines

### Code Organization
- Follow Clean Architecture principles
- Keep classes focused (Single Responsibility)
- Use dependency injection
- Implement proper error handling
- Write meaningful logs

### Error Handling
Standard HTTP status codes:
- 200: Success
- 400: Bad Request
- 404: Not Found
- 500: Internal Server Error
