# UI/UX Mockup: Composite Feature Builder

## Screen: Create Composite Feature (Features of Features)

### Purpose
Visual interface for creating hierarchical composite features by combining existing features with execution order, dependencies, and rollback strategies.

### Layout

```
┌─────────────────────────────────────────────────────────────────────────────┐
│  ← Back to Dashboard           Create Composite Feature               [×]   │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                               │
│  Wizard: [1. Basic Info ●] [2. Select Sub-Features] [3. Configure] [4. Review]│
│                                                                               │
│  ┌─────────────────────────────────────────────────────────────────────┐    │
│  │ Step 2: Select and Order Sub-Features                               │    │
│  └─────────────────────────────────────────────────────────────────────┘    │
│                                                                               │
│  ┌───────────────────────┐  ┌────────────────────────────────────────┐      │
│  │ Available Features    │  │ Selected Sub-Features (3)              │      │
│  │ [🔍 Search...]       │  │                                        │      │
│  ├───────────────────────┤  │ Execution Order: [Sequential ▼]       │      │
│  │                       │  │                                        │      │
│  │ ⭐ Directionality     │  │ ┌────────────────────────────────────┐│      │
│  │    24 params, Simple  │  │ │ 1. DeviceInitialization       ↕️   ││      │
│  │    [+ Add]           │  │ │    Type: Simple                     ││      │
│  │                       │  │ │    Duration: ~1500ms                ││      │
│  │ 🔄 DeviceInit     ✓  │  │ │    Required: ☑ Yes                  ││      │
│  │    6 commands, Loop   │  │ │    Configuration: [Default ▼]      ││      │
│  │    [Added]           │  │ │    [⚙️ Configure] [🗑️ Remove]       ││      │
│  │                       │  │ └────────────────────────────────────┘│      │
│  │ 🔇 NoiseReduction ✓  │  │                                        │      │
│  │    12 params, Simple  │  │ ┌────────────────────────────────────┐│      │
│  │    [Added]           │  │ │ 2. Directionality             ↕️   ││      │
│  │                       │  │ │    Type: Simple                     ││      │
│  │ 🎚️ EqualizerSetup    │  │ │    Duration: ~45ms                  ││      │
│  │    8 params, Simple   │  │ │    Required: ☑ Yes                  ││      │
│  │    [+ Add]           │  │ │    Configuration: [Setting 2 ▼]     ││      │
│  │                       │  │ │    [⚙️ Configure] [🗑️ Remove]       ││      │
│  │ ... (20 more)        │  │ └────────────────────────────────────┘│      │
│  │                       │  │                                        │      │
│  └───────────────────────┘  │ ┌────────────────────────────────────┐│      │
│                              │ │ 3. NoiseReduction             ↕️   ││      │
│                              │ │    Type: Simple                     ││      │
│                              │ │    Duration: ~60ms                  ││      │
│                              │ │    Required: ☑ Yes                  ││      │
│                              │ │    Configuration: [Level 1 ▼]      ││      │
│                              │ │    [⚙️ Configure] [🗑️ Remove]       ││      │
│                              │ └────────────────────────────────────┘│      │
│                              │                                        │      │
│                              │ [+ Add Sub-Feature]                    │      │
│                              └────────────────────────────────────────┘      │
│                                                                               │
│  ┌─────────────────────────────────────────────────────────────────────┐    │
│  │ Execution Policy                                                     │    │
│  │                                                                      │    │
│  │ Atomic Execution: [☑] All sub-features succeed or all rollback      │    │
│  │ Stop on Failure:  [☑] Stop immediately if any sub-feature fails     │    │
│  │ Parallel Exec:    [☐] Execute sub-features in parallel (experimental)│   │
│  │                                                                      │    │
│  │ Rollback Strategy:                                                   │    │
│  │   Order: [● Reverse  ○ Forward  ○ Custom]                          │    │
│  │   On Failure: [● Rollback All  ○ Rollback Completed Only]          │    │
│  └─────────────────────────────────────────────────────────────────────┘    │
│                                                                               │
│  ┌─────────────────────────────────────────────────────────────────────┐    │
│  │ Execution Preview                                                    │    │
│  │                                                                      │    │
│  │ [START]                                                              │    │
│  │    ↓                                                                 │    │
│  │ [1. DeviceInitialization]  Est: 1500ms                              │    │
│  │    → If fails: Abort entire feature                                 │    │
│  │    ↓                                                                 │    │
│  │ [2. Directionality]  Est: 45ms                                       │    │
│  │    → If fails: Rollback [1]                                          │    │
│  │    ↓                                                                 │    │
│  │ [3. NoiseReduction]  Est: 60ms                                       │    │
│  │    → If fails: Rollback [2] then [1]                                 │    │
│  │    ↓                                                                 │    │
│  │ [SUCCESS] Total: ~1605ms                                             │    │
│  │                                                                      │    │
│  │ Rollback Path (if failure):                                          │    │
│  │    [3. Reset NoiseReduction] → [2. Reset Directionality] →          │    │
│  │    [1. Power Down Device]                                            │    │
│  └─────────────────────────────────────────────────────────────────────┘    │
│                                                                               │
│  [⬅ Previous]  [Next: Configure Dependencies ➡]  [Cancel]                   │
│                                                                               │
└─────────────────────────────────────────────────────────────────────────────┘
```

