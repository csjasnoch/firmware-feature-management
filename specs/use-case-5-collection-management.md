# Use Case 5: Manage Feature Collections with Firmware Versions

## Title
User Creates and Manages Feature Collection for Specific Firmware Version

## Description
A firmware engineer needs to create a collection of features targeting a specific combination of LPI and PICCOLO firmware versions. The engineer wants to ensure all features in the collection are compatible with the target firmware and can be shared with team members working on the same firmware version.

## Actors
- Primary: Firmware Engineer
- Secondary: System (Firmware Feature Management System)

## Preconditions
- LPI and PICCOLO XML metadata files are imported into the system
- Multiple firmware versions exist (T0, T1, T2, T3 stages)
- User has permissions to create collections
- At least one feature exists that can be added to collections

## Main Flow

1. User navigates to Collections view
2. User clicks "Create New Collection"
3. System displays Collection Creation wizard:
   ```
   Step 1: Basic Information
   Step 2: Select Firmware Versions
   Step 3: Add Features
   Step 4: Configure Sharing
   ```

4. User enters basic information:
   - Collection Name: "Eagan Advanced Directionality"
   - Description: "Feature set for Eagan program with enhanced directionality"
   - NPI Program: "Eagan (v3.x)"
   - Tags: "eagan", "directionality", "production"

5. User proceeds to firmware version selection
6. System displays available firmware versions filtered by user preferences:
   ```
   NPI Program: Eagan (v3.x - Current Production)
   
   LPI Versions:
   ○ 3.1.0 (T3 - Released) - 2025-12-01 [Eagan Initial]
   ○ 3.2.0 (T3 - Released) - 2026-01-15 [Eagan Update 1]
   ● 3.2.1 (T3 - Released) - 2026-01-28 [Eagan Update 2 - Latest]
   ○ 3.3.0 (T2 - Beta) - 2026-02-01 [Eagan Beta]
   
   Also Available:
   ○ 4.0.0 (T0 - Test) - Elko Program (Next Gen) [Hidden by default]
   ○ 2.8.0 (T3 - Released) - Edina Program (Previous Gen)
   
   PICCOLO Versions:
   ○ 2.4.0 (T3 - Released) - 2025-11-20 [Eagan Initial]
   ● 2.5.0 (T3 - Released) - 2026-01-10 [Eagan Update 1 - Latest]
   ○ 2.6.0 (T2 - Beta) - 2026-02-01 [Eagan Beta]
   ```

7. User selects:
   - LPI Version: 3.2.1 (T3 - Released)
   - PICCOLO Version: 2.5.0 (T3 - Released)

8. System validates version combination and displays compatibility info:
   ```
   ✓ Selected versions are compatible
   ✓ Both versions are T3 (Released) - recommended for production
   
   Available in this combination:
   - 24 LPI parameters
   - 18 PICCOLO commands
   - 12 existing features compatible
   ```

9. User proceeds to feature selection
10. System shows features compatible with selected firmware versions:
    ```
    Compatible Features (12):
    [☑] Directionality (24 params, uses LPI 3.2.1 params)
    [☑] NoiseReduction (12 params, uses LPI 3.2.1 params)
    [☑] DeviceInitialization (uses PICCOLO 2.5.0 commands)
    [☐] EqualizerSetup (8 params)
    ...
    
    Incompatible Features (3):
    [☐] AdvancedNoiseGate (requires LPI 3.3.0) ⚠️
    [☐] NewRadioProtocol (requires PICCOLO 2.6.0) ⚠️
    ```

11. User selects 3 compatible features
12. User configures sharing settings:
    - Visibility: Team
    - Share with: "Firmware Team", "QA Team"
    - Allow modifications by: "Firmware Team" only

13. User reviews collection summary:
    ```
    Collection: Hearing Aid V2 Features
    Firmware Versions:
      LPI: 3.2.1 (T3 - Released)
      PICCOLO: 2.5.0 (T3 - Released)
    Features: 3 selected
    Visibility: Team (shared with 2 teams)
    ```

