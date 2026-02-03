# UI/UX Mockup: Workflow Designer

## Screen: Workflow Editor

### Purpose
Visual workflow designer for creating and editing feature workflows with operations containing steps, loops, conditionals, delays, and command sequences. Operations and steps are collapsible with informative headers for easy navigation.

### Key Concepts
- **Operation**: A container for related steps, can be Sequential (one at a time) or Multi-Command (batched)
- **Step**: An individual action within an operation (command, delay, conditional, etc.)
- **Decision Point**: Configurable success/failure routing with explicit target step selection

### Layout

```
┌─────────────────────────────────────────────────────────────────────────────┐
│  ← Back to Feature                DeviceInitialization Workflow        [×]   │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                               │
│  Tabs: [Overview] [Settings] [Workflow ●] [Byte Packets] [History]          │
│                                                                               │
│  Workflow Controls:                                                           │
│  [📋 Collapse All]  [📂 Expand All]  [➕ Add Operation]                      │
│                                                                               │
│  ┌───────────────────────────────────────────────────────────────────────┐  │
│  │                                                                         │  │
│  │  ════════════════════════════════════════════════════════════════════  │  │
│  │  ╔═══════════════════════════════════════════════════════════════════╗│  │
│  │  ║ ▼ Operation 1: Device Wake-Up                          [Sequential]║│  │
│  │  ║   2 Steps  •  Est: 50ms  •  ✓ Valid                               ║│  │
│  │  ╠═══════════════════════════════════════════════════════════════════╣│  │
│  │  ║                                                                    ║│  │
│  │  ║   Operation Type: [🔘 Sequential │ ○ Multi-Command]               ║│  │
│  │  ║                   ℹ️ Sequential executes steps one at a time       ║│  │
│  │  ║                                                                    ║│  │
│  │  ║   ┌─────────────────────────────────────────────────────────────┐ ║│  │
│  │  ║   │ ▼ Step 1.1: WAKE_DEVICE Command                             │ ║│  │
│  │  ║   │   Payload: [0x01, 0x00]  •  Timeout: 1000ms                 │ ║│  │
│  │  ║   ├─────────────────────────────────────────────────────────────┤ ║│  │
│  │  ║   │ Command: WAKE_DEVICE                                        │ ║│  │
│  │  ║   │ Description: Wakes device from sleep mode                   │ ║│  │
│  │  ║   │                                                             │ ║│  │
│  │  ║   │ Payload: [0x01, 0x00]                                       │ ║│  │
│  │  ║   │                                                             │ ║│  │
│  │  ║   │ ┌─ Decision Point ────────────────────────────────────────┐│ ║│  │
│  │  ║   │ │                                                          ││ ║│  │
│  │  ║   │ │  On Success:                                             ││ ║│  │
│  │  ║   │ │    Action: [Continue to Next Step          ▼]           ││ ║│  │
│  │  ║   │ │            ┌────────────────────────────────┐            ││ ║│  │
│  │  ║   │ │            │ • Continue to Next Step        │            ││ ║│  │
│  │  ║   │ │            │ • Skip to Step... (select)     │            ││ ║│  │
│  │  ║   │ │            │ • Skip to Operation... (select)│            ││ ║│  │
│  │  ║   │ │            │ • Complete Operation           │            ││ ║│  │
│  │  ║   │ │            │ • Complete Workflow (Success)  │            ││ ║│  │
│  │  ║   │ │            └────────────────────────────────┘            ││ ║│  │
│  │  ║   │ │                                                          ││ ║│  │
│  │  ║   │ │  On Failure:                                             ││ ║│  │
│  │  ║   │ │    Action: [Retry This Step (max 3)        ▼]           ││ ║│  │
│  │  ║   │ │            ┌────────────────────────────────┐            ││ ║│  │
│  │  ║   │ │            │ • Retry This Step (max X)      │            ││ ║│  │
│  │  ║   │ │            │ • Skip to Step... (select)     │            ││ ║│  │
│  │  ║   │ │            │ • Skip to Rollback Operation   │            ││ ║│  │
│  │  ║   │ │            │ • Fail Operation               │            ││ ║│  │
│  │  ║   │ │            │ • Fail Workflow                │            ││ ║│  │
│  │  ║   │ │            └────────────────────────────────┘            ││ ║│  │
│  │  ║   │ │    [⚙️ Configure Failure Response Code]                  ││ ║│  │
│  │  ║   │ │                                                          ││ ║│  │
│  │  ║   │ └──────────────────────────────────────────────────────────┘│ ║│  │
│  │  ║   │                                                             │ ║│  │
│  │  ║   │ [🗑️ Delete] [📋 Duplicate] [⬆️ Move Up] [⬇️ Move Down]     │ ║│  │
│  │  ║   └─────────────────────────────────────────────────────────────┘ ║│  │
│  │  ║                                                                    ║│  │
│  │  ║   ╌╌╌╌╌╌╌ [➕ Add Step Here] ╌╌╌╌╌╌╌                              ║│  │
│  │  ║                                                                    ║│  │
│  │  ║   ┌─────────────────────────────────────────────────────────────┐ ║│  │
│  │  ║   │ ▶ Step 1.2: Delay                                   50ms   │ ║│  │
│  │  ║   └─────────────────────────────────────────────────────────────┘ ║│  │
│  │  ║                                                                    ║│  │
│  │  ║   [➕ Add Step to This Operation]                                  ║│  │
│  │  ╚═══════════════════════════════════════════════════════════════════╝│  │
│  │                                                                         │  │
│  │  ─────────────────── [➕ Add Operation Here] ───────────────────────    │  │
│  │                                                                         │  │
│  │  ╔═══════════════════════════════════════════════════════════════════╗│  │
│  │  ║ ▶ Operation 2: Radio Detection Loop               [Sequential]    ║│  │
│  │  ║   1 Step  •  Est: 500-5000ms  •  Loop (max 10)  •  ✓ Valid        ║│  │
│  │  ╚═══════════════════════════════════════════════════════════════════╝│  │
│  │                                                                         │  │
│  │  ─────────────────── [➕ Add Operation Here] ───────────────────────    │  │
│  │                                                                         │  │
│  │  ╔═══════════════════════════════════════════════════════════════════╗│  │
│  │  ║ ▶ Operation 3: Configure & Verify                 [Multi-Command] ║│  │
│  │  ║   3 Steps  •  Est: 100ms  •  Batched  •  ✓ Valid                  ║│  │
│  │  ╚═══════════════════════════════════════════════════════════════════╝│  │
│  │                                                                         │  │
│  │  ─────────────────── [➕ Add Operation Here] ───────────────────────    │  │
│  │                                                                         │  │
│  │  ╔═══════════════════════════════════════════════════════════════════╗│  │
│  │  ║ ▶ Operation 4: Response Validation                [Sequential]    ║│  │
│  │  ║   1 Step  •  Conditional Logic  •  ✓ Valid                        ║│  │
│  │  ╚═══════════════════════════════════════════════════════════════════╝│  │
│  │                                                                         │  │
│  │  ─────────────────── [➕ Add Operation Here] ───────────────────────    │  │
│  │                                                                         │  │
│  │  ╔═══════════════════════════════════════════════════════════════════╗│  │
│  │  ║ ▶ Operation 5: Rollback Handler                   [Sequential]    ║│  │
│  │  ║   3 Steps  •  Est: 75ms  •  Rollback  •  ⚠️ Only on Failure       ║│  │
│  │  ╚═══════════════════════════════════════════════════════════════════╝│  │
│  │                                                                         │  │
│  │  ════════════════════════════════════════════════════════════════════  │  │
│  │                                                                         │  │
│  └───────────────────────────────────────────────────────────────────────┘  │
│                                                                               │
│  Execution Simulation: [▶ Run Simulation] [Step Through] [Reset]            │
│                                                                               │
│  ┌─────────────────────────────────────────────────────────────────┐        │
│  │ Execution Time Estimate                                          │        │
│  │ Best Case: 50ms (radio immediately detectable)                   │        │
│  │ Worst Case: 5050ms (10 iterations × 500ms + command time)       │        │
│  │ Average: ~1500ms                                                 │        │
│  └─────────────────────────────────────────────────────────────────┘        │
│                                                                               │
│  [💾 Save Workflow]  [Validate]  [Export as Code]  [Discard Changes]        │
│                                                                               │
└─────────────────────────────────────────────────────────────────────────────┘
```

