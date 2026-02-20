using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Services;

/// <summary>
/// Extension methods for cross-collection operations and compatibility management.
/// Supports multi-collection workflows: pulling features from parallel efforts,
/// adapting incompatible features, and merging features from different sources.
/// </summary>
public partial class FirmwareDataService
{
    /// <summary>
    /// Copy a feature from one collection to another, optionally adapting it to the target collection's firmware version.
    /// </summary>
    public async Task<Feature> CopyFeatureToCollection(Guid sourceFeatureId, Guid targetCollectionId, bool adaptToTargetVersion = false)
    {
        var sourceFeature = await GetFeatureByIdAsync(sourceFeatureId);
        var targetCollection = await GetCollectionByIdAsync(targetCollectionId);

        if (sourceFeature == null || targetCollection == null)
            throw new ArgumentException("Source feature or target collection not found");

        // Clone the feature
        var clonedFeature = new Feature
        {
            Name = $"{sourceFeature.Name} (Copy)",
            Description = sourceFeature.Description,
            Category = sourceFeature.Category,
            IsComposite = sourceFeature.IsComposite,
            Parameters = sourceFeature.Parameters.Select(p => new ParameterValue
            {
                ParameterId = p.ParameterId,
                Context = p.Context,
                SettingNumber = p.SettingNumber,
                Value = p.Value,
                ValueDecimal = p.ValueDecimal,
                ValueHex = p.ValueHex
            }).ToList(),
            Commands = sourceFeature.Commands.Select(c => new PiccoloCommand
            {
                CommandId = c.CommandId,
                Sequence = c.Sequence,
                ServiceName = c.ServiceName,
                CommandName = c.CommandName,
                Payload = c.Payload
            }).ToList(),
            RequiredLpiParams = sourceFeature.RequiredLpiParams.ToList(),
            RequiredPiccoloCommands = sourceFeature.RequiredPiccoloCommands.ToList(),
            Owner = sourceFeature.Owner,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        };

        // Adapt to target version if requested
        if (adaptToTargetVersion)
        {
            clonedFeature = await AdaptFeatureToVersion(
                clonedFeature,
                targetCollection.RequiredLpiVersionId,
                targetCollection.RequiredPiccoloVersionId
            );
        }

        // Add to target collection
        targetCollection.Features.Add(clonedFeature);
        await _context.SaveChangesAsync();

        return clonedFeature;
    }

    /// <summary>
    /// Clone a feature (create duplicate within same collection or standalone).
    /// </summary>
    public async Task<Feature> CloneFeature(Guid featureId, string newName)
    {
        var sourceFeature = await GetFeatureByIdAsync(featureId);
        if (sourceFeature == null)
            throw new ArgumentException("Source feature not found");

        var clonedFeature = new Feature
        {
            Name = newName,
            Description = $"Cloned from {sourceFeature.Name}",
            Category = sourceFeature.Category,
            IsComposite = sourceFeature.IsComposite,
            Parameters = sourceFeature.Parameters.Select(p => new ParameterValue
            {
                ParameterId = p.ParameterId,
                Context = p.Context,
                SettingNumber = p.SettingNumber,
                Value = p.Value,
                ValueDecimal = p.ValueDecimal,
                ValueHex = p.ValueHex
            }).ToList(),
            Commands = sourceFeature.Commands.Select(c => new PiccoloCommand
            {
                CommandId = c.CommandId,
                Sequence = c.Sequence,
                ServiceName = c.ServiceName,
                CommandName = c.CommandName,
                Payload = c.Payload
            }).ToList(),
            RequiredLpiParams = sourceFeature.RequiredLpiParams.ToList(),
            RequiredPiccoloCommands = sourceFeature.RequiredPiccoloCommands.ToList(),
            Owner = sourceFeature.Owner,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        };

        await _context.Features.AddAsync(clonedFeature);
        await _context.SaveChangesAsync();

        return clonedFeature;
    }

