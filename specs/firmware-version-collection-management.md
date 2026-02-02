# Firmware Version and Collection Management Specification

## Title
Feature Collections with LPI and PICCOLO Firmware Version Management

## Description
A system for organizing features into collections that are tied to specific versions of firmware metadata (LPI parameters and PICCOLO commands). Features are organized by NPI (New Product Integration) Programs managed by the PMO group. Each NPI program (e.g., Eagan, Elko, Edina, Fridley, Chaska) represents a product generation and typically corresponds to firmware major versions, though sometimes hardware differences drive version changes. Supports firmware release lifecycle from T0 test builds to T3 locked releases, with strict semantic versioning and compatibility rules.

## Core Concepts

### Collection
A named grouping of related features that targets a specific combination of LPI and PICCOLO firmware versions. Collections enable:
- Organized feature management by firmware version
- Team collaboration on version-specific features
- Testing against different firmware builds
- Version compatibility tracking

### Firmware Metadata Files

#### LPI (Parameters) XML
Contains firmware parameter definitions including:
- Parameter names and IDs
- Data types and ranges
- Default values
- Semantic versioning metadata

#### PICCOLO (Commands) XML
Contains firmware command definitions including:
- Command names and IDs
- Parameter structures
- Expected responses
- Semantic versioning metadata

### Firmware Release Lifecycle

```
T0 (Test Build)
├─ Characteristics:
│  ├─ Experimental commands/parameters
│  ├─ IDs and names may change
│  ├─ Not for production use
│  └─ Frequent updates
│
├─ T1 (Alpha)
│  ├─ More stable than T0
│  ├─ IDs may still change
│  └─ Names becoming more stable
│
├─ T2 (Beta)
│  ├─ IDs locked for new items
│  ├─ Names locked
│  └─ Breaking changes require major version bump
│
└─ T3 (Released/Locked)
   ├─ Fully locked and immutable
   ├─ IDs and names cannot change
   ├─ Semantic versioning enforced
   └─ Production-ready
```

### Semantic Versioning Rules

Firmware metadata follows semantic versioning: `MAJOR.MINOR.PATCH`

#### MAJOR version increment:
- Breaking changes to existing commands/parameters
- Removal of commands/parameters
- Change to existing parameter IDs or command codes

#### MINOR version increment:
- Addition of new commands/parameters
- Backward-compatible enhancements
- New optional parameters

#### PATCH version increment:
- Documentation updates
- Bug fixes in metadata
- Clarifications that don't affect functionality

### Version Compatibility Matrix

```
Feature Collection ─── requires ──→ LPI Version (e.g., 3.2.1)
                  └─── requires ──→ PICCOLO Version (e.g., 2.5.0)
                  
Compatibility Rules:
- Exact match: Fully supported
- Minor/Patch difference: Compatible (warnings)
- Major difference: Incompatible (errors)
```

## NPI Program Data Model

```csharp
public class NpiProgram
{
    public Guid Id { get; set; }
    public string Name { get; set; } // e.g., "Eagan", "Elko", "Edina"
    public string Description { get; set; }
    public int MajorVersion { get; set; } // Usually matches program (Eagan = v3, Elko = v4)
    public HardwareGeneration HardwareGen { get; set; } // Sometimes versions are hardware-driven
    public List<FirmwareVersion> FirmwareVersions { get; set; }
    public DateTime ProgramStartDate { get; set; }
    public ProgramStatus Status { get; set; }
    public string PmoOwner { get; set; } // PMO group contact
}

public enum HardwareGeneration
{
    Gen1,
    Gen2,
    Gen3,
    Gen4
}

public enum ProgramStatus
{
    Planning,
    Active,
    Released,
    Deprecated
}
```

## Collection Data Model