### Collapsed View (Compact Navigation)

```
┌───────────────────────────────────────────────────────────────────────────┐
│                                                                             │
│  [📋 Collapse All]  [📂 Expand All]  [➕ Add Operation]                    │
│                                                                             │
│  ╔═══════════════════════════════════════════════════════════════════════╗│
│  ║ ▶ Operation 1: Device Wake-Up           [Sequential] 2 Steps • 50ms   ║│
│  ╚═══════════════════════════════════════════════════════════════════════╝│
│  ─────────────────────────── [➕] ────────────────────────────             │
│  ╔═══════════════════════════════════════════════════════════════════════╗│
│  ║ ▶ Operation 2: Radio Detection Loop     [Sequential] Loop×10 • 5000ms ║│
│  ╚═══════════════════════════════════════════════════════════════════════╝│
│  ─────────────────────────── [➕] ────────────────────────────             │
│  ╔═══════════════════════════════════════════════════════════════════════╗│
│  ║ ▶ Operation 3: Configure & Verify       [Multi-Cmd]  3 Steps • 100ms  ║│
│  ╚═══════════════════════════════════════════════════════════════════════╝│
│  ─────────────────────────── [➕] ────────────────────────────             │
│  ╔═══════════════════════════════════════════════════════════════════════╗│
│  ║ ▶ Operation 4: Response Validation      [Sequential] Conditional      ║│
│  ╚═══════════════════════════════════════════════════════════════════════╝│
│  ─────────────────────────── [➕] ────────────────────────────             │
│  ╔═══════════════════════════════════════════════════════════════════════╗│
│  ║ ▶ Operation 5: Rollback Handler         [Sequential] 3 Steps • 75ms   ║│
│  ╚═══════════════════════════════════════════════════════════════════════╝│
│                                                                             │
└───────────────────────────────────────────────────────────────────────────┘
```

