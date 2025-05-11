# Settlement Booking System

## Overview

This project is an ASP.NET Core Web API for managing settlement bookings. It exposes endpoints to create bookings and is designed with clean architecture principles.

## How to Run

1. **Restore dependencies:**
   ```sh
   dotnet restore
   ```
2. **Build the solution:**
   ```sh
   dotnet build
   ```
3. **Run the API:**

   ```sh
   dotnet run --project SettlementBookingSystem/SettlementBookingSystem.csproj
   ```

   By default, the API will be available at:

   - https://localhost:5001
   - http://localhost:5000

4. **Swagger UI:**
   Once running, navigate to [https://localhost:5001/swagger](https://localhost:5001/swagger) or [http://localhost:5000/swagger](http://localhost:5000/swagger) to view and interact with the API documentation.

## Main Endpoint

- **Create Booking**

  - **URL:** `POST /Booking`
    - **Request Body:**
      ```json
      {
        "name": "string",
        "bookingTime": "string (ISO 8601 date/time)"
      }
      ```
    - **Response:**
      - `200 OK` with a JSON object containing the `bookingId`.
      - `400 Bad Request` for validation errors.
      - `409 Conflict` if a booking conflict occurs.
      - `429 Too many requests` when too many concurrent requests are being processed (limited to 4 at a time).
      - `500 Internal Server Error` for unhandled exceptions.

  ## Example Requests

  You can use the following HTTP requests to test the API (see [books.http](/books.http) file for examples):

  ```http
  ### Create a new booking
  POST http://localhost:5000/Booking
  Content-Type: application/json

  {
    "name": "John Doe",
    "bookingTime": "10:00"
  }
  ```

## Additional Details

- **Rate Limiting:** The API limits concurrent booking requests to 4 at a time.
- **Error Handling:** Standardized error responses using Problem Details (RFC 7807).
- **Testing:**
  - Unit tests are located in the `SettlementBookingSystem.Application.UnitTests`, `SettlementBookingSystem.Tests` and `SettlementBookingSystem.Infrastructure.Tests` projects.
  - Code Coverage: 75%+
  - Run tests with:
    ```sh
    dotnet test
    ```

## Requirements

- [.NET 5 SDK](https://dotnet.microsoft.com/download/dotnet/5.0) or later

---

For any questions or issues, please refer to the code or contact the author.
