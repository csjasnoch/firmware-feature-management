# UI/UX Mockup: Feature Editor - Directionality Settings

## Screen: Edit Feature Settings

### Purpose
Allows users to view and modify parameter values for a specific feature setting (e.g., Directionality Setting 2).

### Layout

```
┌─────────────────────────────────────────────────────────────────────────────┐
│  ← Back to Dashboard                     Directionality Feature        [×]   │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                               │
│  Tabs: [Overview] [Settings ●] [Workflow] [Byte Packets] [History]          │
│                                                                               │
│  ┌─────────────────────────────────────────────────────────────────────┐    │
│  │  Settings Editor                                                     │    │
│  │                                                                      │    │
│  │  Select Setting: [● Setting 1] [○ Setting 2] [○ Setting 3] [○ Setting 4] │
│  │                                                                      │    │
│  │  Setting 2 - Enhanced Directionality              [Save] [Cancel]   │    │
│  │  Last Modified: 2026-02-02 10:30 AM                                 │    │
│  └─────────────────────────────────────────────────────────────────────┘    │
│                                                                               │
│  ┌─────────────────────────────────────────────────────────────────────┐    │
│  │  Parameter Groups                                 [Expand All]      │    │
│  │                                                                      │    │
│  │  ▼ Frontend Processing (3 parameters)                Modified ●     │    │
│  │  ┌────────────────────────────────────────────────────────────┐    │    │
│  │  │ PARAM_DIR_FRONT_GAIN                           Byte 2      │    │    │
│  │  │ Description: Front microphone gain adjustment              │    │    │
│  │  │                                                             │    │    │
│  │  │ Value: [64      ] (Decimal)  [0x40   ] (Hex)   Range: 0-100│    │    │
│  │  │        ├────────┼────────────┤                             │    │    │
│  │  │        0       64           100                             │    │    │
│  │  │                                                             │    │    │
│  │  │ [Reset to Default: 60]                                     │    │    │
│  │  └────────────────────────────────────────────────────────────┘    │    │
│  │                                                                      │    │
│  │  ┌────────────────────────────────────────────────────────────┐    │    │
│  │  │ PARAM_DIR_FRONT_SENSITIVITY                    Byte 6      │    │    │
│  │  │ Description: Front microphone sensitivity threshold        │    │    │
│  │  │                                                             │    │    │
│  │  │ Value: [85      ] (Decimal)  [0x55   ] (Hex)   Range: 0-255│    │    │
│  │  │        ├────────────┼─────────────────┤                    │    │    │
│  │  │        0           85                255                    │    │    │
│  │  └────────────────────────────────────────────────────────────┘    │    │
│  │                                                                      │    │
│  │  ┌────────────────────────────────────────────────────────────┐    │    │
│  │  │ PARAM_DIR_FRONT_DELAY                          Byte 8      │    │    │
│  │  │ Value: [43] (Decimal)  [0x2B] (Hex)   Range: 0-100         │    │    │
│  │  └────────────────────────────────────────────────────────────┘    │    │
│  │                                                                      │    │
│  │  ▼ Rear Processing (3 parameters)                                   │    │
│  │  ┌────────────────────────────────────────────────────────────┐    │    │
│  │  │ PARAM_DIR_REAR_GAIN                            Byte 3      │    │    │
│  │  │ Value: [44] (Decimal)  [0x2C] (Hex)   Range: 0-100         │    │    │
│  │  └────────────────────────────────────────────────────────────┘    │    │
│  │  ... (2 more parameters)                                            │    │
│  │                                                                      │    │
│  │  ▶ Threshold and Adaptation (18 parameters)                         │    │
│  │                                                                      │    │
│  └─────────────────────────────────────────────────────────────────────┘    │
│                                                                               │
│  ┌─────────────────────────────────────────────────────────────────────┐    │
│  │  Validation Status                                              ✓   │    │
│  │  • All parameter values are within valid ranges                     │    │
│  │  • No dependency conflicts detected                                 │    │
│  │  • Byte packet generated successfully (27 bytes)                    │    │
│  │                                                                      │    │
│  │  [Preview Byte Packet] [Validate All]                               │    │
│  └─────────────────────────────────────────────────────────────────────┘    │
│                                                                               │
│  [💾 Save Changes]  [Preview & Test]  [Discard Changes]                      │
│                                                                               │
└─────────────────────────────────────────────────────────────────────────────┘
```