14. User saves collection
15. System creates collection and validates all features against selected firmware versions
16. System displays success message and collection ID

## Input
- Collection name and description
- Selected LPI version (3.2.1)
- Selected PICCOLO version (2.5.0)
- Selected features (3)
- Sharing configuration

## Output

### Collection Created Successfully
```
Collection ID: COL-20260202-001
Name: Hearing Aid V2 Features
Status: Active

Firmware Versions:
  LPI: 3.2.1 (T3 - Released, locked)
  PICCOLO: 2.5.0 (T3 - Released, locked)

Features: 3
  1. Directionality
  2. NoiseReduction  
  3. DeviceInitialization

Shared with:
  - Firmware Team (edit access)
  - QA Team (view only)

[View Collection] [Edit] [Share] [Export]
```

## Postconditions
- Collection is created and saved in database
- Collection is visible to selected teams
- Features in collection are validated against firmware versions
- User can now work with collection or share it further

## Alternative Flows

### Alt 1: User Wants Test Build (T0)
- At step 6, user clicks "Show Advanced Versions"
- System reveals T0 and T1 builds
- User selects T0 build
- System displays warning:
  ```
  ⚠️ WARNING: T0 builds are experimental
  - IDs and names may change without notice
  - Not recommended for production collections
  - Use only for testing new features
  
  [I Understand, Continue] [Cancel]
  ```
- User acknowledges and continues

### Alt 2: Version Incompatibility Detected
- At step 15, system detects one feature uses deprecated parameter
- System displays warning:
  ```
  ⚠️ Compatibility Warning
  
  Feature: Directionality
  Issue: Uses PARAM_OLD_THRESHOLD (deprecated in LPI 3.2.0)
  Recommendation: Update to use PARAM_NEW_THRESHOLD
  
  [Update Feature] [Continue Anyway] [Remove Feature]
  ```
- User chooses action to proceed

### Alt 3: Peer Collection Sharing
- At step 12, user selects "Share with Peers"
- System shows list of peers and their firmware versions:
  ```
  Available Peers:
  [☑] Jane Doe (currently using LPI 3.2.1, PICCOLO 2.5.0) ✓ Compatible
  [☐] John Smith (currently using LPI 3.1.0, PICCOLO 2.4.0) ⚠️ Version mismatch
  [☑] Alice Johnson (currently using LPI 3.2.1, PICCOLO 2.5.0) ✓ Compatible
  ```
- User selects compatible peers

## Exception Flows

### Exception 1: No Compatible Features Found
- At step 10, system finds no features compatible with selected versions
- System displays message:
  ```
  No compatible features found for:
  LPI 3.2.1 + PICCOLO 2.5.0
  
  Suggestions:
  - Create new features for these versions
  - Select different firmware versions
  - Import features from another collection
  
  [Create Feature] [Change Versions] [Cancel]
  ```

### Exception 2: Firmware XML File Missing
- At step 6, system cannot find XML file for selected version
- System displays error:
  ```
  ❌ Firmware metadata file not found
  LPI 3.2.1 XML file is missing or corrupted
  
  [Re-import XML] [Select Different Version] [Cancel]
  ```

## Acceptance Criteria
- Collection is tied to specific LPI and PICCOLO versions
- Only compatible features can be added to collection
- Firmware version validation runs on collection creation
- Sharing settings are enforced
- Collection metadata includes version information
- User can see which firmware versions are required

## Test Cases
1. **Happy Path**: Create collection with T3 versions, add 3 features, share with team
2. **T0 Build**: Create collection with T0 LPI version, see warning, proceed
3. **Version Mismatch**: Try to add feature requiring LPI 3.3.0 to collection with LPI 3.2.1, see error
4. **Peer Sharing**: Share collection with peers, system filters by version compatibility
5. **Lock Collection**: Lock collection after creation, prevent modifications

---
_Use case auto-filled by agent. User should review firmware version selection flow and compatibility handling._
