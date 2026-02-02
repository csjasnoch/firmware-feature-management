# Use Case 3: Compose Hierarchical Features (Features of Features)

## Title
User Creates Composite Feature from Existing Sub-Features

## Description
A test engineer needs to create a comprehensive "FullDeviceSetup" feature that combines multiple existing features (DeviceInitialization, Directionality, NoiseReduction) into a single atomic operation. This demonstrates feature composition and hierarchical grouping.

## Actors
- Primary: Test Engineer
- Secondary: System (Firmware Feature Management System)

## Preconditions
- The following features exist and are validated:
  - DeviceInitialization (from Use Case 2)
  - Directionality (4 settings, 24 parameters each)
  - NoiseReduction (3 levels, 12 parameters each)
- User has permissions to create composite features

## Main Flow
1. User creates new composite feature named "FullDeviceSetup"
2. User selects composition mode and adds sub-features:
   - Sub-feature 1: DeviceInitialization (no parameters, uses defaults)
   - Sub-feature 2: Directionality (user selects Setting 2)
   - Sub-feature 3: NoiseReduction (user selects Level 1)
3. User defines execution order and dependencies:
   ```
   Order 1: DeviceInitialization (must succeed before proceeding)
   Order 2: Directionality + NoiseReduction (can execute in parallel if device supports)
   ```
4. User configures composite feature settings:
   - Atomic execution: true (all sub-features succeed or all rollback)
   - Parallel execution: false (execute sub-features sequentially for safety)
   - Rollback strategy: Reverse order (NoiseReduction → Directionality → DeviceInitialization)
5. User defines conditional logic:
   ```
   If DeviceInitialization fails:
     - Abort entire FullDeviceSetup feature
     - Return error from DeviceInitialization
   
   If Directionality OR NoiseReduction fails:
     - Rollback DeviceInitialization
     - Rollback successful sub-features
     - Return composite error with details
   ```
6. User saves composite feature definition
7. System validates:
   - All sub-features are valid and executable
   - Execution order is logically sound
   - Rollback strategy is feasible
   - No circular dependencies
8. System calculates aggregated metrics:
   - Total command count: Sum of all sub-feature commands
   - Total parameter count: Sum of all unique parameters
   - Total byte packet size: Aggregated packet sizes
   - Estimated execution time: Sum with consideration for delays/loops
9. System displays composite feature visualization

## Input
### Composite Feature Configuration
```yaml
Feature: FullDeviceSetup
Type: Composite
Atomic: true
ParallelExecution: false

SubFeatures:
  - Name: DeviceInitialization
    Order: 1
    Required: true
    Configuration: default
  
  - Name: Directionality
    Order: 2
    Required: true
    Configuration:
      Setting: 2
  
  - Name: NoiseReduction
    Order: 3
    Required: true
    Configuration:
      Level: 1

ExecutionPolicy:
  StopOnFailure: true
  RollbackOnFailure: true
  RollbackOrder: Reverse

ConditionalLogic:
  OnDeviceInitializationFailure:
    - AbortFeature
    - ReturnError(ErrorCode.INITIALIZATION_FAILED)
  
  OnDirectionalityFailure:
    - RollbackAll
    - ReturnError(ErrorCode.DIRECTIONALITY_FAILED)
  
  OnNoiseReductionFailure:
    - RollbackAll
    - ReturnError(ErrorCode.NOISE_REDUCTION_FAILED)
```

## Output
### Composite Feature Visualization
```
FullDeviceSetup Composite Feature

Execution Flow:
  [Start]
     ↓
  [1. DeviceInitialization]
     ├─ WAKE_DEVICE (0x01 0x00)
     ├─ Loop: Radio Detection (max 10 iterations, 500ms)
     ├─ CONFIGURE_RADIO (0x02 0x5A 0x3C)
     └─ VERIFY_CONNECTION (0x03 0x00)
     ↓
  [2. Directionality - Setting 2]
     ├─ SET_DIRECTIONALITY (0x5A 0x02 ...)
     └─ 24 parameters → 26-byte packet
     ↓
  [3. NoiseReduction - Level 1]
     ├─ SET_NOISE_REDUCTION (0x6B 0x01 ...)
     └─ 12 parameters → 14-byte packet
     ↓
  [End - Success]

Rollback Flow (if failure):
  [3. NoiseReduction] → RESET_NOISE_REDUCTION
     ↓
  [2. Directionality] → RESET_DIRECTIONALITY
     ↓
  [1. DeviceInitialization] → POWER_DOWN_DEVICE

Aggregated Metrics:
  - Total Sub-Features: 3
  - Total Commands: 6 (WAKE, CONFIGURE, VERIFY, SET_DIR, SET_NR, implicit resets)
  - Total Parameters: 36 (0 + 24 + 12)
  - Total Packet Size: ~46 bytes
  - Est. Execution Time: 5100ms - 5500ms (including DeviceInit loop worst case)
  - Rollback Commands: 3 (if needed)

Byte Packet Summary:
  DeviceInitialization:
    - 0x01 0x00 (WAKE_DEVICE)
    - 0x02 0x5A 0x3C (CONFIGURE_RADIO)
    - 0x03 0x00 (VERIFY_CONNECTION)
  
  Directionality (Setting 2):
    - 0x5A 0x02 0x40 0x2C 0x18 ... [26 bytes total]
  
  NoiseReduction (Level 1):
    - 0x6B 0x01 0x32 0x1E 0x0A ... [14 bytes total]
```

## Postconditions
- FullDeviceSetup composite feature is created and validated
- Feature hierarchy is established (parent → children)
- Atomic execution guarantees are in place
- Rollback strategy is defined and tested
- Composite feature can be executed as a single operation
- Users can inspect individual sub-features or the aggregate view

## Acceptance Criteria
- All sub-features execute in specified order
- Failure in any sub-feature triggers rollback of all completed sub-features
- User can see both hierarchical and flat views of the feature
- Byte packets for all sub-features are accessible
- Execution time estimates are accurate within 10% margin
- Composite feature can itself be used as a sub-feature in other composites

## Test Cases
1. **All Sub-Features Succeed**: FullDeviceSetup completes successfully, no rollback
2. **DeviceInitialization Fails**: Entire feature aborts, no other sub-features execute
3. **Directionality Fails**: DeviceInitialization is rolled back, NoiseReduction does not execute
4. **NoiseReduction Fails**: Both Directionality and DeviceInitialization are rolled back in reverse order
5. **Inspect Sub-Feature Details**: User can drill down into each sub-feature to see parameters and packets

## Notes
- Composite features enable reusability and standardization of common workflows
- Granular control is preserved: users can adjust individual sub-feature settings
- Grouping provides abstraction: users can work at high or low levels as needed

---
_Auto-filled by agent. User should review sub-feature selection, execution order, and rollback strategy._
