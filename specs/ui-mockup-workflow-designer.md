# UI/UX Mockup: Workflow Designer

## Screen: Workflow Editor

### Purpose
Visual workflow designer for creating and editing feature workflows with loops, conditionals, delays, and command sequences.

### Layout

```
┌─────────────────────────────────────────────────────────────────────────────┐
│  ← Back to Feature                DeviceInitialization Workflow        [×]   │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                               │
│  Tabs: [Overview] [Settings] [Workflow ●] [Byte Packets] [History]          │
│                                                                               │
│  ┌───────────────────┐  ┌───────────────────────────────────────────────┐   │
│  │ Toolbox           │  │ Workflow Canvas                         [Zoom]│   │
│  ├───────────────────┤  │                                                │   │
│  │ Drag to Canvas:   │  │  ┌───────────────────────────────────────┐   │   │
│  │                   │  │  │ START                                  │   │   │
│  │ [📄] Command      │  │  │ Feature: DeviceInitialization          │   │   │
│  │                   │  │  └─────────────┬─────────────────────────┘   │   │
│  │ [🔄] Loop         │  │                ↓                             │   │
│  │                   │  │  ┌─────────────────────────────────────┐    │   │
│  │ [◆] Conditional   │  │  │ Step 1: Command                      │←─┐│   │
│  │                   │  │  │ WAKE_DEVICE                          │  ││   │
│  │ [⏱️] Delay        │  │  │ Packet: 0x01 0x00                    │  ││   │
│  │                   │  │  │ Timeout: 1000ms                      │  ││   │
│  │ [🧮] Calculation  │  │  └─────────────┬───────────────────────┘  ││   │
│  │                   │  │                ↓                           ││   │
│  │ [📦] Sub-Feature  │  │  ┌───────────────────────────────────────┐││   │
│  │                   │  │  │ Step 2: Loop 🔄                        │││   │
│  └───────────────────┘  │  │ Condition: RadioAPI.IsDetectable()    │││   │
│                         │  │ Max Iterations: 10                     │││   │
│  Properties Panel       │  │ Delay: 500ms                           │││   │
│  ┌───────────────────┐  │  │                                        │││   │
│  │ Selected:         │  │  │ ┌────────────────────────────┐        │││   │
│  │ Step 2 - Loop     │  │  │ │ Check Radio Detection       │        │││   │
│  │                   │  │  │ │ If TRUE → Break Loop        │────────┘│   │
│  │ Max Iterations:   │  │  │ │ If FALSE → Continue (delay) │──┐      │   │
│  │ [10       ] ↕️    │  │  │ └────────────────────────────┘  │      │   │
│  │                   │  │  │                                  │      │   │
│  │ Delay (ms):       │  │  │ On Timeout:                      │      │   │
│  │ [500      ] ↕️    │  │  │ [×] Fail Feature ─→ ERROR       │      │   │
│  │                   │  │  └─────────┬────────────────────────┘      │   │
│  │ Condition:        │  │            ↓                                │   │
│  │ [Edit Expression] │  │  ┌─────────────────────────────────────┐  │   │
│  │                   │  │  │ Step 3: Command                      │  │   │
│  │ On Timeout:       │  │  │ CONFIGURE_RADIO                      │  │   │
│  │ (•) Fail Feature  │  │  │ Params: 0x5A, 0x3C                  │  │   │
│  │ ( ) Continue      │  │  └─────────────┬───────────────────────┘  │   │
│  │ ( ) Retry         │  │                ↓                           │   │
│  │                   │  │  ┌─────────────────────────────────────┐  │   │
│  │ [Delete Step]     │  │  │ Step 4: Command                      │  │   │
│  └───────────────────┘  │  │ VERIFY_CONNECTION                    │  │   │
│                         │  │ Expected Response: 0x00              │  │   │
│                         │  └─────────────┬───────────────────────┘  │   │
│                         │                ↓                           │   │
│                         │  ┌───────────────────────────────────────┐│   │
│                         │  │ Step 5: Conditional ◆                 ││   │
│                         │  │ Response Validation                   ││   │
│                         │  │                                       ││   │
│                         │  │ IF response == 0x00                   ││   │
│                         │  │    ↓                                  ││   │
│                         │  │    SUCCESS ✓                          ││   │
│                         │  │                                       ││   │
│                         │  │ ELSE IF response == 0x01              ││   │
│                         │  │    ↓                                  ││   │
│                         │  │    Retry Step 4 (max 3) ↻             ││   │
│                         │  │                                       ││   │
│                         │  │ ELSE                                  ││   │
│                         │  │    ↓                                  ││   │
│                         │  │    FAIL ✗                             ││   │
│                         │  └───────────────────────────────────────┘│   │
│                         │                                            │   │
│                         │  [+ Add Step]                              │   │
│                         └────────────────────────────────────────────┘   │
│                                                                           │
│  Execution Simulation: [▶ Run Simulation] [Step Through] [Reset]         │
│                                                                           │
│  ┌─────────────────────────────────────────────────────────────────┐     │
│  │ Execution Time Estimate                                          │     │
│  │ Best Case: 50ms (radio immediately detectable)                   │     │
│  │ Worst Case: 5050ms (10 iterations × 500ms + command time)       │     │
│  │ Average: ~1500ms                                                 │     │
│  └─────────────────────────────────────────────────────────────────┘     │
│                                                                           │
│  [💾 Save Workflow]  [Validate]  [Export as Code]  [Discard Changes]     │
│                                                                           │
└───────────────────────────────────────────────────────────────────────────┘
```

### Loop Step Detail Editor

