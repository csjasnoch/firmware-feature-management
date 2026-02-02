# UI/UX Mockup: Byte Packet Viewer

## Screen: Byte Packet Inspector

### Purpose
Provides detailed visualization of byte-level packets generated from features, with full correlation to parameters and feature concepts. Supports debugging and validation.

### Layout

```
┌─────────────────────────────────────────────────────────────────────────────┐
│  ← Back to Feature              Directionality - Byte Packet View      [×]   │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                               │
│  Tabs: [Overview] [Settings] [Workflow] [Byte Packets ●] [History]          │
│                                                                               │
│  Setting: [Setting 1] [Setting 2 ●] [Setting 3] [Setting 4]                  │
│                                                                               │
│  ┌─────────────────────────────────────────────────────────────────────┐    │
│  │ Packet Overview                                                      │    │
│  │                                                                      │    │
│  │ Total Size: 27 bytes (26 data + 1 checksum)                         │    │
│  │ Generated: 2026-02-02 10:30:15 AM                                   │    │
│  │ Checksum: 0xCF (Valid ✓)                                            │    │
│  │                                                                      │    │
│  │ Display Options:                                                     │    │
│  │ Format: [● Hexadecimal  ○ Decimal  ○ Binary]                       │    │
│  │ Group By: [4 bytes ▼]  Show: [☑ ASCII  ☑ Descriptions]            │    │
│  └─────────────────────────────────────────────────────────────────────┘    │
│                                                                               │
│  ┌─────────────────────────────────────────────────────────────────────┐    │
│  │ Full Packet (Hexadecimal)                      [📋 Copy] [💾 Export]│    │
│  │                                                                      │    │
│  │  0x5A 0x02 0x40 0x2C | 0x18 0x3F 0x55 0x1A |                       │    │
│  │  0x2B 0x09 0x4E 0x33 | 0x17 0x62 0x0C 0x41 |                       │    │
│  │  0x29 0x58 0x13 0x3A | 0x21 0x67 0x0F 0x4C |                       │    │
│  │  0x36 0x1D 0xCF      |                      |                       │    │
│  │                                                                      │    │
│  │  Z  .  @  ,  .  .  ?  U  .  +  .  N  3  .  b  .  A  )  X  .  :  !  │    │
│  │  g  .  L  6  .  Ï                                                   │    │
│  └─────────────────────────────────────────────────────────────────────┘    │
│                                                                               │
│  ┌─────────────────────────────────────────────────────────────────────┐    │
│  │ Byte-by-Byte Breakdown                          [⊟ Collapse All]    │    │
│  │                                                                      │    │
│  │ ┌──────────────────────────────────────────────────────────────┐   │    │
│  │ │ Byte 0 | 0x5A | 90                                   [ℹ️]     │   │    │
│  │ │──────────────────────────────────────────────────────────────│   │    │
│  │ │ Role: Command Header                                         │   │    │
│  │ │ Name: PICCOLO_SET_DIRECTIONALITY                            │   │    │
│  │ │ Description: Initiates directionality configuration command  │   │    │
│  │ │ Feature: Directionality                                      │   │    │
│  │ └──────────────────────────────────────────────────────────────┘   │    │
│  │                                                                      │    │
│  │ ┌──────────────────────────────────────────────────────────────┐   │    │
│  │ │ Byte 1 | 0x02 | 2                                    [ℹ️]     │   │    │
│  │ │──────────────────────────────────────────────────────────────│   │    │
│  │ │ Role: Setting Identifier                                     │   │    │
│  │ │ Name: SETTING_ID                                             │   │    │
│  │ │ Value: Setting 2                                             │   │    │
│  │ │ Feature: Directionality                                      │   │    │
│  │ └──────────────────────────────────────────────────────────────┘   │    │
│  │                                                                      │    │
│  │ ┌──────────────────────────────────────────────────────────────┐   │    │
│  │ │ Byte 2 | 0x40 | 64                         Modified ● [ℹ️]   │   │    │
│  │ │──────────────────────────────────────────────────────────────│   │    │
│  │ │ Role: Parameter Value                                        │   │    │
│  │ │ Parameter: PARAM_DIR_FRONT_GAIN                             │   │    │
│  │ │ Description: Front microphone gain adjustment                │   │    │
│  │ │ Range: 0-100 | Default: 60                                  │   │    │
│  │ │ Feature Group: Frontend Processing                           │   │    │
│  │ │ Dependencies: Affects PARAM_DIR_BALANCE                      │   │    │
│  │ │                                                              │   │    │
│  │ │ [Edit Value] [View History] [Reset to Default]              │   │    │
│  │ └──────────────────────────────────────────────────────────────┘   │    │
│  │                                                                      │    │
│  │ ┌──────────────────────────────────────────────────────────────┐   │    │
│  │ │ Byte 3 | 0x2C | 44                                   [ℹ️]     │   │    │
│  │ │ Parameter: PARAM_DIR_REAR_GAIN                              │   │    │
│  │ └──────────────────────────────────────────────────────────────┘   │    │
│  │                                                                      │    │
│  │ ... (Bytes 4-25 collapsed for brevity)                              │    │
│  │                                                                      │    │
│  │ ┌──────────────────────────────────────────────────────────────┐   │    │
│  │ │ Byte 26 | 0xCF | 207                                 [ℹ️]     │   │    │
│  │ │──────────────────────────────────────────────────────────────│   │    │
│  │ │ Role: Checksum                                               │   │    │
│  │ │ Algorithm: XOR of bytes 0-25                                 │   │    │
│  │ │ Calculated: 0xCF                                             │   │    │
│  │ │ Status: Valid ✓                                              │   │    │
│  │ │                                                              │   │    │
│  │ │ [Verify Checksum] [Recalculate]                              │   │    │
│  │ └──────────────────────────────────────────────────────────────┘   │    │
│  └─────────────────────────────────────────────────────────────────────┘    │
│                                                                               │
│  ┌─────────────────────────────────────────────────────────────────────┐    │
│  │ Feature Correlation Tree                            [Expand All]     │    │
│  │                                                                      │    │
│  │ ▼ Directionality Feature (Setting 2)                                │    │
│  │   ├─ ▼ Packet Metadata                                              │    │
│  │   │   ├─ Byte 0: COMMAND_HEADER (0x5A)                              │    │
│  │   │   ├─ Byte 1: SETTING_ID (0x02)                                  │    │
│  │   │   └─ Byte 26: CHECKSUM (0xCF)                                   │    │
│  │   │                                                                  │    │
│  │   ├─ ▼ Frontend Processing                                          │    │
│  │   │   ├─ Byte 2: PARAM_DIR_FRONT_GAIN (0x40)                        │    │
│  │   │   ├─ Byte 6: PARAM_DIR_FRONT_SENSITIVITY (0x55)                 │    │
│  │   │   └─ Byte 8: PARAM_DIR_FRONT_DELAY (0x2B)                       │    │
│  │   │                                                                  │    │
│  │   ├─ ▼ Rear Processing                                              │    │
│  │   │   ├─ Byte 3: PARAM_DIR_REAR_GAIN (0x2C)                         │    │
│  │   │   ├─ Byte 7: PARAM_DIR_REAR_SENSITIVITY (0x1A)                  │    │
│  │   │   └─ Byte 9: PARAM_DIR_REAR_DELAY (0x09)                        │    │
│  │   │                                                                  │    │
│  │   └─ ▶ Threshold and Adaptation (18 parameters)                     │    │
│  └─────────────────────────────────────────────────────────────────────┘    │
│                                                                               │
│  [Generate New Packet] [Compare with Another Setting] [Send to Device]       │
│                                                                               │
└─────────────────────────────────────────────────────────────────────────────┘
```