### Step 3: Configure Dependencies and Conditions

```
┌─────────────────────────────────────────────────────────────────────────────┐
│  ← Back to Dashboard           Create Composite Feature               [×]   │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                               │
│  Wizard: [1. Basic Info ✓] [2. Select Sub-Features ✓] [3. Configure ●] [4. Review]│
│                                                                               │
│  ┌─────────────────────────────────────────────────────────────────────┐    │
│  │ Step 3: Configure Dependencies and Conditional Logic                │    │
│  └─────────────────────────────────────────────────────────────────────┘    │
│                                                                               │
│  ┌─────────────────────────────────────────────────────────────────────┐    │
│  │ Dependency Graph                                  [Auto-Arrange]     │    │
│  │                                                                      │    │
│  │     ┌──────────────────┐                                            │    │
│  │     │ 1. DeviceInit    │                                            │    │
│  │     └────────┬─────────┘                                            │    │
│  │              │                                                       │    │
│  │              │ Required                                              │    │
│  │              ↓                                                       │    │
│  │     ┌──────────────────┐                                            │    │
│  │     │ 2. Directionality│                                            │    │
│  │     └────────┬─────────┘                                            │    │
│  │              │                                                       │    │
│  │              │ Optional                                              │    │
│  │              ↓                                                       │    │
│  │     ┌──────────────────┐                                            │    │
│  │     │ 3. NoiseReduction│                                            │    │
│  │     └──────────────────┘                                            │    │
│  │                                                                      │    │
│  └─────────────────────────────────────────────────────────────────────┘    │
│                                                                               │
│  ┌─────────────────────────────────────────────────────────────────────┐    │
│  │ Conditional Logic                                                    │    │
│  │                                                                      │    │
│  │ ┌──────────────────────────────────────────────────────────────┐   │    │
│  │ │ After DeviceInitialization:                                   │   │    │
│  │ │                                                               │   │    │
│  │ │ IF DeviceInitialization.Result == Success                     │   │    │
│  │ │    THEN: Continue to Directionality                           │   │    │
│  │ │    ELSE: Abort feature (return error)                         │   │    │
│  │ │                                                               │   │    │
│  │ │ [Edit Condition]                                              │   │    │
│  │ └──────────────────────────────────────────────────────────────┘   │    │
│  │                                                                      │    │
│  │ ┌──────────────────────────────────────────────────────────────┐   │    │
│  │ │ After Directionality:                                         │   │    │
│  │ │                                                               │   │    │
│  │ │ IF Directionality.Result == Success                           │   │    │
│  │ │    THEN: Continue to NoiseReduction                           │   │    │
│  │ │    ELSE: Rollback DeviceInitialization, Abort feature         │   │    │
│  │ │                                                               │   │    │
│  │ │ [Edit Condition]                                              │   │    │
│  │ └──────────────────────────────────────────────────────────────┘   │    │
│  │                                                                      │    │
│  │ [+ Add Conditional Logic]                                            │    │
│  └─────────────────────────────────────────────────────────────────────┘    │
│                                                                               │
│  ┌─────────────────────────────────────────────────────────────────────┐    │
│  │ Parameter Mapping (Cross-Feature)                                   │    │
│  │                                                                      │    │
│  │ Map output from one sub-feature to input of another:                │    │
│  │                                                                      │    │
│  │ DeviceInitialization.RadioStrength                                  │    │
│  │    → Directionality.PARAM_ADAPTATION_RATE                           │    │
│  │    Mapping: Linear scale (0-100 → 0-255)                            │    │
│  │                                                                      │    │
│  │ [+ Add Parameter Mapping]                                            │    │
│  └─────────────────────────────────────────────────────────────────────┘    │
│                                                                               │
│  [⬅ Previous]  [Next: Review ➡]  [Cancel]                                   │
│                                                                               │
└─────────────────────────────────────────────────────────────────────────────┘
```