### Operation Type Toggle

```
┌─────────────────────────────────────────────────────────────────┐
│ Operation Type:                                                  │
│                                                                  │
│ ┌─────────────────────────┐  ┌─────────────────────────────┐   │
│ │  🔘 Sequential          │  │  ○ Multi-Command            │   │
│ │  ─────────────────────  │  │  ─────────────────────────  │   │
│ │  Execute steps one      │  │  Batch all steps into       │   │
│ │  at a time, waiting     │  │  single transmission.       │   │
│ │  for each to complete   │  │  No intermediate checks.    │   │
│ │                         │  │                             │   │
│ │  ✓ Decision points      │  │  ⚡ Faster execution        │   │
│ │  ✓ Per-step timeouts    │  │  ✓ Atomic batch             │   │
│ │  ✓ Conditional flow     │  │  ✗ No mid-operation logic   │   │
│ └─────────────────────────┘  └─────────────────────────────┘   │
│                                                                  │
│ Current: Sequential  [Switch to Multi-Command]                   │
│                                                                  │
│ ⚠️ Switching type will reset decision point configurations      │
└─────────────────────────────────────────────────────────────────┘
```

### Expanded Operation View (With All Steps Visible)

```
╔═══════════════════════════════════════════════════════════════════════════════╗
║ ▼ Operation 3: Configure & Verify                              [Multi-Command]║
║   3 Steps  •  Est: 100ms  •  Batched  •  ✓ Valid            [⚙️] [🗑️] [📋] ║
╠═══════════════════════════════════════════════════════════════════════════════╣
║                                                                                ║
║   Operation Type: [○ Sequential │ 🔘 Multi-Command]                           ║
║                   ℹ️ Multi-Command batches all steps into single transmission  ║
║                                                                                ║
║   ┌───────────────────────────────────────────────────────────────────────┐  ║
║   │ ▼ Step 3.1: CONFIGURE_RADIO Command                                   │  ║
║   │   Payload: [0x02, 0x5A, 0x3C]  •  No Decision Points (batched)        │  ║
║   ├───────────────────────────────────────────────────────────────────────┤  ║
║   │ Command: CONFIGURE_RADIO                                              │  ║
║   │ Parameters: param1=0x5A, param2=0x3C                                  │  ║
║   │                                                                       │  ║
║   │ Payload: [0x02, 0x5A, 0x3C]                                           │  ║
║   │                                                                       │  ║
║   │ ⚡ Multi-Command Mode: Decision points handled at operation level     │  ║
║   │                                                                       │  ║
║   │ [🗑️ Delete] [📋 Duplicate] [⬆️] [⬇️]                                 │  ║
║   └───────────────────────────────────────────────────────────────────────┘  ║
║                                                                                ║
║   ╌╌╌╌╌╌╌╌╌╌╌╌╌╌╌╌╌╌╌╌ [➕ Add Step Here] ╌╌╌╌╌╌╌╌╌╌╌╌╌╌╌╌╌╌╌                ║
║                                                                                ║
║   ┌───────────────────────────────────────────────────────────────────────┐  ║
║   │ ▶ Step 3.2: VERIFY_CONNECTION                      Payload: [0x03]    │  ║
║   └───────────────────────────────────────────────────────────────────────┘  ║
║                                                                                ║
║   ╌╌╌╌╌╌╌╌╌╌╌╌╌╌╌╌╌╌╌╌ [➕ Add Step Here] ╌╌╌╌╌╌╌╌╌╌╌╌╌╌╌╌╌╌╌                ║
║                                                                                ║
║   ┌───────────────────────────────────────────────────────────────────────┐  ║
║   │ ▶ Step 3.3: READ_STATUS                            Payload: [0x04]    │  ║
║   └───────────────────────────────────────────────────────────────────────┘  ║
║                                                                                ║
║   [➕ Add Step to This Operation]                                              ║
║                                                                                ║
║   ┌─ Operation-Level Decision Point (for Multi-Command) ────────────────────┐║
║   │                                                                          │║
║   │  After all steps in this operation complete as a batch:                  │║
║   │                                                                          │║
║   │  On Success (all commands acknowledged):                                 │║
║   │    Action: [Continue to Next Operation    ▼]                            │║
║   │                                                                          │║
║   │  On Failure (any command fails):                                         │║
║   │    Action: [Skip to Operation 5: Rollback ▼]  ← Click to change target  │║
║   │                                                                          │║
║   └──────────────────────────────────────────────────────────────────────────┘║
║                                                                                ║
╚═══════════════════════════════════════════════════════════════════════════════╝
```