### Comparison View (Side-by-Side)

```
┌─────────────────────────────────────────────────────────────────────────────┐
│  Packet Comparison: Setting 2 vs. Setting 3                            [×]   │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                               │
│  ┌────────────────────────────────┬────────────────────────────────────┐     │
│  │ Setting 2 (Current)            │ Setting 3                          │     │
│  ├────────────────────────────────┼────────────────────────────────────┤     │
│  │ Byte 0 | 0x5A  COMMAND_HEADER │ Byte 0 | 0x5A  COMMAND_HEADER     │     │
│  │ Byte 1 | 0x02  Setting ID      │ Byte 1 | 0x03  Setting ID         │     │
│  │ Byte 2 | 0x40  FRONT_GAIN      │ Byte 2 | 0x50  FRONT_GAIN    ≠    │     │
│  │ Byte 3 | 0x2C  REAR_GAIN       │ Byte 3 | 0x35  REAR_GAIN     ≠    │     │
│  │ Byte 4 | 0x18  THRESHOLD       │ Byte 4 | 0x18  THRESHOLD     ✓    │     │
│  │ Byte 5 | 0x3F  ADAPTATION_RATE │ Byte 5 | 0x4A  ADAPTATION    ≠    │     │
│  │ ...                            │ ...                                │     │
│  │ Byte 26| 0xCF  CHECKSUM        │ Byte 26| 0xDA  CHECKSUM      ≠    │     │
│  └────────────────────────────────┴────────────────────────────────────┘     │
│                                                                               │
│  Differences: 18 bytes differ                                                │
│  Identical: 8 bytes match                                                    │
│                                                                               │
│  [Export Diff] [Highlight Differences] [Close]                               │
└─────────────────────────────────────────────────────────────────────────────┘
```

