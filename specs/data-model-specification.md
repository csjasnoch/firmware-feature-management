# Data Model Specification - Firmware Feature Management System

## Title
Core Data Structures for Features, Commands, Parameters, and Workflows

## Description
This specification defines the core data models and their relationships for the Firmware Feature Management System. These models support feature definition, hierarchical composition, byte packet generation, and workflow execution.

## Core Entities

### 1. Feature
Represents a logical firmware operation or configuration.

```csharp
public class Feature
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public FeatureType Type { get; set; } // Simple, Composite
    public bool IsAtomic { get; set; }
    public List<FeatureSetting> Settings { get; set; }
    public Workflow Workflow { get; set; }
    public List<Feature> SubFeatures { get; set; } // For composite features
    public Dictionary<string, object> Metadata { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    public string CreatedBy { get; set; }
}

public enum FeatureType
{
    Simple,      // Single-level feature with direct command mappings
    Composite    // Hierarchical feature composed of sub-features
}
```

### 2. FeatureSetting
Represents a specific configuration or mode within a feature (e.g., Directionality Setting 2).

```csharp
public class FeatureSetting
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int SettingNumber { get; set; }
    public List<ParameterValue> ParameterValues { get; set; }
    public string Description { get; set; }
}
```

### 3. Parameter (LPI)
Defines a firmware parameter that can be configured.

```csharp
public class Parameter
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public ParameterType Type { get; set; }
    public int MinValue { get; set; }
    public int MaxValue { get; set; }
    public int DefaultValue { get; set; }
    public int BytePosition { get; set; } // Position in the byte packet
    public List<Parameter> Dependencies { get; set; }
    public ValidationRule ValidationRule { get; set; }
}

public enum ParameterType
{
    Byte,
    Word,        // 2 bytes
    DWord,       // 4 bytes
    BitField     // Packed bits within a byte
}
```

### 4. ParameterValue
Associates a parameter with a specific value in a feature setting.

```csharp
public class ParameterValue
{
    public Guid Id { get; set; }
    public Parameter Parameter { get; set; }
    public int Value { get; set; }
    public string HexValue => $"0x{Value:X2}";
    public string Note { get; set; }
}
```

### 5. PiccoloCommand
Represents a firmware-level command sent to the device.

```csharp
public class PiccoloCommand
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public byte CommandCode { get; set; }
    public string Description { get; set; }
    public List<Parameter> Parameters { get; set; }
    public int ExpectedResponseCode { get; set; }
    public TimeSpan Timeout { get; set; }
}
```

### 6. Workflow
Defines the execution flow for a feature, including steps, loops, and conditionals.

```csharp
public class Workflow
{
    public Guid Id { get; set; }
    public List<WorkflowStep> Steps { get; set; }
    public ExecutionPolicy ExecutionPolicy { get; set; }
    public RollbackStrategy RollbackStrategy { get; set; }
}

public class WorkflowStep
{
    public Guid Id { get; set; }
    public int Order { get; set; }
    public WorkflowStepType Type { get; set; }
    public object StepData { get; set; } // CommandStep, LoopStep, ConditionalStep, etc.
}

public enum WorkflowStepType
{
    Command,
    Loop,
    Conditional,
    Delay,
    Calculation,
    SubFeature
}
```

### 7. CommandStep
Executes a PICCOLO command as part of a workflow.

```csharp
public class CommandStep
{
    public PiccoloCommand Command { get; set; }
    public List<ParameterValue> ParameterValues { get; set; }
    public bool Required { get; set; }
}
```

### 8. LoopStep
Repeats a set of steps based on a condition.

```csharp
public class LoopStep
{
    public int MaxIterations { get; set; }
    public TimeSpan DelayBetweenIterations { get; set; }
    public string Condition { get; set; } // Expression evaluated each iteration
    public List<WorkflowStep> Steps { get; set; }
    public LoopTimeoutBehavior TimeoutBehavior { get; set; }
}

public enum LoopTimeoutBehavior
{
    FailFeature,
    ContinueWorkflow,
    RetryFromBeginning
}
```

### 9. ConditionalStep
Executes different steps based on a condition.

```csharp
public class ConditionalStep
{
    public string Condition { get; set; } // Expression to evaluate
    public List<WorkflowStep> TrueSteps { get; set; }
    public List<WorkflowStep> FalseSteps { get; set; }
}
```

