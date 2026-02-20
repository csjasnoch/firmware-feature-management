namespace WebApp.Models;

/// <summary>
/// Represents the analysis of feature compatibility between different firmware versions
/// </summary>
public class CompatibilityAnalysis
{
    /// <summary>
    /// Source feature being analyzed
    /// </summary>
    public Feature SourceFeature { get; set; } = null!;

    /// <summary>
    /// Target firmware version
    /// </summary>
    public FirmwareVersion TargetVersion { get; set; } = null!;

    /// <summary>
    /// Whether the feature is compatible with the target version
    /// </summary>
    public bool IsCompatible { get; set; }

    /// <summary>
    /// Overall compatibility score (0.0 = incompatible, 1.0 = fully compatible)
    /// </summary>
    public double CompatibilityScore { get; set; }

    /// <summary>
    /// List of breaking changes that prevent direct use
    /// </summary>
    public List<BreakingChange> BreakingChanges { get; set; } = new();

    /// <summary>
    /// Suggested parameter mappings to adapt the feature
    /// </summary>
    public List<ParameterMapping> SuggestedMappings { get; set; } = new();

    /// <summary>
    /// Warnings about potential issues (non-blocking)
    /// </summary>
    public List<string> Warnings { get; set; } = new();

    /// <summary>
    /// Recommendations for successfully adapting the feature
    /// </summary>
    public List<string> Recommendations { get; set; } = new();

    /// <summary>
    /// Whether automatic adaptation is possible
    /// </summary>
    public bool CanAutoAdapt { get; set; }

    /// <summary>
    /// Estimated confidence in automatic adaptation (0.0 to 1.0)
    /// </summary>
    public double AdaptationConfidence { get; set; }
}

/// <summary>
/// Represents a breaking change that affects compatibility
/// </summary>
public class BreakingChange
{
    /// <summary>
    /// Type of breaking change
    /// </summary>
    public BreakingChangeType Type { get; set; }

    /// <summary>
    /// Description of the breaking change
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Affected element (parameter name, command name, etc.)
    /// </summary>
    public string AffectedElement { get; set; } = string.Empty;

    /// <summary>
    /// Severity of the breaking change
    /// </summary>
    public Severity Severity { get; set; }

    /// <summary>
    /// Suggested fix or workaround
    /// </summary>
    public string? SuggestedFix { get; set; }
}

/// <summary>
/// Types of breaking changes
/// </summary>
public enum BreakingChangeType
{
    ParameterIdChanged,
    ParameterRemoved,
    ParameterTypeChanged,
    CommandRemoved,
    CommandIdChanged,
    CommandSignatureChanged,
    DataTypeIncompatible
}

/// <summary>
/// Suggested mapping between source and target parameters
/// </summary>
public class ParameterMapping
{
    /// <summary>
    /// Source parameter definition
    /// </summary>
    public ParameterDefinition? SourceParameter { get; set; }

    /// <summary>
    /// Target parameter definition (mapped equivalent)
    /// </summary>
    public ParameterDefinition? TargetParameter { get; set; }

    /// <summary>
    /// Confidence in this mapping (0.0 to 1.0)
    /// </summary>
    public double Confidence { get; set; }

    /// <summary>
    /// Whether this mapping requires manual review
    /// </summary>
    public bool RequiresReview { get; set; }

    /// <summary>
    /// Notes about the mapping (e.g., why it was suggested)
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Whether value transformation is needed
    /// </summary>
    public bool RequiresValueTransformation { get; set; }

    /// <summary>
    /// Description of value transformation (if needed)
    /// </summary>
    public string? TransformationDescription { get; set; }
}

/// <summary>
/// Severity levels for compatibility issues
/// </summary>
public enum Severity
{
    Low,
    Medium,
    High,
    Critical
}