    /// <summary>
    /// Adapt a feature to work with a different firmware version (parameter and command mapping).
    /// </summary>
    public async Task<Feature> AdaptFeatureToVersion(Feature feature, Guid targetLpiVersionId, Guid targetPiccoloVersionId)
    {
        var targetLpiVersion = await GetFirmwareVersionByIdAsync(targetLpiVersionId);
        var targetPiccoloVersion = await GetFirmwareVersionByIdAsync(targetPiccoloVersionId);

        if (targetLpiVersion == null || targetPiccoloVersion == null)
            throw new ArgumentException("Target firmware versions not found");

        // Get compatibility issues
        var issues = await GetCompatibilityIssues(feature, targetLpiVersionId, targetPiccoloVersionId);
        
        // Get suggested fixes
        var fixes = await SuggestCompatibilityFixes(feature, targetLpiVersionId, targetPiccoloVersionId);

        // Apply automatic fixes where possible
        foreach (var fix in fixes.Where(f => f.CanAutoApply))
        {
            if (fix.Type == CompatibilityFixType.ParameterMapping)
            {
                // Update parameter IDs
                var param = feature.Parameters.FirstOrDefault(p => p.ParameterId == fix.OldParameterId);
                if (param != null && fix.NewParameterId.HasValue)
                {
                    param.ParameterId = fix.NewParameterId.Value;
                }
            }
            else if (fix.Type == CompatibilityFixType.CommandSubstitution)
            {
                // Update command IDs
                var command = feature.Commands.FirstOrDefault(c => c.CommandId == fix.OldCommandId);
                if (command != null && fix.NewCommandId.HasValue)
                {
                    command.CommandId = fix.NewCommandId.Value;
                    command.CommandName = fix.NewCommandName ?? command.CommandName;
                }
            }
        }

        // Update required parameter/command lists
        feature.RequiredLpiParams = feature.Parameters.Select(p => p.ParameterId.ToString()).Distinct().ToList();
        feature.RequiredPiccoloCommands = feature.Commands.Select(c => c.CommandId.ToString()).Distinct().ToList();

        // Mark as modified
        feature.ModifiedAt = DateTime.UtcNow;
        feature.Description += $"\n\n[Adapted to LPI {targetLpiVersion.Version} + PICCOLO {targetPiccoloVersion.Version}]";

        return feature;
    }

    /// <summary>
    /// Get compatibility issues when moving a feature to a different firmware version.
    /// </summary>
    public async Task<List<CompatibilityIssue>> GetCompatibilityIssues(Feature feature, Guid lpiVersionId, Guid piccoloVersionId)
    {
        var issues = new List<CompatibilityIssue>();

        var targetLpiVersion = await GetFirmwareVersionByIdAsync(lpiVersionId);
        var targetPiccoloVersion = await GetFirmwareVersionByIdAsync(piccoloVersionId);

        if (targetLpiVersion == null || targetPiccoloVersion == null)
            return issues;

        // Check LPI parameter availability
        foreach (var param in feature.Parameters)
        {
            var paramDef = await _context.ParameterDefinitions.FindAsync(param.ParameterId);
            if (paramDef == null)
            {
                issues.Add(new CompatibilityIssue
                {
                    Severity = IssueSeverity.Error,
                    Type = IssueType.MissingParameter,
                    Message = $"Parameter ID {param.ParameterId} not found in system",
                    ParameterId = param.ParameterId
                });
                continue;
            }

            // Check if parameter exists in target LPI version
            if (!targetLpiVersion.Parameters.Any(p => p.Id == param.ParameterId))
            {
                issues.Add(new CompatibilityIssue
                {
                    Severity = IssueSeverity.Error,
                    Type = IssueType.MissingParameter,
                    Message = $"Parameter '{paramDef.Name}' not available in LPI {targetLpiVersion.Version}",
                    ParameterId = param.ParameterId,
                    ParameterName = paramDef.Name
                });
            }
        }

        // Check PICCOLO command availability
        foreach (var command in feature.Commands)
        {
            var commandDef = await _context.CommandDefinitions.FindAsync(command.CommandId);
            if (commandDef == null)
            {
                issues.Add(new CompatibilityIssue
                {
                    Severity = IssueSeverity.Error,
                    Type = IssueType.MissingCommand,
                    Message = $"Command ID {command.CommandId} not found in system",
                    CommandId = command.CommandId
                });
                continue;
            }

            // Check if command exists in target PICCOLO version
            if (!targetPiccoloVersion.Commands.Any(c => c.CommandId == command.CommandId))
            {
                issues.Add(new CompatibilityIssue
                {
                    Severity = IssueSeverity.Error,
                    Type = IssueType.MissingCommand,
                    Message = $"Command '{commandDef.Name}' not available in PICCOLO {targetPiccoloVersion.Version}",
                    CommandId = command.CommandId,
                    CommandName = commandDef.Name
                });
            }
        }

        return issues;
    }