### Interactive Parameter Editor (Detailed View)

```
┌─────────────────────────────────────────────────────────────────┐
│ PARAM_DIR_FRONT_GAIN                              Byte 2 | ℹ️   │
├─────────────────────────────────────────────────────────────────┤
│ Description: Front microphone gain adjustment for directional   │
│              processing. Higher values increase front focus.     │
│                                                                  │
│ Current Value:                                   Modified ●      │
│   Decimal: [64      ] ↕️                                         │
│   Hex:     [0x40    ] (auto-calculated)                         │
│                                                                  │
│ Slider:                                                          │
│   0 ├────────┼────────────┤ 100                                │
│     Low     64           High                                    │
│     │        │             │                                     │
│     └─ Safe ─┴── Normal ──┴─ Max                                │
│                                                                  │
│ Valid Range: 0 - 100                                             │
│ Default: 60                                                      │
│ Last Changed: 2026-02-02 10:30 AM by JohnDoe                    │
│                                                                  │
│ Dependencies:                                                    │
│   • Must be >= PARAM_DIR_REAR_GAIN (currently 44) ✓             │
│   • Affects PARAM_DIR_BALANCE calculation                       │
│                                                                  │
│ Impact Preview:                                                  │
│   Byte 2: 0x3C → 0x40 (60 → 64)                                │
│   Checksum: Will recalculate                                    │
│                                                                  │
│ [Reset to Default] [History] [Compare with Other Settings]      │
└─────────────────────────────────────────────────────────────────┘
```

### Bulk Edit Mode

```
┌─────────────────────────────────────────────────────────────────┐
│ Bulk Parameter Editor                                      [×]   │
├─────────────────────────────────────────────────────────────────┤
│ Selected Parameters: 3                                           │
│                                                                  │
│ [☑] PARAM_DIR_FRONT_GAIN (Byte 2)                              │
│ [☑] PARAM_DIR_REAR_GAIN (Byte 3)                               │
│ [☑] PARAM_DIR_THRESHOLD (Byte 4)                               │
│                                                                  │
│ Bulk Actions:                                                    │
│   [Set All To Value]  [Increase By %]  [Decrease By %]         │
│   [Reset All to Defaults]  [Copy from Another Setting]          │
│                                                                  │
│ Increase all selected by: [10] % [Apply]                        │
│                                                                  │
│ Preview Changes:                                                 │
│   PARAM_DIR_FRONT_GAIN: 64 → 70 (0x40 → 0x46)                  │
│   PARAM_DIR_REAR_GAIN: 44 → 48 (0x2C → 0x30)                   │
│   PARAM_DIR_THRESHOLD: 24 → 26 (0x18 → 0x1A)                   │
│                                                                  │
│ [Apply Changes] [Cancel]                                         │
└─────────────────────────────────────────────────────────────────┘
```

### User Interactions

1. **Setting Selection**: Click radio button to switch between Setting 1-4
2. **Parameter Edit**: 
   - Type in decimal or hex input (auto-syncs)
   - Use slider for visual adjustment
   - Use increment/decrement arrows
3. **Group Expand/Collapse**: Click ▼/▶ to show/hide parameter groups
4. **Validation**: Real-time validation as user types
5. **Preview**: Click "Preview Byte Packet" to see resulting packet
6. **Save**: Validates all changes, generates new packet, saves to database
7. **Bulk Edit**: Select multiple parameters, apply changes in bulk

### Validation States

```
✓ Valid: Green checkmark, no error message
⚠️ Warning: Yellow warning icon, suggestion message
  Example: "Value is outside recommended range (60-80)"
❌ Error: Red error icon, blocks saving
  Example: "Value exceeds maximum (100)"
🔗 Dependency: Blue link icon, shows related parameters
  Example: "Changes to this affect PARAM_DIR_BALANCE"
```

### Keyboard Shortcuts
- Tab/Shift+Tab: Navigate between parameters
- ↑/↓: Increment/decrement by 1
- Ctrl+↑/↓: Increment/decrement by 10
- Ctrl+S: Save changes
- Ctrl+Z: Undo last change
- Esc: Cancel editing

---
_UI mockup auto-filled by agent. User should review parameter layout, validation UX, and editing workflows._