```csharp
public class FeatureCollection
{
    // Link to NPI Program
    public NpiProgram TargetProgram { get; set; }
    
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    
    // Firmware version requirements
    public FirmwareVersion RequiredLpiVersion { get; set; }
    public FirmwareVersion RequiredPiccoloVersion { get; set; }
    
    // Features in this collection
    public List<Feature> Features { get; set; }
    
    // Metadata
    public CollectionVisibility Visibility { get; set; }
    public string Owner { get; set; }
    public List<string> SharedWith { get; set; } // User IDs or team names
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    
    // Version locking
    public bool IsLocked { get; set; }
    public string LockedReason { get; set; }
    
    // Tags for organization
    public List<string> Tags { get; set; }
}

public enum CollectionVisibility
{
    Private,      // Only owner can see
    Team,         // Shared with specific users/teams
    Organization, // All users in organization
    Public        // Everyone (for released collections)
}

public class FirmwareVersion
{
    public Guid Id { get; set; }
    public NpiProgram NpiProgram { get; set; } // e.g., Eagan program
    public string Version { get; set; } // Semantic version (e.g., "3.2.1")
    public FirmwareReleaseStage Stage { get; set; }
    public string FilePath { get; set; } // Path to XML file
    public DateTime ReleasedAt { get; set; }
    public string ReleasedBy { get; set; }
    public List<string> ChangeLog { get; set; }
    
    // Parsed metadata
    public List<ParameterDefinition> Parameters { get; set; } // For LPI
    public List<CommandDefinition> Commands { get; set; }     // For PICCOLO
}

public enum FirmwareReleaseStage
{
    T0_Test,        // Experimental
    T1_Alpha,       // Early testing
    T2_Beta,        // Pre-release
    T3_Released     // Locked and production-ready
}

public class ParameterDefinition
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string DataType { get; set; }
    public int MinValue { get; set; }
    public int MaxValue { get; set; }
    public int DefaultValue { get; set; }
    public string Description { get; set; }
    public bool IsDeprecated { get; set; }
    public string DeprecationMessage { get; set; }
}

public class CommandDefinition
{
    public byte CommandCode { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<ParameterDefinition> Parameters { get; set; }
    public byte ExpectedResponseCode { get; set; }
    public bool IsDeprecated { get; set; }
    public string DeprecationMessage { get; set; }
}
```

## User Preferences for Version Visibility

Users can configure which firmware versions they want to see:

```csharp
public class UserVersionPreferences
{
    public Guid UserId { get; set; }
    
    // Which stages to show
    public bool ShowT0Builds { get; set; }      // Default: false
    public bool ShowT1Builds { get; set; }      // Default: false
    public bool ShowT2Builds { get; set; }      // Default: true (for beta testers)
    public bool ShowT3Builds { get; set; }      // Default: true (released)
    
    // Show builds from peers
    public bool ShowPeerBuilds { get; set; }    // Default: false
    public List<string> PeerUserIds { get; set; } // Specific peers to watch
    
    // Default versions
    public FirmwareVersion DefaultLpiVersion { get; set; }
    public FirmwareVersion DefaultPiccoloVersion { get; set; }
    
    // Compatibility warnings
    public bool ShowCompatibilityWarnings { get; set; } // Default: true
    public bool BlockIncompatibleVersions { get; set; } // Default: false
}
```

## Version Compatibility Validation

