# Firmware Feature Management POC

A web application for managing PICCOLO-based firmware features across multiple NPI programs and firmware versions. This POC demonstrates UI/UX for navigating pre-parsed firmware data, building features with realistic LPI parameter groups and PICCOLO command sequences, and creating version-aware operational workflows.

## Overview

This POC focuses on **UI/UX for firmware feature management**, not data parsing or version control (handled independently). Features are based on actual PiccoloInterface.xml and PiccoloProtocol.xml structures from Example Firmware Data/ folder.

## Key Capabilities
- ✅ **Firmware Version Management** - Dashboard with version matrices, global context switcher, and compatibility heatmaps
- ✅ **AI Chat Assistant** - Context-aware AI assistance on every page with suggested prompts and help
- ✅ **Feature Management** - 20 comprehensive features with realistic LPI parameter groups and PICCOLO commands
- ✅ **Cross-Collection Feature Pulling** - Pull features from one collection to another with compatibility analysis
- ✅ **Feature Compatibility Analyzer** - Analyze and adapt features for different firmware versions
- ✅ **Feature Editor** - Settings tabs (1-4) with grouped parameters, dual hex/decimal inputs, and validation
- ✅ **Byte Packet Viewer** - PICCOLO packet visualization with parameter-to-byte correlation tree and export options
- ✅ **Workflow Designer** - Visual workflow canvas with collapsible operations, loops, conditionals, and decision points
- ✅ **Collection Management** - Version-aware collections with 4-step wizard and compatibility badges (✓/⚠️/❌/🧪)
- 🚧 **Composite Feature Builder** - Multi-feature orchestration wizard (in progress)
- 🚧 **Authentication** - User-scoped collections with ASP.NET Core Identity (planned)

## Realistic Mock Data

**20 comprehensive features** with PICCOLO/LPI mappings implemented in `Services/FirmwareDataService.Enhanced.cs`:

### Core Audio Processing (7 features)
1. **Adaptive Directionality** - 4 settings (Omni, Fixed Cardioid, Adaptive, Full Adaptive), 45 parameters
2. **Advanced Noise Reduction** - 6-channel spectral processing with binaural coordination
3. **Adaptive Feedback Cancellation** - Real-time acoustic feedback suppression
4. **Output Compression Limiter (OCL)** - Multi-channel compression/limiting
5. **Multi-Channel Equalizer** - 8-band frequency shaping (125Hz-8kHz)
6. **Frequency Translation** - Non-linear frequency compression for high-frequency hearing loss
7. **DNN Speech Enhancement** - AI/ML-powered speech clarity with TensorFlow Lite integration

### Environment & Adaptation (5 features)
8. **Advanced Environment Classification** - 8 environments (Quiet, Speech, Music, Restaurant, Street, Car, Crowd, Wind) with confidence scoring
9. **Own Voice Detection** - Occlusion reduction with voice activity detection
10. **Wind Noise Adaptation** - Automatic directionality switching and spectral filtering
11. **Motion Adaptation (IMU)** - 6-axis motion sensor with activity classification (Walking, Running, Cycling, Stationary)
12. **Sleep & Health Monitoring** - Respiratory rate tracking and sleep quality analysis

### Connectivity & Streaming (4 features)
13. **Bluetooth Audio Streaming** - Wireless streaming with codec preference (SBC/AAC/LC3)
14. **Telecoil System** - T-coil/Mic mixing with automatic program switching
15. **Google Fast Pair** - One-tap Android device pairing with GATT service
16. **Auracast Broadcast Audio** - LE Audio broadcast reception for public venues

### User Experience (4 features)
17. **Tinnitus Therapy** - Sound generator with multiple noise types (white, pink, brown)
18. **Auto-Phone Detection** - RSSI-based proximity sensing with automatic streaming
19. **Multi-Memory Management** - 5 program memories with automatic switching
20. **Device Firmware Update (DFU)** - 15-command PICCOLO workflow for over-the-air updates

All features map to actual LPI parameter groups (DirectionalityPerMemory, NoiseReduction, FeedbackCanceller, etc.) and PICCOLO commands from PiccoloInterface.xml and PiccoloProtocol.xml specifications

