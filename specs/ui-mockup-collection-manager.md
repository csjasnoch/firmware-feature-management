# UI/UX Mockup: Collection Manager with Firmware Versions

## Screen: Collections Dashboard

### Purpose
Main view for managing feature collections organized by firmware versions. Provides quick access to collections, firmware version filtering, and sharing capabilities.

### Layout

```
┌─────────────────────────────────────────────────────────────────────────────┐
│  Firmware Feature Management System                    [User: Admin] [Help] │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                               │
│  Navigation: [Features] [Collections ●] [Firmware Versions] [Settings]       │
│                                                                               │
│  ┌─────────────────────────────────────────────────────────────────────┐    │
│  │  Quick Actions                                                       │    │
│  │  [+ New Collection]  [Import XML]  [Version Manager]  [My Settings] │    │
│  └─────────────────────────────────────────────────────────────────────┘    │
│                                                                               │
│  ┌─────────────────────────────────────────────────────────────────────┐    │
│  │  Current Device Context                                [Change]     │    │
│  │                                                                      │    │
│  │  NPI Program: Eagan (v3.x - Current Production)                     │    │
│  │  LPI Version: 3.2.1 (T3 - Released) 📌                              │    │
│  │  PICCOLO Version: 2.5.0 (T3 - Released) 📌                          │    │
│  │                                                                      │    │
│  │  Version Preferences: [☑ T3] [☐ T2] [☐ T1] [☐ T0] [☐ Peer Builds]  │    │
│  └─────────────────────────────────────────────────────────────────────┘    │
│                                                                               │
│  ┌─────────────────────────────────────────────────────────────────────┐    │
│  │  Filters & Search                                      [🔍 Search]  │    │
│  │                                                                      │    │
│  │  View: [● My Collections  ○ Shared with Me  ○ All]                 │    │
│  │  Filter by Firmware: [LPI Version ▼] [PICCOLO Version ▼]           │    │
│  │  Filter by Stage: [All ▼]  Tags: [         ]                       │    │
│  └─────────────────────────────────────────────────────────────────────┘    │
│                                                                               │
│  Collections (8 matching)                               [List ●] [Cards]     │
│  ┌─────────────────────────────────────────────────────────────────────┐    │
│  │ Name / Versions          Features  Modified    Compatibility  Actions│    │
│  ├─────────────────────────────────────────────────────────────────────┤    │
│  │ 📦 Eagan Advanced Dir.   3         2026-02-02  ✓ Compatible  [Open] │    │
│  │    Eagan | LPI 3.2.1 | PICCOLO 2.5.0                        [Edit] │    │
│  │    🏷️ eagan, directionality, production                      [⋮]   │    │
│  │    👥 Shared: Firmware Team, QA Team                                 │    │
│  ├─────────────────────────────────────────────────────────────────────┤    │
│  │ 📦 Eagan Beta Features   5         2026-02-01  ⚠️ Partial    [Open] │    │
│  │    Eagan (Beta) | LPI 3.3.0 (T2) | PICCOLO 2.6.0 (T2)       [Edit] │    │
│  │    🏷️ eagan, beta, testing                                   [⋮]   │    │
│  │    👤 Private                                                        │    │
│  │    ⚠️ Current device: LPI 3.2.1 (1 minor version behind)             │    │
│  ├─────────────────────────────────────────────────────────────────────┤    │
│  │ 📦 Edina Legacy Set      12        2026-01-15  ❌ Incomp.    [Open] │    │
│  │    Edina (Gen2) | LPI 2.8.0 | PICCOLO 1.9.0                  [Edit] │    │
│  │    🏷️ edina, legacy, deprecated                              [⋮]   │    │
│  │    👥 Shared: Support Team                                           │    │
│  │    ❌ Major version mismatch with current device (Eagan)             │    │
│  ├─────────────────────────────────────────────────────────────────────┤    │
│  │ 📦 Elko Next Gen (T0)    2         2026-02-02  🧪 Test Only  [Open] │    │
│  │    Elko (Next Gen) | LPI 4.0.0 (T0) | PICCOLO 3.0.0 (T0)    [Edit] │    │
│  │    🏷️ elko, experimental, unstable                           [⋮]   │    │
│  │    👤 Private                                                        │    │
│  │    🧪 T0 builds - IDs/names may change                               │    │
│  ├─────────────────────────────────────────────────────────────────────┤    │
│  │ 📦 Shared from Jane Doe  7         2026-01-30  ✓ Compatible  [Open] │    │
│  │    LPI 3.2.1 | PICCOLO 2.5.0                                 [View] │    │
│  │    🏷️ peer-shared, directionality                            [Fork] │    │
│  │    👤 From: jane.doe@company.com (Read only)                         │    │
│  ├─────────────────────────────────────────────────────────────────────┤    │
│  │ ... (3 more collections)                                             │    │
│  └─────────────────────────────────────────────────────────────────────┘    │
│                                                                               │
│  [1] [2]  (Page 1 of 2)                                                      │
│                                                                               │
└─────────────────────────────────────────────────────────────────────────────┘
```

