# Use Case 2: Create Feature with Loop and Conditional Logic

## Title
User Creates New Feature with Radio Detection Loop

## Description
A software engineer needs to create a new feature called "DeviceInitialization" that requires waiting for the device radio to become detectable before proceeding with configuration commands. This involves loop logic with timeout, conditional checks, and delay mechanisms.

## Actors
- Primary: Software Engineer
- Secondary: System (Firmware Feature Management System)

## Preconditions
- User has permissions to create new features
- System has PICCOLO command definitions available
- Radio detection API is available

## Main Flow
1. User creates new feature named "DeviceInitialization"
2. User defines feature workflow:
   ```
   Step 1: Send WAKE_DEVICE command
   Step 2: Loop (max 10 iterations, 500ms delay between iterations):
           - Check if radio is detectable
           - If detectable: break loop and proceed to Step 3
           - If not detectable and max iterations reached: fail feature
   Step 3: Send CONFIGURE_RADIO command with parameters
   Step 4: Send VERIFY_CONNECTION command
   Step 5: Validate response and complete
   ```
3. User configures Step 2 loop details:
   - Loop condition: `RadioDetectionAPI.IsRadioDetectable() == true`
   - Max iterations: 10
   - Delay between iterations: 500ms
   - Timeout behavior: Fail feature and return error code
4. User defines conditional logic for Step 5:
   - If response code == 0x00: Feature succeeds
   - If response code == 0x01: Retry Step 4 (max 3 retries)
   - If response code >= 0x02: Feature fails with error
5. User saves feature definition
6. System validates the feature:
   - Checks loop semantics (max iterations, delay values)
   - Validates conditional branches
   - Ensures all PICCOLO commands are valid
   - Verifies atomic execution boundaries
7. System generates feature execution plan
8. System displays feature visualization showing:
   - Workflow diagram
   - Loop and conditional logic
   - Byte packets for each PICCOLO command
   - Estimated execution time (min/max scenarios)

## Input
### Feature Definition (Pseudocode)
```csharp
Feature: DeviceInitialization
{
    Commands: [
        WAKE_DEVICE,
        CONFIGURE_RADIO(param1=0x5A, param2=0x3C),
        VERIFY_CONNECTION
    ],
    
    Workflow: {
        Step1: ExecuteCommand(WAKE_DEVICE),
        Step2: Loop {
            Condition: RadioDetectionAPI.IsRadioDetectable(),
            MaxIterations: 10,
            DelayMs: 500,
            OnTimeout: FailFeature(ErrorCode.RADIO_NOT_DETECTED)
        },
        Step3: ExecuteCommand(CONFIGURE_RADIO),
        Step4: ExecuteCommand(VERIFY_CONNECTION),
        Step5: ValidateResponse {
            If response == 0x00: Success,
            If response == 0x01: RetryStep(Step4, MaxRetries=3),
            If response >= 0x02: FailFeature(ErrorCode.VERIFY_FAILED)
        }
    },
    
    Atomic: true,
    RollbackOnFailure: true
}
```

## Output
### Feature Visualization
```
DeviceInitialization Feature Workflow:

[Start]
   ↓
[WAKE_DEVICE Command]
   - Packet: 0x01 0x00
   ↓
[Loop: Radio Detection]
   - Max 10 iterations
   - 500ms delay
   - Condition: IsRadioDetectable()
   ├─ [Detectable] → Continue
   └─ [Timeout] → FAIL (ErrorCode.RADIO_NOT_DETECTED)
   ↓
[CONFIGURE_RADIO Command]
   - Packet: 0x02 0x5A 0x3C
   ↓
[VERIFY_CONNECTION Command]
   - Packet: 0x03 0x00
   ↓
[Validate Response]
   ├─ [0x00] → SUCCESS
   ├─ [0x01] → Retry (max 3)
   └─ [>=0x02] → FAIL (ErrorCode.VERIFY_FAILED)
   ↓
[End]

Estimated Execution Time:
  - Best case: 50ms (radio immediately detectable)
  - Worst case: 5050ms (10 iterations × 500ms + command execution time)
```

## Postconditions
- DeviceInitialization feature is created and ready for execution
- Feature is atomic (all steps succeed or all are rolled back)
- Loop and conditional logic are validated
- Byte packets are pre-calculated for all commands
- Feature can be used as a sub-feature in higher-level workflows

## Acceptance Criteria
- Loop executes correctly with specified delay and max iterations
- Radio detection check is called each iteration
- Feature fails gracefully on timeout
- Conditional response handling works for all response codes
- Byte packets are correctly generated for all commands
- Feature execution can be monitored and logged

## Test Cases
1. **Happy Path**: Radio detectable on iteration 2, all commands succeed
2. **Timeout Path**: Radio never detectable, feature fails after 10 iterations
3. **Retry Path**: VERIFY_CONNECTION returns 0x01, retries succeed on attempt 2
4. **Failure Path**: VERIFY_CONNECTION returns 0x02, feature fails immediately

---
_Auto-filled by agent. User should review loop logic, delay values, and conditional branches._
