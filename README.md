# 🎓 Course Management System

**CourseManagement** is a web-based API for managing courses, students, and enrollments.  
It is built with **C# (.NET 8)**, **Entity Framework Core**, and **Microsoft SQL Server**, and provides a fully documented **Swagger UI** for easy testing.

---

## 🧩 Technologies Used
- **Backend Framework:** ASP.NET Core Web API (.NET 8 / .NET 9 ready)
- **Database:** Microsoft SQL Server
- **ORM:** Entity Framework Core
- **API Documentation:** Swagger (Swashbuckle.AspNetCore)
- **Testing:** xUnit, Moq, EF Core InMemory
- **Dependency Injection:** Built-in .NET DI Container

---

## ⚙️ Features
- Full CRUD operations for **Courses** and **Students**
- **Student Enrollment** with course capacity validation
- **Progress tracking** using the Strategy design pattern
- **Automatic completion** when progress reaches 100%
- Centralized **error handling** with custom exceptions
- **Swagger UI** for API exploration and testing
- **Unit tests** for all main service layers

---

## 🚀 Getting Started

### 1️⃣ Clone the repository
```bash
git clone https://github.com/DimitrovYordan/CourseManagement.git
cd CourseManagement


### 2️⃣ Configure the database
Edit the connection string in appsettings.json:
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=CourseManagementDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
}


### 3️⃣ Apply migrations and create the database
bash
Copy code
dotnet ef database update


### 4️⃣ Run the application
bash
Copy code
dotnet run


### 5️⃣ Open Swagger
Open your browser and navigate to:
👉 https://localhost:5001/swagger

---

🧪 Running Unit Tests
Run all unit tests using:
dotnet test

---

📘 API Endpoints Overview
HTTP Method	Endpoint	Description
GET	/api/Courses	Get all courses
POST	/api/Courses	Create a new course
GET	/api/Students	Get all students
POST	/api/Students	Register a new student
POST	/api/Enrollments	Enroll a student in a course
PUT	/api/Enrollments/{id}	Update enrollment progress
GET	/api/Enrollments/student/{id}	Get all enrollments for a student