### Collection Card (Expanded View)

```
┌─────────────────────────────────────────────────────────────────────────────┐
│ 📦 Eagan Advanced Directionality                          [Open] [Edit] [⋮] │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                               │
│ NPI Program: Eagan (v3.x - Current Production)                               │
│                                                                               │
│ Firmware Versions:                                                            │
│   LPI: 3.2.1 (T3 - Released, 2026-01-28) 📌 Locked                          │
│   PICCOLO: 2.5.0 (T3 - Released, 2026-01-10) 📌 Locked                      │
│                                                                               │
│ Compatibility with Current Device:                                            │
│   ✓ LPI: Exact match (3.2.1)                                                 │
│   ✓ PICCOLO: Exact match (2.5.0)                                             │
│   Status: Fully Compatible - Ready for deployment                            │
│                                                                               │
│ Features (3):                                                                 │
│   1. Directionality (24 params)                                              │
│   2. NoiseReduction (12 params)                                              │
│   3. DeviceInitialization (6 commands)                                       │
│                                                                               │
│ Metadata:                                                                     │
│   Owner: john.doe@company.com                                                │
│   Created: 2026-02-02 10:00 AM                                               │
│   Modified: 2026-02-02 2:30 PM                                               │
│   Tags: v2-device, directionality, production                                │
│                                                                               │
│ Sharing:                                                                      │
│   Visibility: Team                                                            │
│   Shared with: Firmware Team (edit), QA Team (view)                         │
│                                                                               │
│ Actions:                                                                      │
│ [View Details] [Export Collection] [Duplicate] [Share] [Lock/Unlock]        │
│                                                                               │
└─────────────────────────────────────────────────────────────────────────────┘
```

### Version Compatibility Indicators

```
✓ Compatible       Green checkmark - Exact version match or compatible
⚠️ Partial         Yellow warning - Minor/patch differences, may work
❌ Incompatible    Red X - Major version mismatch, will not work
🧪 Test Only       Purple beaker - T0/T1 build, experimental
📌 Locked          Blue pin - T3 release, production-ready
```

## Screen: Create New Collection Wizard

### Step 1: Basic Information

```
┌─────────────────────────────────────────────────────────────────────────────┐
│  Create New Collection                                                  [×] │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                               │
│  Step: [1. Basic Info ●] [2. Firmware Versions] [3. Features] [4. Sharing]  │
│                                                                               │
│  ┌─────────────────────────────────────────────────────────────────────┐    │
│  │ Collection Details                                                   │    │
│  │                                                                      │    │
│  │ Name: *                                                              │    │
│  │ [Hearing Aid V2 Features                                          ] │    │
│  │                                                                      │    │
│  │ Description:                                                         │    │
│  │ ┌──────────────────────────────────────────────────────────────┐   │    │
│  │ │ Feature set for V2 device with enhanced directionality       │   │    │
│  │ │ and improved noise reduction algorithms.                     │   │    │
│  │ └──────────────────────────────────────────────────────────────┘   │    │
│  │                                                                      │    │
│  │ Tags: (comma-separated)                                              │    │
│  │ [v2-device, directionality, production                            ] │    │
│  │                                                                      │    │
│  │ Template: (optional)                                                 │    │
│  │ [None ▼]                                                            │    │
│  │   Options: None, Hearing Aid Standard, Bluetooth Audio, Custom     │    │
│  └─────────────────────────────────────────────────────────────────────┘    │
│                                                                               │
│  [Cancel]                                    [Next: Select Versions ➡]       │
│                                                                               │
└─────────────────────────────────────────────────────────────────────────────┘
```

### Step 2: Select Firmware Versions

