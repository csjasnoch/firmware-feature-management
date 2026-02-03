# Firmware Feature Management System

A web application for managing firmware features, workflows, and PICCOLO protocol commands. This tool enables engineers to define, configure, and manage firmware features with complex workflow operations.

## Features

### Core Functionality
- **Feature Management** - Create, edit, and organize firmware features
- **Workflow Designer** - Visual workflow editor with operations and steps
- **Collection Management** - Group features into versioned collections
- **Firmware Data Parsing** - Parse PICCOLO protocol XML definitions

### Workflow Designer Capabilities
- **Operation Types**
  - **Sequential** - Execute steps one after another
  - **Multi-Command** - Execute multiple commands in parallel
  - **Loop** - Repeat operations with configurable variables and exit conditions
- **Step Categories**
  - Command Execution (Send Command, Wait for Response, Execute Macro)
  - Parameter Operations (Set Parameter, Get Parameter, Calculate Value)
  - Math & Logic (Arithmetic, Bitwise, Conditional)
  - Flow Control (Delay, Jump, Exit Loop)
  - Status & Validation (Check Status, Validate Response, Log Message)
- **Decision Points** - Configure success/failure actions with dropdown selectors
- **Collapsible UI** - Expand/collapse operations for better organization

## Tech Stack
- ASP.NET Core Razor Pages (.NET 10)
- Bootstrap 5
- JavaScript (client-side interactivity)

## Getting Started

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download)

### Running the Application
```sh
# Clone the repository
git clone https://github.com/csjasnoch/firmware-feature-management.git
cd firmware-feature-management

# Checkout the concept branch
git checkout concept

# Build and run
dotnet build
dotnet run
```

The application will be available at `http://localhost:5127`

## Project Structure
```
├── Models/                 # Data models (Feature, Command, Parameter definitions)
├── Pages/                  # Razor Pages
│   ├── Features/          # Feature editor and listing
│   ├── Collections/       # Collection management
│   └── Shared/            # Layout and partial views
├── Services/              # Business logic services
├── Example Firmware Data/ # Sample PICCOLO protocol XML files
├── specs/                 # UI mockups and specifications
└── wwwroot/               # Static assets (CSS, JS, libraries)
```

## Documentation
See the `specs/` folder for detailed documentation:
- [Data Model Specification](specs/data-model-specification.md)
- [Firmware Feature System Overview](specs/firmware-feature-system-overview.md)
- [Workflow Designer Mockup](specs/ui-mockup-workflow-designer.md)
- [Collection Management](specs/firmware-version-collection-management.md)

## Contributing
1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## License
This project is for internal use.