# Bocchi Web Backend

A .NET Core Web API for an e-commerce platform with features like product management, cart operations, and order processing.

## Features

- User Authentication & Authorization
- Product Management
- Category Management
- Shopping Cart
- Order Processing
- File Upload (Images)
- JWT Authentication

## Technologies

- .NET 8
- Entity Framework Core
- MySQL
- JWT Authentication
- Swagger/OpenAPI

## Prerequisites

- .NET 8 SDK
- MySQL
- Visual Studio 2022 or VS Code

1. Clone the repository
```bash
git clone https://github.com/yourusername/bocchiwebbackend.git
```

2. Navigate to the project directory
```bash
cd bocchiwebbackend
```

3. Update the connection string in `appsettings.json`
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Your_Connection_String_Here"
  }
}
```

4. Run the migrations
```bash
dotnet ef database update
```

5. Run the application
```bash
dotnet run
```
## API Endpoints
For more information about endpoint go to
```bash
http://localhost:your_port/swagger/index.html
```
### Authentication
- POST `/api/auth/register` - Register new user
- POST `/api/auth/login` - User login

### Users
- GET `/api/users/{id}` - Get user info by Id 
- PUT `/api/users/{id}` - Update user by Id
- DELETE `/api/users/{id}` - Delete user by Id
- GET `/api/users/profile` - Get current user profile
- GET `/api/users` - Get all users

### Products
- GET `/api/products` - Get all products
- GET `/api/products/{id}` - Get product by ID
- POST `/api/products` - Create new product (Admin only)
- PUT `/api/products/{id}` - Update product (Admin only)
- DELETE `/api/products/{id}` - Delete product (Admin only)

### Categories
- GET `/api/categories` - Get all categories
- GET `/api/categories/{id}` - Get category by ID
- POST `/api/categories` - Create new category (Admin only)
- PUT `/api/categories/{id}` - Update category (Admin only)
- DELETE `/api/categories/{id}` - Delete category (Admin only)

### Cart
- GET `/api/cart` - Get user's cart
- POST `/api/cart/items` - Add item to cart
- PUT `/api/cart/items/{productId}` - Update cart item quantity
- DELETE `/api/cart/items/{productId}` - Remove item from cart
- DELETE `/api/cart` - Clear cart

### Orders
- GET `/api/orders` - Get user's orders
- GET `/api/orders/{id}` - Get order by ID
- POST `/api/orders` - Create new order
- PUT `/api/orders/{id}/status` - Update order status (Admin only)
- DELETE `/api/orders/{id}` - Cancel order

## Project Structure
```text
bocchiwebbackend/
├── Controllers/      # API Controllers
├── DTOs/             # Data Transfer Objects
├── Models/           # Domain Models
├── Repositories/     # Data Access Layer
├── Services/         # Business Logic Layer
├── Data/             # Database Context
├── Migrations/       # Database Migrations
├── wwwroot/          # Static Files
└── Program.cs        # Application Entry Point
```


## Authentication

The API uses JWT (JSON Web Tokens) for authentication. Include the token in the Authorization header. Authorization: Bearer `your_jwt_token`

## File Upload

- Supported image formats: JPEG, PNG, GIF
- Images are stored in `wwwroot/uploads/products/`
- Maximum file size: 10MB

## Environment Variables

Write in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Your_Development_Connection_String"
  },
  "Jwt": {
    "Key": "Your_Secret_Key",
    "Issuer": "Your_Issuer",
    "Audience": "Your_Audience",
    "ExpiryMinutes": 120
  }
}
```
## Contact

nhd3009 - conan675@live.com

Project Link: [https://github.com/nhd3009/bocchiwebbackend](https://github.com/nhd3009/bocchiwebbackend)