### Step 4: Review and Save

```
┌─────────────────────────────────────────────────────────────────────────────┐
│  ← Back to Dashboard           Create Composite Feature               [×]   │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                               │
│  Wizard: [1. Basic Info ✓] [2. Select Sub-Features ✓] [3. Configure ✓] [4. Review ●]│
│                                                                               │
│  ┌─────────────────────────────────────────────────────────────────────┐    │
│  │ Step 4: Review and Save                                              │    │
│  └─────────────────────────────────────────────────────────────────────┘    │
│                                                                               │
│  ┌─────────────────────────────────────────────────────────────────────┐    │
│  │ Feature Summary                                                      │    │
│  │                                                                      │    │
│  │ Name: FullDeviceSetup                                                │    │
│  │ Type: Composite                                                      │    │
│  │ Description: Comprehensive device initialization and configuration   │    │
│  │                                                                      │    │
│  │ Sub-Features: 3                                                      │    │
│  │   1. DeviceInitialization                                            │    │
│  │   2. Directionality (Setting 2)                                      │    │
│  │   3. NoiseReduction (Level 1)                                        │    │
│  │                                                                      │    │
│  │ Execution: Sequential, Atomic                                        │    │
│  │ Rollback: Reverse order on failure                                   │    │
│  └─────────────────────────────────────────────────────────────────────┘    │
│                                                                               │
│  ┌─────────────────────────────────────────────────────────────────────┐    │
│  │ Aggregated Metrics                                                   │    │
│  │                                                                      │    │
│  │ Total Commands: 6 PICCOLO commands                                   │    │
│  │ Total Parameters: 36 unique parameters                               │    │
│  │ Total Packet Size: ~46 bytes (aggregated)                            │    │
│  │ Estimated Execution Time: 1605ms (best) - 5150ms (worst)            │    │
│  │ Rollback Commands: 3                                                 │    │
│  └─────────────────────────────────────────────────────────────────────┘    │
│                                                                               │
│  ┌─────────────────────────────────────────────────────────────────────┐    │
│  │ Validation Results                                    ✓ All Passed  │    │
│  │                                                                      │    │
│  │ ✓ All sub-features are valid and executable                         │    │
│  │ ✓ Execution order is logically sound                                │    │
│  │ ✓ Rollback strategy is feasible                                     │    │
│  │ ✓ No circular dependencies detected                                 │    │
│  │ ✓ Conditional logic is valid                                        │    │
│  │ ⚠️ Worst-case execution time exceeds 5 seconds (consider optimization)│   │
│  │                                                                      │    │
│  │ Warnings: 1    Errors: 0                                             │    │
│  └─────────────────────────────────────────────────────────────────────┘    │
│                                                                               │
│  ┌─────────────────────────────────────────────────────────────────────┐    │
│  │ Preview: Full Workflow                                               │    │
│  │ [View Detailed Workflow Diagram] [View Aggregated Byte Packets]     │    │
│  └─────────────────────────────────────────────────────────────────────┘    │
│                                                                               │
│  [⬅ Previous]  [💾 Save Feature]  [💾 Save & Execute]  [Cancel]             │
│                                                                               │
└─────────────────────────────────────────────────────────────────────────────┘
```

### User Interactions

1. **Drag to Reorder**: Drag sub-features to change execution order
2. **Configure Sub-Feature**: Click ⚙️ to set specific settings/levels
3. **Add/Remove**: Add from available list, remove from selected
4. **Dependency Visualization**: See visual graph of dependencies
5. **Conditional Logic**: Add conditions between sub-feature executions
6. **Parameter Mapping**: Map outputs to inputs across features
7. **Validation**: Real-time validation of composite structure
8. **Preview**: See complete workflow before saving

---
_UI mockup auto-filled by agent. User should review composite feature builder, dependency graph, and validation._