### PICCOLO Commands
All features include realistic PICCOLO command mappings:
- **LPI Service**: LpiContextRead/Write (0x40/0x41), LpiParameterGroupRead/Write (0x44/0x45), LpiMonitorRead (0x46)
- **DFU Service**: DfuGetStatus (0x06), DfuRequest (0x10), DfuWriteParcel (0x0B), DfuGetPackageInfo (0x09), DfuHashNvmBlock (0x0E)
- **BLE Service**: BleStreamStart (0x50), BleStreamSetVolume (0x51), BleStreamStop (0x52), BleStreamGetStatus (0x53)

All commands include proper ServiceIds and payload structures from PiccoloProtocol.xml.

## Tech Stack
- **Backend**: ASP.NET Core 10.0 Razor Pages, Entity Framework Core with SQLite
- **Frontend**: Bootstrap 5, JavaScript (Alpine.js or Vue.js for reactive components)
- **Visualization**: Mermaid.js (workflow diagrams), D3.js (version matrices), custom SVG (byte correlation)

## Project Structure
```
Services/
  FirmwareDataService.cs          - Main service with data access methods (includes EF Core navigation)
  FirmwareDataService.Enhanced.cs - 20 realistic features with PICCOLO/LPI mappings
Models/
  Feature.cs, FeatureCollection.cs, FirmwareVersion.cs, NpiProgram.cs
  OperationalFlow.cs, FlowOperation.cs, FlowStep.cs, FlowBranch.cs
  ParameterDefinition.cs, CommandDefinition.cs
Pages/
  Index.cshtml              - ✅ Dashboard with version matrix, context switcher, compatibility heatmaps
  Collections/              - ✅ Collection management with 4-step wizard and compatibility badges
    Index.cshtml, Details.cshtml, Create.cshtml, Edit.cshtml, Compare.cshtml
  Features/                 - ✅ Feature editor with Settings tabs and byte packet viewer
    Index.cshtml, Edit.cshtml, BytePacket.cshtml
  Workflow/                 - ✅ Visual workflow designer with collapsible operations
    Designer.cshtml, Visualize.cshtml
Example Firmware Data/
  12.000.000.002/          - Gen4 firmware (Lamarr/Mozart PICCOLO)
  11.000.000.003/          - Gen3/Gen4 transition
  10.003.002.005/          - Gen3 firmware
  8.100.000.005/           - Gen2 firmware
  PiccoloInterface.xml     - LPI parameter definitions (34,464 lines)
  PiccoloProtocol.xml      - PICCOLO command protocol (64,980 lines)
specs/                     - UI mockups and use cases
```