### 10. BytePacket
Represents the byte-level packet generated for a feature or command.

```csharp
public class BytePacket
{
    public Guid Id { get; set; }
    public Feature Feature { get; set; }
    public FeatureSetting Setting { get; set; }
    public byte[] Data { get; set; }
    public List<ByteMetadata> ByteMetadata { get; set; }
    public byte Checksum { get; set; }
    public DateTime GeneratedAt { get; set; }
}

public class ByteMetadata
{
    public int ByteIndex { get; set; }
    public byte Value { get; set; }
    public string HexValue => $"0x{Value:X2}";
    public Parameter Parameter { get; set; } // Null for header/checksum bytes
    public string Description { get; set; }
    public ByteRole Role { get; set; }
}

public enum ByteRole
{
    CommandHeader,
    SettingId,
    ParameterValue,
    Checksum,
    Reserved
}
```

### 11. ExecutionPolicy
Defines how a feature should be executed.

```csharp
public class ExecutionPolicy
{
    public bool StopOnFailure { get; set; }
    public bool RollbackOnFailure { get; set; }
    public bool AllowParallelExecution { get; set; }
    public int MaxRetries { get; set; }
    public TimeSpan RetryDelay { get; set; }
}
```

### 12. RollbackStrategy
Defines how to undo a feature execution.

```csharp
public class RollbackStrategy
{
    public RollbackOrder Order { get; set; }
    public List<RollbackStep> Steps { get; set; }
}

public enum RollbackOrder
{
    Forward,   // Same order as execution
    Reverse    // Reverse order (most common)
}

public class RollbackStep
{
    public PiccoloCommand Command { get; set; }
    public List<ParameterValue> ParameterValues { get; set; }
}
```

## Relationships

```
Feature (1) ──┬── (1..n) FeatureSetting
              │
              ├── (1) Workflow
              │
              └── (0..n) Feature (SubFeatures, for composite)

FeatureSetting (1) ── (1..n) ParameterValue

ParameterValue (n) ── (1) Parameter

Workflow (1) ── (1..n) WorkflowStep

WorkflowStep (1) ──┬── (0..1) CommandStep
                   ├── (0..1) LoopStep
                   ├── (0..1) ConditionalStep
                   └── (0..1) other step types

CommandStep (n) ── (1) PiccoloCommand

PiccoloCommand (1) ── (0..n) Parameter

BytePacket (1) ──┬── (1) Feature
                 ├── (0..1) FeatureSetting
                 └── (1..n) ByteMetadata

ByteMetadata (n) ── (0..1) Parameter
```

## Example: Directionality Feature Instance

```csharp
var directionalityFeature = new Feature
{
    Id = Guid.NewGuid(),
    Name = "Directionality",
    Description = "Controls directional hearing settings",
    Type = FeatureType.Simple,
    IsAtomic = true,
    Settings = new List<FeatureSetting>
    {
        new FeatureSetting
        {
            Name = "Setting 2",
            SettingNumber = 2,
            ParameterValues = new List<ParameterValue>
            {
                new ParameterValue
                {
                    Parameter = new Parameter { Name = "PARAM_DIR_FRONT_GAIN", BytePosition = 2 },
                    Value = 0x40
                },
                new ParameterValue
                {
                    Parameter = new Parameter { Name = "PARAM_DIR_REAR_GAIN", BytePosition = 3 },
                    Value = 0x2C
                },
                // ... 22 more parameter values
            }
        },
        // ... 3 more settings
    },
    Workflow = new Workflow
    {
        Steps = new List<WorkflowStep>
        {
            new WorkflowStep
            {
                Order = 1,
                Type = WorkflowStepType.Command,
                StepData = new CommandStep
                {
                    Command = new PiccoloCommand
                    {
                        Name = "SET_DIRECTIONALITY",
                        CommandCode = 0x5A
                    }
                }
            }
        },
        ExecutionPolicy = new ExecutionPolicy
        {
            StopOnFailure = true,
            RollbackOnFailure = true
        }
    }
};
```

## Notes
- This data model supports all use cases defined in the specification
- Extensible design allows for future feature types and workflow step types
- Clear separation between feature definition and feature execution
- Byte packet generation can be deterministic based on feature settings

---
_Auto-filled by agent. User should review entity properties, relationships, and extensibility points._
