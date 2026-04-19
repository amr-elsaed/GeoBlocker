🌍 Blocked Countries & IP Validation API

A .NET Core Web API that manages blocked countries and validates IP addresses using third-party geolocation services (such as ipapi.co or IPGeolocation.io).

This project demonstrates:

External API integration
Thread-safe in-memory data handling
Background processing
Clean architecture principles
🚀 Features
🔒 Country Blocking
Add a country to the blocked list
Remove a blocked country
Temporarily block a country (with expiration)
🌐 IP Validation
Detect country from IP address
Automatically detect caller IP if not provided
Check if an IP is blocked based on country
📊 Logs & Monitoring
Track blocked access attempts
Store logs in-memory
Pagination support for logs
⚙️ System Design Highlights
No database (in-memory only)
Thread-safe collections (ConcurrentDictionary, ConcurrentBag)
Background service for expiration handling
Clean separation of concerns
🧱 Tech Stack
.NET 8 (Web API)
HttpClient via IHttpClientFactory
Optional: Newtonsoft.Json
In-memory storage (thread-safe collections)
⚙️ Setup Instructions
1. Clone the repository
git clone https://github.com/amr-elsaed/GeoBlocker.git
cd GeoBlocker
2. Install dependencies
dotnet restore
3. Configure API Key

Edit appsettings.json:

{
  "GeoLocation": {
    "ApiKey": "",
    "BaseUrl": "https://api.ipapi.co/"
  }
}

1. Block a Country

POST /api/countries/block

{
  "countryCode": "US"
}
2. Delete Blocked Country

DELETE /api/countries/block/{countryCode}

Returns 404 if not found
3. Get Blocked Countries

GET /api/countries/blocked?page=1&pageSize=10&search=US

Features:

Pagination
Filtering / search
4. IP Lookup

GET /api/ip/lookup?ipAddress=8.8.8.8

If no IP provided → uses caller IP
Returns:
Country code
Country name
ISP
5. Check If IP is Blocked

GET /api/ip/check-block

Flow:

Detect caller IP
Get country from external API
Check against blocked list
Log the attempt
6. Get Blocked Attempts Logs

GET /api/logs/blocked-attempts?page=1&pageSize=10

Each log contains:

IP Address
Timestamp
Country Code
Blocked Status
User-Agent
7. Temporarily Block a Country

POST /api/countries/temporal-block

{
  "countryCode": "EG",
  "durationMinutes": 120
}

Validation:

Duration: 1 → 1440 minutes
Prevent duplicates (returns 409 Conflict)
⏱ Background Processing

A background service runs every 5 minutes to:

Remove expired temporary blocks automatically
🧠 Architecture Overview
Controllers → Services → In-Memory Store
                    ↓
            External API (GeoLocation)
Key Design Decisions
In-Memory Storage
Fast, simple, no persistence
Uses thread-safe collections
Separation of Concerns
Services handle logic
Controllers handle HTTP
Stores manage data
HttpClientFactory
Avoid socket exhaustion
Proper lifecycle management
⚠️ Limitations
Data is lost when application restarts
No persistence layer (by design)
Rate limits depend on external API
🧪 Testing

Unit tests were not implemented in this version.

However, the architecture supports adding tests easily (services are decoupled and testable).

📌 Future Improvements
Add unit & integration tests
Introduce caching for API responses
Add rate limiting per IP
Replace in-memory store with persistent database
Add authentication & authorization
👨‍💻 Author

Your Name
Junior .NET Backend Developer

💡 Notes

This project is designed to showcase:

Real-world API integration
Clean backend design
Problem-solving without a database
🔥 Small Mentorship Note (important for interviews)

If they ask you:

“Why no database?”

You answer:

Requirement-driven decision
Faster performance (memory vs disk)
Simpler architecture
But not scalable → would replace with DB in production