### Binary View (Bit-Level Detail)

```
┌─────────────────────────────────────────────────────────────────┐
│ Binary View: Byte 2 (PARAM_DIR_FRONT_GAIN)                 [×] │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│ Byte Value: 0x40 (64 decimal)                                   │
│                                                                  │
│ Binary Representation:                                           │
│  ┌─┬─┬─┬─┬─┬─┬─┬─┐                                             │
│  │0│1│0│0│0│0│0│0│                                             │
│  └─┴─┴─┴─┴─┴─┴─┴─┘                                             │
│   7 6 5 4 3 2 1 0  (bit positions)                              │
│                                                                  │
│ Bit Fields: (if applicable)                                      │
│   Bits 0-6: Gain Value (64)                                     │
│   Bit 7:    Reserved (0)                                        │
│                                                                  │
│ [Close]                                                          │
└─────────────────────────────────────────────────────────────────┘
```

### Export Options

```
┌─────────────────────────────────────────────────────────────────┐
│ Export Byte Packet                                         [×]  │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│ Export Format:                                                   │
│   (•) Hexadecimal Array (C/C++)                                 │
│       uint8_t packet[] = {0x5A, 0x02, 0x40, ...};               │
│                                                                  │
│   ( ) Hexadecimal String                                         │
│       "5A02402C183F551A2B09..."                                  │
│                                                                  │
│   ( ) JSON                                                       │
│       { "bytes": [90, 2, 64, ...], "metadata": {...} }          │
│                                                                  │
│   ( ) CSV with Descriptions                                      │
│       Index,Hex,Dec,Parameter,Description                        │
│       0,0x5A,90,COMMAND_HEADER,...                              │
│                                                                  │
│   ( ) Binary File (.bin)                                         │
│                                                                  │
│ Include Metadata:                                                │
│   [☑] Parameter names                                           │
│   [☑] Descriptions                                              │
│   [☐] Feature hierarchy                                         │
│   [☑] Checksum details                                          │
│                                                                  │
│ [Export to File] [Copy to Clipboard] [Cancel]                   │
└─────────────────────────────────────────────────────────────────┘
```

### User Interactions

1. **Byte Selection**: Click any byte to see detailed info in side panel
2. **Expand/Collapse**: Click byte card to expand/collapse details
3. **Edit Value**: Click "Edit Value" to jump to Settings tab for that parameter
4. **Compare**: Select another setting to see side-by-side diff
5. **Copy**: Copy individual bytes, ranges, or entire packet
6. **Export**: Export in various formats for external tools
7. **Binary View**: Switch to bit-level view for packed bit fields
8. **Correlation Tree**: Click parameter to highlight corresponding byte

### Visual Indicators

```
✓ Valid: Green checkmark (value within range, checksum valid)
● Modified: Blue dot (value changed from default)
≠ Different: Orange indicator (when comparing)
⚠️ Warning: Yellow warning (unusual value, not recommended)
❌ Error: Red error (invalid value, checksum mismatch)
🔗 Linked: Blue link (parameter has dependencies)
```

---
_UI mockup auto-filled by agent. User should review byte visualization, comparison features, and export options._
