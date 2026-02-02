# Use Case 1: Adjust Directionality Feature Settings

## Title
User Adjusts Directionality Feature Parameter Values

## Description
A firmware engineer needs to update the Directionality feature to modify the parameter values for Setting 2. The Directionality feature has 4 settings, each mapping to a specific combination of 24 firmware parameters. The engineer wants to change parameter values, validate the changes, and inspect the resulting byte packets.

## Actors
- Primary: Firmware Engineer
- Secondary: System (Firmware Feature Management System)

## Preconditions
- Directionality feature is defined in the system
- Feature has 4 settings (Setting 1, Setting 2, Setting 3, Setting 4)
- Each setting has 24 parameter mappings
- User has appropriate permissions to modify features

## Main Flow
1. User opens the Directionality feature definition
2. System displays:
   - Feature name and description
   - Available settings (1-4)
   - Current parameter mappings for each setting
3. User selects Setting 2 to edit
4. System displays all 24 parameters for Setting 2 with current values:
   - Parameter names (e.g., PARAM_DIR_FRONT_GAIN, PARAM_DIR_REAR_GAIN)
   - Current values (hexadecimal and decimal)
   - Valid ranges and constraints
5. User modifies 3 parameter values:
   - PARAM_DIR_FRONT_GAIN: 0x3C → 0x40 (60 → 64)
   - PARAM_DIR_REAR_GAIN: 0x28 → 0x2C (40 → 44)
   - PARAM_DIR_THRESHOLD: 0x14 → 0x18 (20 → 24)
6. User saves changes
7. System validates the changes:
   - Checks value ranges
   - Validates parameter dependencies
   - Recalculates derived values if needed
8. System generates byte packets for Setting 2
9. System displays packet visualization:
   - Full byte packet in hexadecimal
   - Byte-by-byte breakdown
   - Correlation of each byte to parameter and feature

## Output
### Byte Packet Visualization (Example)
```
Full Packet (Hex): 0x5A 0x02 0x40 0x2C 0x18 ... [total 24 bytes + header]

Breakdown:
  Byte 0: 0x5A - Command Header (PICCOLO_SET_DIRECTIONALITY)
  Byte 1: 0x02 - Setting ID (Setting 2)
  Byte 2: 0x40 - PARAM_DIR_FRONT_GAIN (64 decimal)
  Byte 3: 0x2C - PARAM_DIR_REAR_GAIN (44 decimal)
  Byte 4: 0x18 - PARAM_DIR_THRESHOLD (24 decimal)
  ...
  Byte 25: 0xCF - Checksum

Feature Correlation:
  Directionality Feature → Setting 2 → 24 Parameters → 26-byte packet
```

## Postconditions
- Directionality Setting 2 parameter values are updated
- New byte packet is generated and ready for firmware transmission
- Change history is logged for audit purposes
- Other settings (1, 3, 4) remain unchanged

## Alternative Flows
### Alt 1: Invalid Parameter Value
- At step 5, user enters value outside valid range
- System displays validation error with valid range
- User corrects value or cancels operation

### Alt 2: Parameter Dependency Conflict
- At step 7, system detects parameter dependency violation
- System displays conflict details and suggested resolutions
- User adjusts conflicting parameters or cancels operation

## Exception Flows
### Exception 1: System Validation Failure
- System cannot generate valid byte packet
- System rolls back all changes
- User is notified of the failure reason

---
_Auto-filled by agent. User should review and adjust as needed._
