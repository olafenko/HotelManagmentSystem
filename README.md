#  Hotel Management System (Mobile App & API)

A cross-platform mobile application and backend API built for hotel staff daily operations.

> **Status:** MVP is functional. The project is under development.

## Screenshots

### Reservations view
<img width="250"  alt="Screenshot_1788890723" src="https://github.com/user-attachments/assets/09e98860-cd7f-426c-902d-1faf74d0bf8c" />

### Add reservation form 

<img width="250" alt="Screenshot_1788890736" src="https://github.com/user-attachments/assets/fa97b524-ed7e-4c6c-879c-b52cdb2de593" /> &nbsp;&nbsp;&nbsp;&nbsp;<img width="250"  alt="Screenshot_1788890755" src="https://github.com/user-attachments/assets/c5635807-657a-44ed-a327-9b4405dbe7c7" /> &nbsp;&nbsp;&nbsp;&nbsp; <img width="250" alt="Screenshot_1788890786" src="https://github.com/user-attachments/assets/cfd7790c-f378-48d1-9854-470db0384ad4" /> &nbsp;&nbsp;&nbsp;&nbsp;

### Rooms view
<img width="250"  alt="Screenshot_1788890853" src="https://github.com/user-attachments/assets/e855d51e-f512-4af3-911c-c1c97a3654c8" />

### Add room form with validation
<img width="250"  alt="Screenshot_1788890976" src="https://github.com/user-attachments/assets/60437566-92c1-4c70-9178-ae5aa69ca964" />

## Tech Stack

**Backend (REST API):**
* C# .NET 
* Entity Framework Core (SQL Server)
* Architecture: CQRS (MediatR)
* Validation: FluentValidation, Pipeline Behaviors

**Frontend (Mobile):**
* React Native
* React Native Paper 
* Custom Hooks for automated form error mapping

**Engineering & Architectural Decisions:**

* CQRS with MediatR: Strict separation of read and write  operations ensures maximum isolation of business logic. Each Handler has a single responsibility, making the code testable and scalable.

* Pipeline Behaviors (Validation): FluentValidation logic is integrated directly into the MediatR pipeline. Invalid requests are intercepted and rejected globally before they ever reach the Handlers, keeping the Application layer completely clean.

* Automated Error Mapping (Custom Hooks): Frontend error handling is abstracted into custom React hooks. When the API returns a 400 Bad Request containing validation errors, the hook automatically maps these backend errors directly to the corresponding UI form inputs.

**Infrastructure:**
* Docker & Docker Compose

## How to run locally

The entire backend infrastructure (API & Database) is fully containerized.

**1. Start the backend environment:**
   ```bash
   docker-compose up --build -d
   ```
**2. Run the mobile app:**
   ```bash
   cd HotelMobile
   npm install
   ```
   **For Android**
   ```bash
   npx react-native run-android
   ```

   **For iOS**
   ```bash
   cd ios && pod install && cd ..
   npx react-native run-ios
   ```

## TO DO Features

* Sorting and filtering
* Implement JWT Authentication & Role-Based Access Control
* Unit and Integration Testing 
* CI/CD Pipeline (GitLab)
* App settings (ex. english language, light theme)