```
┌─────────────────────────────────────────────────────────────────────────────┐
│  Create New Collection                                                  [×] │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                               │
│  Step: [1. Basic Info ✓] [2. Firmware Versions ●] [3. Features] [4. Sharing]│
│                                                                               │
│  ┌─────────────────────────────────────────────────────────────────────┐    │
│  │ Select Target Firmware Versions                                     │    │
│  │                                                                      │    │
│  │ These versions determine which parameters and commands are           │    │
│  │ available for features in this collection.                           │    │
│  └─────────────────────────────────────────────────────────────────────┘    │
│                                                                               │
│  ┌────────────────────────────────────┬────────────────────────────────┐    │
│  │ LPI Version (Parameters)           │ PICCOLO Version (Commands)     │    │
│  ├────────────────────────────────────┼────────────────────────────────┤    │
│  │ Show: [☑ T3] [☐ T2] [☐ T1] [☐ T0] │ Show: [☑ T3] [☐ T2] [☐ T1] [☐ T0]│    │
│  │                                    │                                │    │
│  │ Available Versions:                │ Available Versions:            │    │
│  │                                    │                                │    │
│  │ ○ 3.1.0 (T3)                       │ ○ 2.4.0 (T3)                   │    │
│  │   Released: 2025-12-01             │   Released: 2025-11-20         │    │
│  │   24 parameters                    │   16 commands                  │    │
│  │                                    │                                │    │
│  │ ○ 3.2.0 (T3)                       │ ● 2.5.0 (T3) 📌                │    │
│  │   Released: 2026-01-15             │   Released: 2026-01-10         │    │
│  │   26 parameters                    │   18 commands                  │    │
│  │   + 2 new params                   │   + 2 new commands             │    │
│  │                                    │   [View Changelog]             │    │
│  │ ● 3.2.1 (T3) 📌                    │                                │    │
│  │   Released: 2026-01-28             │ ○ 2.6.0 (T2) 🧪                │    │
│  │   26 parameters (Latest)           │   Beta: 2026-02-01             │    │
│  │   Bug fixes only                   │   20 commands                  │    │
│  │   [View Changelog]                 │   + Experimental features      │    │
│  │                                    │                                │    │
│  │ [Show T2/T1/T0 Builds]             │ [Show T2/T1/T0 Builds]         │    │
│  └────────────────────────────────────┴────────────────────────────────┘    │
│                                                                               │
│  ┌─────────────────────────────────────────────────────────────────────┐    │
│  │ Version Combination Summary                                 ✓ Valid  │    │
│  │                                                                      │    │
│  │ Selected:                                                            │    │
│  │   LPI 3.2.1 (T3) + PICCOLO 2.5.0 (T3)                               │    │
│  │                                                                      │    │
│  │ This combination provides:                                           │    │
│  │   • 26 LPI parameters                                                │    │
│  │   • 18 PICCOLO commands                                              │    │
│  │   • 12 existing compatible features                                  │    │
│  │                                                                      │    │
│  │ Stage Status: Both T3 (Released) ✓ Recommended for production       │    │
│  │                                                                      │    │
│  │ [View Detailed Comparison] [Load from Device]                       │    │
│  └─────────────────────────────────────────────────────────────────────┘    │
│                                                                               │
│  [⬅ Previous]              [Next: Add Features ➡]              [Cancel]      │
│                                                                               │
└─────────────────────────────────────────────────────────────────────────────┘
```

### Advanced: Show Test Builds (T0/T1)

```
┌─────────────────────────────────────────────────────────────────┐
│ ⚠️ Show Experimental Builds (T0/T1)                        [×] │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│ You are about to view test builds that are not finalized.       │
│                                                                  │
│ Risks:                                                           │
│   ❌ Parameter/command IDs may change without notice            │
│   ❌ Names may be renamed in future builds                      │
│   ❌ Features may be removed or significantly altered           │
│   ❌ Not recommended for production collections                 │
│   ❌ May break compatibility with future releases               │
│                                                                  │
│ Use Cases:                                                       │
│   ✓ Testing new firmware features before release                │
│   ✓ Providing feedback to firmware team                         │
│   ✓ Prototyping for future versions                             │
│                                                                  │
│ [☐] I understand the risks and want to proceed                  │
│                                                                  │
│ [Show Test Builds] [Cancel]                                     │
└─────────────────────────────────────────────────────────────────┘
```

---
_UI mockup auto-filled by agent. User should review firmware version selection, compatibility indicators, and collection organization._