### Decision Point Configuration (Detailed)

```
┌─────────────────────────────────────────────────────────────────────────────┐
│ Decision Point Configuration                                           [×]   │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                               │
│ Step: 1.1 - WAKE_DEVICE Command                                              │
│                                                                               │
│ ┌───────────────────────────────────────────────────────────────────────┐   │
│ │ ON SUCCESS                                                             │   │
│ │                                                                        │   │
│ │ What happens when this step succeeds?                                  │   │
│ │                                                                        │   │
│ │ Action: [Continue to Next Step ▼]                                     │   │
│ │                                                                        │   │
│ │ Available Actions:                                                     │   │
│ │ ┌─────────────────────────────────────────────────────────────────┐  │   │
│ │ │ ● Continue to Next Step                                          │  │   │
│ │ │   → Proceeds to Step 1.2 (Delay)                                │  │   │
│ │ │                                                                  │  │   │
│ │ │ ○ Skip to Specific Step                                         │  │   │
│ │ │   Target: [Select Step...        ▼]                             │  │   │
│ │ │           ├─ Step 1.2: Delay                                    │  │   │
│ │ │           ├─ Step 2.1: Radio Check                              │  │   │
│ │ │           ├─ Step 3.1: CONFIGURE_RADIO                          │  │   │
│ │ │           └─ ... (all available steps)                          │  │   │
│ │ │                                                                  │  │   │
│ │ │ ○ Skip to Specific Operation                                    │  │   │
│ │ │   Target: [Select Operation...   ▼]                             │  │   │
│ │ │           ├─ Operation 2: Radio Detection Loop                  │  │   │
│ │ │           ├─ Operation 3: Configure & Verify                    │  │   │
│ │ │           ├─ Operation 4: Response Validation                   │  │   │
│ │ │           └─ Operation 5: Rollback Handler                      │  │   │
│ │ │                                                                  │  │   │
│ │ │ ○ Complete Current Operation                                    │  │   │
│ │ │   → Jumps to next operation in sequence                         │  │   │
│ │ │                                                                  │  │   │
│ │ │ ○ Complete Workflow Successfully                                │  │   │
│ │ │   → Ends workflow with success status                           │  │   │
│ │ └─────────────────────────────────────────────────────────────────┘  │   │
│ └───────────────────────────────────────────────────────────────────────┘   │
│                                                                               │
│ ┌───────────────────────────────────────────────────────────────────────┐   │
│ │ ON FAILURE                                                             │   │
│ │                                                                        │   │
│ │ What happens when this step fails?                                     │   │
│ │                                                                        │   │
│ │ Action: [Retry This Step ▼]                                           │   │
│ │                                                                        │   │
│ │ Retry Configuration:                                                   │   │
│ │   Max Retries: [3      ] ↕️                                           │   │
│ │   Delay Between Retries: [500    ] ms                                 │   │
│ │   After Max Retries Exhausted: [Fail Operation ▼]                     │   │
│ │                                                                        │   │
│ │ Available Actions:                                                     │   │
│ │ ┌─────────────────────────────────────────────────────────────────┐  │   │
│ │ │ ● Retry This Step                                                │  │   │
│ │ │   Configure retry attempts and behavior above ↑                  │  │   │
│ │ │                                                                  │  │   │
│ │ │ ○ Skip to Specific Step                                         │  │   │
│ │ │   Target: [Select Step...        ▼]                             │  │   │
│ │ │                                                                  │  │   │
│ │ │ ○ Skip to Rollback Operation                                    │  │   │
│ │ │   Target: [Operation 5: Rollback ▼] ← Auto-detected rollback op │  │   │
│ │ │                                                                  │  │   │
│ │ │ ○ Fail Current Operation                                        │  │   │
│ │ │   Error Code: [ErrorCode.WAKE_FAILED  ▼]                        │  │   │
│ │ │   → Parent workflow handles operation failure                    │  │   │
│ │ │                                                                  │  │   │
│ │ │ ○ Fail Entire Workflow                                          │  │   │
│ │ │   Error Code: [ErrorCode.DEVICE_ERROR ▼]                        │  │   │
│ │ │   → Immediately terminates workflow with error                   │  │   │
│ │ └─────────────────────────────────────────────────────────────────┘  │   │
│ └───────────────────────────────────────────────────────────────────────┘   │
│                                                                               │
│ [Save Configuration] [Cancel]                                                 │
└─────────────────────────────────────────────────────────────────────────────┘
```