    /// <summary>
    /// Suggest compatibility fixes for a feature being moved to a different firmware version.
    /// </summary>
    public async Task<List<CompatibilityFix>> SuggestCompatibilityFixes(Feature feature, Guid lpiVersionId, Guid piccoloVersionId)
    {
        var fixes = new List<CompatibilityFix>();

        var targetLpiVersion = await GetFirmwareVersionByIdAsync(lpiVersionId);
        var targetPiccoloVersion = await GetFirmwareVersionByIdAsync(piccoloVersionId);

        if (targetLpiVersion == null || targetPiccoloVersion == null)
            return fixes;

        // Find parameter mappings (old version parameter ID -> new version parameter ID)
        foreach (var param in feature.Parameters)
        {
            var paramDef = await _context.ParameterDefinitions.FindAsync(param.ParameterId);
            if (paramDef == null) continue;

            // Check if parameter exists in target version
            if (!targetLpiVersion.Parameters.Any(p => p.Id == param.ParameterId))
            {
                // Try to find equivalent parameter by name
                var equivalentParam = targetLpiVersion.Parameters
                    .FirstOrDefault(p => p.Name == paramDef.Name || p.Name.Contains(paramDef.Name.Split('.').Last()));

                if (equivalentParam != null)
                {
                    fixes.Add(new CompatibilityFix
                    {
                        Type = CompatibilityFixType.ParameterMapping,
                        Description = $"Map parameter '{paramDef.Name}' to equivalent '{equivalentParam.Name}'",
                        OldParameterId = param.ParameterId,
                        OldParameterName = paramDef.Name,
                        NewParameterId = equivalentParam.Id,
                        NewParameterName = equivalentParam.Name,
                        CanAutoApply = true,
                        Confidence = 0.8 // High confidence if names match
                    });
                }
                else
                {
                    fixes.Add(new CompatibilityFix
                    {
                        Type = CompatibilityFixType.ParameterMapping,
                        Description = $"Parameter '{paramDef.Name}' has no direct equivalent - manual mapping required",
                        OldParameterId = param.ParameterId,
                        OldParameterName = paramDef.Name,
                        CanAutoApply = false,
                        Confidence = 0.0
                    });
                }
            }
        }

        // Find command substitutions
        foreach (var command in feature.Commands)
        {
            var commandDef = await _context.CommandDefinitions.FindAsync(command.CommandId);
            if (commandDef == null) continue;

            // Check if command exists in target version
            if (!targetPiccoloVersion.Commands.Any(c => c.CommandId == command.CommandId))
            {
                // Try to find equivalent command by name
                var equivalentCommand = targetPiccoloVersion.Commands
                    .FirstOrDefault(c => c.Name == commandDef.Name || c.Name.Contains(commandDef.Name));

                if (equivalentCommand != null)
                {
                    fixes.Add(new CompatibilityFix
                    {
                        Type = CompatibilityFixType.CommandSubstitution,
                        Description = $"Substitute command '{commandDef.Name}' with '{equivalentCommand.Name}'",
                        OldCommandId = command.CommandId,
                        OldCommandName = commandDef.Name,
                        NewCommandId = equivalentCommand.CommandId,
                        NewCommandName = equivalentCommand.Name,
                        CanAutoApply = true,
                        Confidence = 0.8
                    });
                }
                else
                {
                    fixes.Add(new CompatibilityFix
                    {
                        Type = CompatibilityFixType.CommandSubstitution,
                        Description = $"Command '{commandDef.Name}' has no direct equivalent - manual substitution required",
                        OldCommandId = command.CommandId,
                        OldCommandName = commandDef.Name,
                        CanAutoApply = false,
                        Confidence = 0.0
                    });
                }
            }
        }

        return fixes;
    }