```
┌─────────────────────────────────────────────────────────────────┐
│ Loop Configuration                                         [×]   │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│ Loop Type: [Conditional ▼]                                      │
│   Options: Conditional, Fixed Count, Infinite (with break)     │
│                                                                  │
│ Condition Expression:                                            │
│ ┌──────────────────────────────────────────────────────────┐   │
│ │ RadioDetectionAPI.IsRadioDetectable()                    │   │
│ └──────────────────────────────────────────────────────────┘   │
│ [Expression Builder] [Test Expression]                          │
│                                                                  │
│ Max Iterations: [10      ] ↕️  (Prevents infinite loop)         │
│                                                                  │
│ Delay Between Iterations:                                        │
│   [500] ms  [● Fixed  ○ Exponential Backoff]                   │
│                                                                  │
│ Timeout Behavior:                                                │
│   (•) Fail entire feature with error code                       │
│       Error Code: [ErrorCode.RADIO_NOT_DETECTED ▼]             │
│   ( ) Continue to next step (log warning)                       │
│   ( ) Retry from beginning of feature                           │
│                                                                  │
│ Loop Body Steps:                                                 │
│   Currently: 1 step (Radio Detection Check)                     │
│   [+ Add Step Inside Loop]                                      │
│                                                                  │
│ Advanced Options:                                                │
│   [☐] Log each iteration                                        │
│   [☑] Break on first success                                    │
│   [☐] Collect iteration data                                    │
│                                                                  │
│ [Save] [Cancel]                                                  │
└─────────────────────────────────────────────────────────────────┘
```

### Conditional Step Detail Editor

```
┌─────────────────────────────────────────────────────────────────┐
│ Conditional Configuration                                  [×]   │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│ Conditional Type: [Multi-Branch ▼]                              │
│   Options: If/Else, Multi-Branch, Switch/Case                  │
│                                                                  │
│ ┌────────────────────────────────────────────────────────────┐ │
│ │ Branch 1: IF                                                │ │
│ │ Condition: [responseCode == 0x00         ] [📝 Edit]       │ │
│ │                                                             │ │
│ │ Then Execute:                                               │ │
│ │   → Complete Feature (Success)                              │ │
│ │   [+ Add Step]                                              │ │
│ └────────────────────────────────────────────────────────────┘ │
│                                                                  │
│ ┌────────────────────────────────────────────────────────────┐ │
│ │ Branch 2: ELSE IF                          [🗑️ Remove]      │ │
│ │ Condition: [responseCode == 0x01         ] [📝 Edit]       │ │
│ │                                                             │ │
│ │ Then Execute:                                               │ │
│ │   → Retry Step 4 (VERIFY_CONNECTION)                        │ │
│ │   → Max Retries: [3]                                        │ │
│ │   [+ Add Step]                                              │ │
│ └────────────────────────────────────────────────────────────┘ │
│                                                                  │
│ ┌────────────────────────────────────────────────────────────┐ │
│ │ Branch 3: ELSE                             [🗑️ Remove]      │ │
│ │ (Default - all other cases)                                 │ │
│ │                                                             │ │
│ │ Then Execute:                                               │ │
│ │   → Fail Feature                                            │ │
│ │   → Error Code: [ErrorCode.VERIFY_FAILED]                   │ │
│ │   [+ Add Step]                                              │ │
│ └────────────────────────────────────────────────────────────┘ │
│                                                                  │
│ [+ Add Branch]                                                   │
│                                                                  │
│ [Save] [Cancel]                                                  │
└─────────────────────────────────────────────────────────────────┘
```

### Expression Builder

```
┌─────────────────────────────────────────────────────────────────┐
│ Expression Builder                                         [×]   │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│ Build your condition:                                            │
│                                                                  │
│ [Variable ▼] [Operator ▼] [Value ▼]                            │
│                                                                  │
│ Available Variables:                                             │
│   • responseCode (byte)                                          │
│   • RadioDetectionAPI.IsRadioDetectable() (bool)               │
│   • deviceState (enum)                                           │
│   • executionCount (int)                                         │
│   • currentTimestamp (datetime)                                  │
│                                                                  │
│ Operators:                                                       │
│   ==, !=, <, >, <=, >=, &&, ||, !                               │
│                                                                  │
│ Current Expression:                                              │
│ ┌──────────────────────────────────────────────────────────┐   │
│ │ responseCode == 0x00                                     │   │
│ └──────────────────────────────────────────────────────────┘   │
│                                                                  │
│ Validation: ✓ Expression is valid                               │
│                                                                  │
│ [Test Expression]  [Clear]  [Use Expression]                    │
└─────────────────────────────────────────────────────────────────┘
```

### User Interactions

1. **Drag & Drop**: Drag steps from toolbox to canvas
2. **Connect Steps**: Auto-connects in sequence, or drag connector lines
3. **Edit Step**: Click step to show properties panel
4. **Reorder**: Drag steps up/down to change execution order
5. **Zoom/Pan**: Mouse wheel to zoom, drag to pan canvas
6. **Simulation**: Step through workflow visually to test logic
7. **Validate**: Check for errors (unreachable steps, missing connections)

### Workflow Validation

```
┌─────────────────────────────────────────────────────────────────┐
│ Workflow Validation Results                                     │
├─────────────────────────────────────────────────────────────────┤
│ ✓ All steps are reachable                                       │
│ ✓ No circular dependencies detected                             │
│ ✓ All conditionals have valid expressions                       │
│ ⚠️ Loop may timeout (worst case 5050ms exceeds recommended 3s)  │
│ ✓ Rollback strategy defined                                     │
│ ✓ Error handling complete                                       │
│                                                                  │
│ Warnings: 1    Errors: 0                                         │
│                                                                  │
│ [View Details] [Fix Warnings] [Ignore]                          │
└─────────────────────────────────────────────────────────────────┘
```

---
_UI mockup auto-filled by agent. User should review workflow designer, loop/conditional editors, and validation logic._
