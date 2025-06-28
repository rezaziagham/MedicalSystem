
# Medical Appointment System

A simple medical appointment booking system built with **.NET 8**, **Blazor WebAssembly**, and following **Clean Architecture** principles. The app supports managing doctors, patients, schedules, and appointments with a modular, maintainable design.

---

## Features

- Role-based users: Admin, Doctor, Patient  
- Book and manage appointments and schedules  
- Backend APIs built with Minimal API and Entity Framework Core  
- Frontend built with Blazor WebAssembly  
- Clear separation of concerns using Clean Architecture and CQRS patterns  
- Uses SQL Server with code-first migrations  

---

## Tech Stack

- **Backend:** .NET 8, Minimal APIs, EF Core, SQL Server  
- **Frontend:** Blazor WebAssembly  
- **Architecture:** Clean Architecture, CQRS  
- **Tools:** Git, Visual Studio  

---

## Getting Started

1. Clone the repo:
   ```bash
   git clone https://github.com/rezaziagham/MedicalSystem.git
   cd MedicalSystem
   ```

2. Setup database:
   - Make sure SQL Server is running locally  
   - Update connection string if needed (`Infrastructure` project)  
   - Run migrations:
   ```bash
   dotnet ef database update --project Infrastructure
   ```

3. Run backend API:
   ```bash
   dotnet run --project API
   ```

4. Run frontend client:
   ```bash
   dotnet run --project Client
   ```

5. Open the app in your browser at `https://localhost:5001` (or configured port)

---

## How to Use

- Admins manage users and view all appointments  
- Doctors manage their schedules and appointments  
- Patients book and track their appointments  

---

## Next Steps

- Add authentication and authorization  
- Write tests  
- Improve UI/UX  
- Prepare for deployment  

---

## Contact

Reza Ziagham Ahwazi  
[LinkedIn](https://www.linkedin.com/in/reza-ziagham-ahvazi-407a25214)  
Email: reza.ziagham@gmail.com

---

Feel free to explore and contribute!
