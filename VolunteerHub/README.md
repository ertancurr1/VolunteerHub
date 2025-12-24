# VolunteerHub

**NGO Community Management System**

A professional desktop application for managing volunteers, projects, and assignments built with .NET MAUI.

---

## 🌟 Features

- **Volunteer Management** - Complete CRUD operations for volunteer profiles
- **Project Management** - Track NGO projects with status and requirements
- **Assignment Management** - Link volunteers to projects with hours tracking
- **Real-time Dashboard** - 8 key statistics displayed at a glance
- **Advanced Search** - Filter volunteers, projects, and assignments
- **Data Export** - Export to CSV and JSON formats
- **Professional UI** - Clean corporate design with consistent styling

---

## 🛠️ Technology Stack

- **Framework:** .NET MAUI (Multi-platform App UI)
- **Language:** C# 12
- **Database:** SQLite
- **ORM:** Entity Framework Core
- **Platform:** Windows Desktop
- **IDE:** Visual Studio 2022

---

## 📊 System Architecture

### Database Entities

1. **Volunteer** - Stores volunteer profile information
2. **Project** - Contains NGO project details
3. **VolunteerAssignment** - Junction table linking volunteers to projects

### Relationships

- One Volunteer → Many Assignments
- One Project → Many Assignments
- Volunteer ←→ Project (many-to-many through VolunteerAssignment)

---

## 🚀 Getting Started

### Prerequisites

- Windows 10/11
- .NET 8.0 SDK
- Visual Studio 2022 (Community Edition or higher)

### Installation

1. Clone the repository:
```bash
   git clone https://github.com/ertancurr1/VolunteerHub.git
```

2. Open `VolunteerHub.sln` in Visual Studio 2022

3. Restore NuGet packages (automatic)

4. Build and run (F5)

### First Launch

The application will automatically:
- Create the SQLite database
- Seed sample data (8 volunteers, 7 projects, 7 assignments)
- Display the dashboard

---

## 📱 Application Pages

1. **Dashboard** - Real-time statistics and quick navigation
2. **Volunteers** - Manage volunteer profiles
3. **Volunteer Details** - Add/edit volunteer information
4. **Projects** - Manage NGO projects
5. **Project Details** - Add/edit project information
6. **Assignments** - Manage volunteer-project assignments
7. **Assignment Details** - Create/edit assignments
8. **Export Reports** - Generate CSV/JSON exports
9. **Help & About** - User guide and information

---

## 💻 Key Features Implementation

### Dashboard Statistics
- Total Volunteers / Active Volunteers
- Total Projects / Active Projects
- Total Assignments / Total Hours
- Completed Projects / Planning Projects

### CRUD Operations
- Full Create, Read, Update, Delete on all entities
- Input validation with friendly error messages
- Confirmation dialogs for destructive actions

### Search & Filter
- Real-time search on all management pages
- Filters by name, email, skills, project name, description, status

### Data Export
- CSV format (Excel-compatible)
- JSON format (data interchange)
- Summary reports

---

## 🎨 Design Philosophy

**Professional Corporate Theme**
- Clean, minimal interface
- Consistent spacing and typography
- Color-coded status indicators
- No emojis, focus on professionalism

**Color Palette:**
- Primary: #2C3E50 (Dark Blue-Gray)
- Accent: #3498DB (Professional Blue)
- Success: #27AE60 (Muted Green)
- Warning: #F39C12 (Amber)
- Danger: #E74C3C (Red)

---

## 📋 Project Requirements Compliance

✅ Minimum 3 screens (9 implemented)  
✅ CRUD operations (on 3 entities)  
✅ Search/filter functionality  
✅ Data persistence (SQLite + EF Core)  
✅ Input validation  
✅ Event-driven logic  
✅ Error handling  
✅ Export/import capability  
✅ Help/About section  

---

## 🏆 Exceptional Features

- Multi-entity relational database with proper foreign keys
- Dependency injection for database context
- Async/await for responsive UI
- Value converters for data transformation
- Professional documentation (3 comprehensive documents)
- Zero crashes (comprehensive error handling)
- Real-time dashboard with 8 statistics

---

## 📖 Documentation

- **Project Overview** - Complete system description and features
- **User Guide** - Step-by-step instructions with screenshots
- **Code Explanation** - Technical documentation for developers

---

## 👨‍💻 Author

**Ertan Curri**  
Visual Programming Project - December 2025

---

## 📄 License

This project was created as a university coursework assignment.

---

## 🙏 Acknowledgments

- .NET MAUI Documentation
- Entity Framework Core Documentation
- SQLite Database Engine
- Visual Studio 2022 IDE

---

**© 2025 VolunteerHub - NGO Community Management System**