### Add Step/Operation Dialogs

```
┌─────────────────────────────────────────────────────────────────┐
│ Add New Step                                               [×]   │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│ Adding step to: Operation 1 - Device Wake-Up                    │
│ Position: After Step 1.1 (WAKE_DEVICE)                          │
│                                                                  │
│ Select Step Type:                                                │
│                                                                  │
│ ┌────────────────────┐  ┌────────────────────┐                  │
│ │ 📄 Command         │  │ ⏱️ Delay           │                  │
│ │ Send a PICCOLO     │  │ Wait for specified │                  │
│ │ firmware command   │  │ duration           │                  │
│ └────────────────────┘  └────────────────────┘                  │
│                                                                  │
│ ┌────────────────────┐  ┌────────────────────┐                  │
│ │ 🔄 Loop            │  │ ◆ Conditional      │                  │
│ │ Repeat steps with  │  │ Branch based on    │                  │
│ │ condition check    │  │ expression result  │                  │
│ └────────────────────┘  └────────────────────┘                  │
│                                                                  │
│ ┌────────────────────┐  ┌────────────────────┐                  │
│ │ 🧮 Calculation     │  │ 📦 Sub-Feature     │                  │
│ │ Compute values for │  │ Include another    │                  │
│ │ use in later steps │  │ feature's workflow │                  │
│ └────────────────────┘  └────────────────────┘                  │
│                                                                  │
│ [Cancel]                                                         │
└─────────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────────┐
│ Add New Operation                                          [×]   │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│ Position: Between Operation 2 and Operation 3                   │
│                                                                  │
│ Operation Name: [________________________]                       │
│                                                                  │
│ Operation Type:                                                  │
│                                                                  │
│ ┌────────────────────────────────────────────────────────────┐  │
│ │  🔘 Sequential                                              │  │
│ │  Steps execute one at a time with decision points between  │  │
│ │                                                             │  │
│ │  ○ Multi-Command                                            │  │
│ │  Steps batched into single transmission, faster but less   │  │
│ │  granular control                                          │  │
│ └────────────────────────────────────────────────────────────┘  │
│                                                                  │
│ Initial Steps: [☑] Start with one empty command step            │
│                                                                  │
│ Tags: [ ] Loop   [ ] Rollback   [ ] Cleanup                     │
│                                                                  │
│ [Create Operation] [Cancel]                                      │
└─────────────────────────────────────────────────────────────────┘
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

1. **Collapse/Expand Operations**: Click ▼/▶ on operation header to toggle visibility
2. **Collapse/Expand Steps**: Click ▼/▶ on step header to toggle details
3. **Global Collapse**: Use [📋 Collapse All] / [📂 Expand All] buttons
4. **Add Step**: Click [➕ Add Step Here] between steps, or [➕ Add Step to This Operation] at end
5. **Add Operation**: Click [➕ Add Operation Here] between operations, or [➕ Add Operation] at top
6. **Reorder Steps**: Use ⬆️/⬇️ buttons or drag steps
7. **Reorder Operations**: Drag operation headers to reorder
8. **Change Operation Type**: Toggle between Sequential/Multi-Command in operation header
9. **Configure Decision Points**: Click on decision point dropdown to select action and target
10. **Edit Step**: Click step to expand and show full details
11. **Simulation**: Step through workflow visually to test logic
12. **Validate**: Check for errors (unreachable steps, missing connections)

### Collapsible Header Information

When collapsed, headers show key information at a glance:

**Operation Header (Collapsed)**:
- Operation number and name
- Operation type badge [Sequential] or [Multi-Command]
- Step count
- Estimated duration
- Special indicators (Loop, Rollback, Conditional)
- Validation status (✓ Valid, ⚠️ Warning, ❌ Error)

**Step Header (Collapsed)**:
- Step number (e.g., "Step 1.2" for Operation 1, Step 2)
- Step type and name
- Key info summary (Payload for commands, Duration for delays)
- Decision point indicator if configured

### Visual Affordances for Add Actions

```
Between Operations:
─────────────────── [➕ Add Operation Here] ───────────────────────
                          ↑
               Clearly visible divider with + button
               Hover effect highlights the insertion point

Between Steps:
╌╌╌╌╌╌╌ [➕ Add Step Here] ╌╌╌╌╌╌╌
              ↑
         Dashed line indicates step-level insertion
         Appears on hover between steps

At End of Operation:
[➕ Add Step to This Operation]
              ↑
         Always visible button at bottom of operation
```

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
│ ✓ All decision point targets are valid                          │
│                                                                  │
│ Warnings: 1    Errors: 0                                         │
│                                                                  │
│ [View Details] [Fix Warnings] [Ignore]                          │
└─────────────────────────────────────────────────────────────────┘
```

### Keyboard Shortcuts
- **Tab/Shift+Tab**: Navigate between operations and steps
- **Enter**: Expand/collapse current item
- **Ctrl+Shift+A**: Add new operation
- **Ctrl+A**: Add new step to current operation
- **Ctrl+↑/↓**: Move current step/operation up/down
- **Ctrl+D**: Duplicate current step/operation
- **Delete**: Delete current step/operation (with confirmation)
- **Ctrl+S**: Save workflow
- **Ctrl+E**: Expand all
- **Ctrl+Shift+E**: Collapse all

---
_UI mockup auto-filled by agent. User should review workflow designer, collapsible operations/steps, decision point configuration, and add operation/step flows._