    /// <summary>
    /// Get features from multiple collections (cross-collection browsing).
    /// </summary>
    public async Task<List<Feature>> GetFeaturesFromMultipleCollections(List<Guid> collectionIds, string? searchTerm = null)
    {
        var collections = await _context.FeatureCollections
            .Where(c => collectionIds.Contains(c.Id))
            .Include(c => c.Features)
            .ToListAsync();

        var allFeatures = collections.SelectMany(c => c.Features).ToList();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            allFeatures = allFeatures.Where(f =>
                f.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                f.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                f.Category.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        return allFeatures;
    }

    /// <summary>
    /// Merge multiple features into a single composite feature.
    /// </summary>
    public async Task<Feature> MergeFeatures(List<Guid> featureIds, MergeStrategy strategy)
    {
        var sourceFeatures = new List<Feature>();
        foreach (var id in featureIds)
        {
            var feature = await GetFeatureByIdAsync(id);
            if (feature != null) sourceFeatures.Add(feature);
        }

        if (!sourceFeatures.Any())
            throw new ArgumentException("No valid source features found");

        var mergedFeature = new Feature
        {
            Name = $"Merged: {string.Join(" + ", sourceFeatures.Take(3).Select(f => f.Name))}",
            Description = $"Merged from {sourceFeatures.Count} features using {strategy} strategy",
            Category = sourceFeatures.First().Category,
            IsComposite = true,
            Parameters = new List<ParameterValue>(),
            Commands = new List<PiccoloCommand>(),
            RequiredLpiParams = new List<string>(),
            RequiredPiccoloCommands = new List<string>(),
            Owner = "System",
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        };

        // Merge parameters based on strategy
        if (strategy == MergeStrategy.Union)
        {
            // Include all parameters from all features (deduplicate by parameter ID + context)
            var allParams = sourceFeatures.SelectMany(f => f.Parameters).ToList();
            var uniqueParams = allParams
                .GroupBy(p => new { p.ParameterId, p.Context })
                .Select(g => g.First())
                .ToList();
            mergedFeature.Parameters = uniqueParams;
        }
        else if (strategy == MergeStrategy.Intersection)
        {
            // Include only parameters common to all features
            var firstFeatureParams = sourceFeatures.First().Parameters.Select(p => new { p.ParameterId, p.Context }).ToHashSet();
            foreach (var feature in sourceFeatures.Skip(1))
            {
                var currentParams = feature.Parameters.Select(p => new { p.ParameterId, p.Context }).ToHashSet();
                firstFeatureParams.IntersectWith(currentParams);
            }

            mergedFeature.Parameters = sourceFeatures.First().Parameters
                .Where(p => firstFeatureParams.Contains(new { p.ParameterId, p.Context }))
                .ToList();
        }
        else if (strategy == MergeStrategy.FirstWins)
        {
            // Use parameters from first feature only
            mergedFeature.Parameters = sourceFeatures.First().Parameters.ToList();
        }

        // Merge commands (combine all, preserve sequence order)
        int sequence = 0;
        foreach (var feature in sourceFeatures)
        {
            foreach (var command in feature.Commands.OrderBy(c => c.Sequence))
            {
                mergedFeature.Commands.Add(new PiccoloCommand
                {
                    CommandId = command.CommandId,
                    Sequence = sequence++,
                    ServiceName = command.ServiceName,
                    CommandName = command.CommandName,
                    Payload = command.Payload
                });
            }
        }

        // Update required lists
        mergedFeature.RequiredLpiParams = mergedFeature.Parameters.Select(p => p.ParameterId.ToString()).Distinct().ToList();
        mergedFeature.RequiredPiccoloCommands = mergedFeature.Commands.Select(c => c.CommandId.ToString()).Distinct().ToList();

        await _context.Features.AddAsync(mergedFeature);
        await _context.SaveChangesAsync();

        return mergedFeature;
    }

    /// <summary>
    /// Compare two features and return a structured diff.
    /// </summary>
    public async Task<FeatureDiff> CompareFeatures(Guid feature1Id, Guid feature2Id)
    {
        var feature1 = await GetFeatureByIdAsync(feature1Id);
        var feature2 = await GetFeatureByIdAsync(feature2Id);

        if (feature1 == null || feature2 == null)
            throw new ArgumentException("One or both features not found");

        var diff = new FeatureDiff
        {
            Feature1Name = feature1.Name,
            Feature2Name = feature2.Name,
            ParameterDifferences = new List<ParameterDifference>(),
            CommandDifferences = new List<CommandDifference>()
        };

        // Compare parameters
        var params1 = feature1.Parameters.ToDictionary(p => new { p.ParameterId, p.Context }, p => p);
        var params2 = feature2.Parameters.ToDictionary(p => new { p.ParameterId, p.Context }, p => p);

        // Parameters only in feature1
        foreach (var kvp in params1.Where(p => !params2.ContainsKey(p.Key)))
        {
            diff.ParameterDifferences.Add(new ParameterDifference
            {
                ParameterId = kvp.Value.ParameterId,
                Context = kvp.Value.Context,
                Status = DiffStatus.RemovedInFeature2,
                Value1 = kvp.Value.ValueDecimal,
                Value2 = null
            });
        }

        // Parameters only in feature2
        foreach (var kvp in params2.Where(p => !params1.ContainsKey(p.Key)))
        {
            diff.ParameterDifferences.Add(new ParameterDifference
            {
                ParameterId = kvp.Value.ParameterId,
                Context = kvp.Value.Context,
                Status = DiffStatus.AddedInFeature2,
                Value1 = null,
                Value2 = kvp.Value.ValueDecimal
            });
        }

        // Parameters in both (check values)
        foreach (var kvp in params1.Where(p => params2.ContainsKey(p.Key)))
        {
            var param2 = params2[kvp.Key];
            if (kvp.Value.ValueDecimal != param2.ValueDecimal)
            {
                diff.ParameterDifferences.Add(new ParameterDifference
                {
                    ParameterId = kvp.Value.ParameterId,
                    Context = kvp.Value.Context,
                    Status = DiffStatus.Modified,
                    Value1 = kvp.Value.ValueDecimal,
                    Value2 = param2.ValueDecimal
                });
            }
        }

        // Compare commands (simplified - by command ID only)
        var commands1Ids = feature1.Commands.Select(c => c.CommandId).ToHashSet();
        var commands2Ids = feature2.Commands.Select(c => c.CommandId).ToHashSet();

        foreach (var cmdId in commands1Ids.Except(commands2Ids))
        {
            var cmd = feature1.Commands.First(c => c.CommandId == cmdId);
            diff.CommandDifferences.Add(new CommandDifference
            {
                CommandId = cmdId,
                CommandName = cmd.CommandName,
                Status = DiffStatus.RemovedInFeature2
            });
        }

        foreach (var cmdId in commands2Ids.Except(commands1Ids))
        {
            var cmd = feature2.Commands.First(c => c.CommandId == cmdId);
            diff.CommandDifferences.Add(new CommandDifference
            {
                CommandId = cmdId,
                CommandName = cmd.CommandName,
                Status = DiffStatus.AddedInFeature2
            });
        }

        return diff;
    }
}

// Supporting models for cross-collection operations

public class CompatibilityIssue
{
    public IssueSeverity Severity { get; set; }
    public IssueType Type { get; set; }
    public string Message { get; set; } = string.Empty;
    public int? ParameterId { get; set; }
    public string? ParameterName { get; set; }
    public int? CommandId { get; set; }
    public string? CommandName { get; set; }
}

public enum IssueSeverity
{
    Warning,
    Error,
    Critical
}

public enum IssueType
{
    MissingParameter,
    MissingCommand,
    VersionMismatch,
    DeprecatedFeature
}

public class CompatibilityFix
{
    public CompatibilityFixType Type { get; set; }
    public string Description { get; set; } = string.Empty;
    public int? OldParameterId { get; set; }
    public string? OldParameterName { get; set; }
    public int? NewParameterId { get; set; }
    public string? NewParameterName { get; set; }
    public int? OldCommandId { get; set; }
    public string? OldCommandName { get; set; }
    public int? NewCommandId { get; set; }
    public string? NewCommandName { get; set; }
    public bool CanAutoApply { get; set; }
    public double Confidence { get; set; } // 0.0 to 1.0
}

public enum CompatibilityFixType
{
    ParameterMapping,
    CommandSubstitution,
    ValueConversion,
    StructuralChange
}

public enum MergeStrategy
{
    Union,        // Include all parameters from all features
    Intersection, // Include only common parameters
    FirstWins     // Use first feature as base
}

public class FeatureDiff
{
    public string Feature1Name { get; set; } = string.Empty;
    public string Feature2Name { get; set; } = string.Empty;
    public List<ParameterDifference> ParameterDifferences { get; set; } = new();
    public List<CommandDifference> CommandDifferences { get; set; } = new();
}

public class ParameterDifference
{
    public int ParameterId { get; set; }
    public string Context { get; set; } = string.Empty;
    public DiffStatus Status { get; set; }
    public decimal? Value1 { get; set; }
    public decimal? Value2 { get; set; }
}

public class CommandDifference
{
    public int CommandId { get; set; }
    public string CommandName { get; set; } = string.Empty;
    public DiffStatus Status { get; set; }
}

public enum DiffStatus
{
    AddedInFeature2,
    RemovedInFeature2,
    Modified
}


