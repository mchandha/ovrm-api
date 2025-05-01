# Online Vehicle Rental Management System (OVRM) 🚗

A full-featured ASP.NET Core Web API project to manage vehicle rentals with:

- 🔐 JWT Authentication
- 📄 Swagger API Documentation
- 🛒 Booking & Payments Management
- 📦 Entity Framework Core + Code-First
- 🛠️ Global Exception Handling
- ✅ FluentValidation
- 📊 Serilog Logging (SQL Server Sink)

---

## 🔧 Technologies Used

- ASP.NET Core 7 Web API
- Entity Framework Core
- SQL Server
- JWT Authentication
- Serilog for Logging
- Swagger (Swashbuckle)
- FluentValidation

---

## 📁 Project Structure

OVRM/ ├── Controllers/ │ ├── VehiclesController.cs │ ├── BookingsController.cs │ └── PaymentsController.cs ├── DTO/ ├── Models/ ├── Services/ ├── Middleware/ │ └── GlobalExceptionMiddleware.cs ├── Logging/ │ └── Serilog config ├── Program.cs └── appsettings.json


---

## 🚀 Getting Started

### Prerequisites

- .NET 7 SDK or higher
- SQL Server (or local instance)
- Visual Studio 2022 or Visual Studio Code
- Chrome/Any Browser
- Postman for API testing (optional)

### Setup

1. **Clone the Repository**
2. **Configure the Database**

    - Create a database in SQL Server named `OVRM`.
    - Update the connection string in `appsettings.json` to match your SQL Server instance.
    

3. **Run Migrations**

    After configuring your database, apply migrations to create the required tables:


4. **Install Dependencies**

    If you're using Visual Studio or Visual Studio Code, the necessary packages should already be included. However, ensure all dependencies are installed:


5. **Build the Project**


6. **Run the Application**

    The application should now be running at `http://localhost:5000` by default.

7. **Test with Postman/Swagger**

    - Open `http://localhost:5000/swagger` to access the Swagger API documentation.
    - You can test all endpoints from the Swagger UI or use Postman to make API requests.

---

## 📜 API Endpoints

### Authentication

- **POST** `/api/auth/login`  
  - Request body:  
    ```json
    {
      "email": "user@example.com",
      "password": "password123"
    }
    ```
  - Returns a JWT token for authenticated requests.

### Vehicles

- **GET** `/api/vehicles`  
  - Returns a list of all vehicles.

- **GET** `/api/vehicles/{id}`  
  - Returns details of a specific vehicle.

- **POST** `/api/vehicles`  
  - Adds a new vehicle.  
  - Request body:  
    ```json
    {
      "name": "Sedan",
      "type": "Car",
      "pricePerDay": 50
    }
    ```

### Bookings

- **GET** `/api/bookings`  
  - Returns a list of all bookings.

- **POST** `/api/bookings`  
  - Creates a new booking.  
  - Request body:  
    ```json
    {
      "vehicleId": 1,
      "customerId": 2,
      "startDate": "2023-06-01T00:00:00",
      "endDate": "2023-06-07T00:00:00",
      "isDelivery": true
    }
    ```

### Payments

- **POST** `/api/payments`  
  - Creates a payment for a booking.  
  - Request body:  
    ```json
    {
      "bookingId": 1,
      "paymentMethod": "Credit Card",
      "transactionId": "TX123456",
      "status": "Completed",
      "currency": "USD",
      "amount": "350",
      "paymentDate": "2023-06-01T12:00:00"
    }
    ```

---

## 📝 Notes

- Ensure the SQL Server instance is configured correctly for remote connections if you're running on a cloud server.
- You can modify the connection string for a local database or use SQL Server authentication if necessary.
- JWT tokens are required for protected endpoints. Always include the token in the `Authorization` header as `Bearer <token>`.

---

## 🛠️ Troubleshooting

- **"Unable to connect to database" error:**  
  Double-check your connection string and ensure SQL Server is running. Also, verify your SQL Server allows remote connections if using a remote database.

- **"JWT token expired" error:**  
  Ensure your JWT token is refreshed or valid. Check the expiry date in your token claims.

---

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