## Getting Started

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- (Optional) [OpenAI API key](https://platform.openai.com/api-keys) for AI chat features

### Quick Start (Recommended)
**Double-click `Launch.bat`** in the project root - this automatically handles:
- Dependency checks (.NET SDK)
- Process cleanup (ports 5000/5001)
- Build and database setup
- Browser launch

### Manual Launch
If you prefer command-line control:

```sh
# Full rebuild with clean database
.\Launch\Relaunch.ps1 -OpenBrowser

# Quick restart (no rebuild) - fast for development
.\Launch\QuickLaunch.ps1 -OpenBrowser

# Reset database only
.\Launch\ResetDatabase.ps1
```

For detailed launch options, see [Launch/README.md](Launch/README.md)

### AI Chat Configuration (Optional)
To enable AI chat assistance:

1. Get an OpenAI API key from https://platform.openai.com/api-keys
2. Open `appsettings.Development.json`
3. Add your API key:
   ```json
   "ChatService": {
     "ApiKey": "sk-your-api-key-here",
     "Endpoint": "https://api.openai.com/v1/chat/completions",
     "Model": "gpt-4",
     "MaxTokens": "1500"
   }
   ```

**Note:** The app works fully without an API key - chat features will show a "not configured" message.

### Running the Application
```sh
# Clone the repository
git clone https://github.com/csjasnoch/firmware-feature-management.git
cd firmware-feature-management

# Checkout the concept branch
git checkout concept

# Quick launch (recommended)
.\Launch.bat

# OR manual launch
dotnet build
dotnet run
```

The application will be available at:
- **HTTP**: http://localhost:5000
- **HTTPS**: https://localhost:5001

### New Quick Actions (Dashboard)
The redesigned dashboard includes 6 context-aware quick actions:

**February 2026 Release:**
- ✅ **AI Chat Integration**: Context-aware AI assistance on all pages with OpenAI/Azure OpenAI support
- ✅ **Cross-Collection Feature Pulling**: `/Collections/PullFeature` page for importing features between collections
- ✅ **Feature Compatibility Analyzer**: `/Features/Compatibility` page with automatic adaptation suggestions
- ✅ **Enhanced Launch Scripts**: One-click `Launch.bat` with auto-browser and dependency checks
- ✅ **Redesigned Quick Actions**: 6 domain-specific actions replacing generic navigation
Controllers/
  ChatController.cs               - API endpoints for AI chat service
Launch/
  Relaunch.ps1                   - Full rebuild script with clean/restore/build
  QuickLaunch.ps1                - Fast restart without rebuild
  ResetDatabase.ps1              - Database reset utility
  README.md                      - Launch scripts documentation
Launch.bat                       - One-click launcher (recommended entry point)
Models/
  ChatContext.cs, ChatMessage.cs  - AI chat models
  CompatibilityAnalysis.cs       - Feature compatibility analysis
  RecentActivity.cs              - User activity tracking
  Feature.cs, FeatureCollection.cs, FirmwareVersion.cs, NpiProgram.cs
  OperationalFlow.cs, FlowOperation.cs, FlowStep.cs, FlowBranch.cs
  ParameterDefinition.cs, CommandDefinition.cs
Pages/
  Index.cshtml                   - ✅ Dashboard with Quick Actions and AI chat
  Collections/                   - ✅ Collection management
    Index.cshtml, Details.cshtml, Create.cshtml, Compare.cshtml
    PullFeature.cshtml           - ✅ NEW: Cross-collection feature pulling
  Features/                      - ✅ Feature management with AI assistance
    Index.cshtml, Edit.cshtml, BytePacket.cshtml
    Compatibility.cshtml         - ✅ NEW: Feature adaptation analyzer
  Workflow/                      - ✅ Workflow designer
    Designer.cshtml, Visualize.cshtml
  Shared/
    _ChatPanel.cshtml            - ✅ NEW: Reusable AI chat component
Services/
  FirmwareDataService.cs         - Main service with data access methods
  FirmwareDataService.Enhanced.cs - 20 realistic features with PICCOLO/LPI mappings
  FirmwareDataService.Extended.cs - ✅ NEW: Compatibility & cross-collection methods
  ChatService.cs                 - ✅ NEW: AI chat integration (OpenAI/Azure)
  ContextService.cs              - Firmware context management
wwwroot/
  css/chat.css                   - ✅ NEW: Chat UI styling
  js/chat.js                     - ✅ NEW: Chat client logic
Example Firmware Data/
  12.000.000.002/               - Gen4 firmware (Lamarr/Mozart PICCOLO)
  PiccoloInterface.xml          - LPI parameter definitions (34,464 lines)
  PiccoloProtocol.xml           - PICCOLO command protocol (64,980 lines)
specs/                          - UI mockups and use casess 3-6 collections)
6. **Today's Work** - Resume recent activity with context restoration

### Database
The application uses SQLite with Entity Framework Core. On first run, the database (`firmwarefeatures.db`) will be automatically created and seeded with:
- 3 NPI programs (Lamarr, Mozart, Marconi) with multiple firmware versions
- 20 comprehensive features across Core Audio, Environment Adaptation, Connectivity, and UX categories
- Sample feature collections with version compatibility indicators

## Recent Updates
- ✅ **Feature Library Expansion**: Grew from 7 to 20 comprehensive features based on PiccoloInterface.xml analysis
- ✅ **Navigation Bug Fixes**: Resolved Edit and Flow button routing issues with proper EF Core Include statements
- ✅ **Dynamic Workflow Generation**: Visualize page now generates feature-specific flows from PICCOLO commands
- ✅ **Complete POC Steps 2-6**: Dashboard, Collection Manager, Feature Editor, Byte Packet Viewer, Workflow Designer

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