```csharp
public class VersionCompatibilityValidator
{
    public CompatibilityResult ValidateCollectionCompatibility(
        FeatureCollection collection,
        FirmwareVersion currentLpi,
        FirmwareVersion currentPiccolo)
    {
        var result = new CompatibilityResult();
        
        // Check LPI compatibility
        result.LpiCompatibility = CheckVersionCompatibility(
            collection.RequiredLpiVersion,
            currentLpi
        );
        
        // Check PICCOLO compatibility
        result.PiccoloCompatibility = CheckVersionCompatibility(
            collection.RequiredPiccoloVersion,
            currentPiccolo
        );
        
        return result;
    }
    
    private VersionCompatibility CheckVersionCompatibility(
        FirmwareVersion required,
        FirmwareVersion current)
    {
        var requiredSemVer = ParseSemVer(required.Version);
        var currentSemVer = ParseSemVer(current.Version);
        
        // Major version mismatch = incompatible
        if (requiredSemVer.Major != currentSemVer.Major)
        {
            return VersionCompatibility.Incompatible;
        }
        
        // Minor/Patch difference = compatible with warnings
        if (requiredSemVer.Minor != currentSemVer.Minor ||
            requiredSemVer.Patch != currentSemVer.Patch)
        {
            return VersionCompatibility.CompatibleWithWarnings;
        }
        
        // Exact match = fully compatible
        return VersionCompatibility.FullyCompatible;
    }
}

public enum VersionCompatibility
{
    FullyCompatible,        // Exact version match
    CompatibleWithWarnings, // Minor/Patch difference
    Incompatible            // Major version mismatch
}

public class CompatibilityResult
{
    public VersionCompatibility LpiCompatibility { get; set; }
    public VersionCompatibility PiccoloCompatibility { get; set; }
    public List<string> Warnings { get; set; }
    public List<string> Errors { get; set; }
    
    public bool IsCompatible => 
        LpiCompatibility != VersionCompatibility.Incompatible &&
        PiccoloCompatibility != VersionCompatibility.Incompatible;
}
```

## Relationships

```
User (1) ── (1) UserVersionPreferences

FeatureCollection (1) ──┬── (1) FirmwareVersion (LPI)
                        └── (1) FirmwareVersion (PICCOLO)

FeatureCollection (1) ── (0..n) Feature

FirmwareVersion (1) ──┬── (0..n) ParameterDefinition (for LPI)
                      └── (0..n) CommandDefinition (for PICCOLO)

Feature.Parameter (n) ── (1) ParameterDefinition (reference)
Feature.Command (n) ── (1) CommandDefinition (reference)
```

## Use Cases

### UC1: Create Collection for Specific Firmware Version
User creates a new collection targeting LPI 3.2.1 and PICCOLO 2.5.0, adds features that use parameters/commands from those versions.

### UC2: View Test Builds (T0)
Developer enables "Show T0 Builds" to see experimental firmware versions for testing new features.

### UC3: Share Collection with Peers
User shares their collection with specific team members who are working on the same firmware version.

### UC4: Upgrade Collection to New Firmware Version
User migrates a collection from LPI 3.1.0 to 3.2.0, system validates compatibility and flags any breaking changes.

### UC5: Lock Collection for Release
After testing, user locks collection to T3 firmware versions only, preventing accidental changes.

## Compatibility Scenarios

### Scenario 1: Exact Match (Ideal)
```
Collection requires: LPI 3.2.1, PICCOLO 2.5.0
Current device: LPI 3.2.1, PICCOLO 2.5.0
Result: ✓ Fully compatible, no warnings
```

### Scenario 2: Minor Version Difference
```
Collection requires: LPI 3.2.0, PICCOLO 2.5.0
Current device: LPI 3.2.1, PICCOLO 2.5.0
Result: ⚠️ Compatible with warnings (patch difference in LPI)
Warning: "Collection built for LPI 3.2.0, device has 3.2.1. Minor differences may exist."
```

### Scenario 3: Major Version Mismatch
```
Collection requires: LPI 3.2.1, PICCOLO 2.5.0
Current device: LPI 4.0.0, PICCOLO 2.5.0
Result: ❌ Incompatible (major version change in LPI)
Error: "Collection requires LPI 3.x, device has 4.x. Breaking changes prevent execution."
```

### Scenario 4: Using Deprecated Parameters
```
Collection uses: PARAM_OLD_GAIN (deprecated in LPI 3.3.0)
Current device: LPI 3.3.0
Result: ⚠️ Compatible with warnings
Warning: "PARAM_OLD_GAIN is deprecated. Use PARAM_NEW_GAIN instead."
```

---
_Specification auto-filled by agent. User should review collection model, versioning rules, and compatibility logic._
