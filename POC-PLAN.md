## Plan: Firmware Feature Management POC Delivery (Updated Feb 2026)

Build a functional proof-of-concept that enables engineers to manage PICCOLO-based firmware features across NPI programs and versions. Focus is on UI/UX for navigating pre-parsed firmware data structures, building features with realistic LPI parameter groups and PICCOLO command sequences, and demonstrating version-aware workflows—ready to demo to senior leadership.

### Updated Context: Firmware Data Integration

The POC now uses **realistic mock data** based on actual PiccoloInterface.xml and PiccoloProtocol.xml structures from Example Firmware Data/ folder:

- **Real LPI Parameter Groups**: DirectionalityPerMemory, DirectionalityControls, DirectionalityMonitor, DirectionalitySpatialAwareness, NoiseReduction, BinauralNoiseReduction, TinnitusTherapyPerMemory, FeedbackCanceller, Ocl, StreamingControls
- **Real LPI Contexts**: Memory1-5 (each contains ~19 parameter groups), MemoryGlobal, DataLogMemory1-5, BLE contexts (BleBond0-14, BleConfiguration, BleSystem), Indicator contexts
- **Real PICCOLO Commands**: LPI service (LpiContextRead/Write, LpiParameterGroupRead/Write), AFS service, DFU service (DfuGetStatus, DfuRequest, DfuWriteParcel), BLE service

**20 Comprehensive Features with PICCOLO/LPI Mappings** (Based on PiccoloInterface.xml analysis):
1. **Adaptive Directionality** - 4 settings (Omni, Fixed, Adaptive, Full), 45 parameters → DirectionalityPerMemory/Controls/Monitor/SpatialAwareness groups
2. **Advanced Noise Reduction** - 6-channel spectral + binaural → NoiseReduction + BinauralNoiseReduction groups  
3. **Tinnitus Therapy** - Sound generator → TinnitusTherapyPerMemory group
4. **Device Firmware Update (DFU)** - 15-command PICCOLO DFU service workflow with parcel-based download
5. **Adaptive Feedback Cancellation** - Acoustic feedback suppression → FeedbackCanceller group
6. **Bluetooth Audio Streaming** - Wireless streaming with codec preference → StreamingControls group
7. **Output Compression Limiter (OCL)** - Multi-channel compression → Ocl group
8. **Frequency Translation** - Frequency lowering for high-frequency hearing loss → FrequencyTranslation group
9. **Advanced Environment Classification** - AI-powered 8-environment detection (quiet, speech, music, restaurant, outdoor, wind, car, large room) → AdvancedEnvironmentClassification + AdaptiveTuningPerAcousticEnvironment groups
10. **Own Voice Detection** - Specialized processing for user's own voice → OwnVoiceDetection group
11. **Wind Noise Adaptation** - Outdoor wind detection and suppression → WindNoiseAdaptation group
12. **Multi-Channel Equalizer** - 8-band parametric EQ (250Hz-8kHz) → EqualizerSlider group
13. **Motion-Based Audio Adaptation** - IMU-based adjustment for walking/running/stationary → MotionDetectionControls + ImuFeatures groups
14. **Telecoil (T-Coil) System** - Hearing loop and telephone compatibility → Telecoil group
15. **Google Fast Pair** - Seamless Android device pairing → BleGoogleFastPairConfiguration group
16. **Auto-Phone Detection & Alerts** - Proximity alerts when phone left behind → PhoneLeftBehind group
17. **Sleep & Health Monitoring** - Sleep stages, respiratory rate, activity tracking → SleepMonitorControl + SleepMonitorStage + RespiratoryRateControls groups
18. **Multi-Memory Program Management** - 5 customizable memory programs → Memory1-5 contexts + UiControls group
19. **Auracast Broadcast Audio** - Bluetooth LE Audio public venue broadcasting → AuracastTransmitterInfo + BroadcastDeviceList groups
20. **DNN Speech Enhancement** - AI/ML-powered speech separation → DnnSpeechEnhancement group

**Note**: Data parsing and firmware version management is handled independently. This POC demonstrates UI/UX for working with pre-parsed firmware structures.

### Steps

✅ **Step 1: Enhanced mock data layer with realistic PICCOLO features (COMPLETED)** — Expanded [FirmwareDataService.Enhanced.cs](Services/FirmwareDataService.Enhanced.cs) from initial 7 features to **20 comprehensive features** based on extensive PiccoloInterface.xml and PiccoloProtocol.xml analysis. New features include: Frequency Translation (frequency lowering), Advanced Environment Classification (8 acoustic environments), Own Voice Detection, Wind Noise Adaptation, Multi-Channel Equalizer (8-band), Motion-Based Audio Adaptation (IMU), Telecoil System, Google Fast Pair, Auto-Phone Detection, Sleep & Health Monitoring, Multi-Memory Program Management (5 memories), Auracast Broadcast Audio, and DNN Speech Enhancement. All features map to actual LPI parameter groups and PICCOLO commands extracted from the 100,000+ line XML firmware specifications.

✅ **Step 2: Build version management dashboard and firmware context switcher (COMPLETED)** — Implemented [Pages/Index.cshtml](Pages/Index.cshtml) showing firmware version matrices per NPI program, global context switcher (NPI Program + LPI Version + PICCOLO Version), compatibility heatmaps showing which features work with which versions (✓ Compatible, ⚠️ Partial, 🧪 Experimental, ❌ Incompatible), and quick stats cards displaying total features, collections, NPI programs, and firmware versions per [ui-mockup-dashboard.md](specs/ui-mockup-dashboard.md)

✅ **Step 3: Complete collection manager with compatibility indicators and wizard (COMPLETED)** — Enhanced [Pages/Collections/Index.cshtml](Pages/Collections/Index.cshtml) and [Create.cshtml](Pages/Collections/Create.cshtml) with compatibility badges (✓/⚠️/❌/🧪), 4-step wizard (basic info → version selection with T0/T1/T2/T3 filters → feature selection filtered by compatibility → sharing configuration), and version-aware feature filtering per [firmware-version-collection-management.md](specs/firmware-version-collection-management.md) and [use-case-5-collection-management.md](specs/use-case-5-collection-management.md)

✅ **Step 4: Build feature parameter editor with Settings tabs and validation (COMPLETED)** — Implemented [Pages/Features/Edit.cshtml](Pages/Features/Edit.cshtml) with Settings 1-4 tabs for multi-setting features (like Directionality), grouped parameter cards with expand/collapse (Front Processing, Rear Processing, Spatial Awareness groups), dual decimal/hex inputs with auto-sync, range validation with visual feedback, and parameter dependency highlighting per [ui-mockup-feature-editor.md](specs/ui-mockup-feature-editor.md) and [use-case-1-adjust-directionality.md](specs/use-case-1-adjust-directionality.md)

✅ **Step 5: Create byte packet viewer with parameter correlation (COMPLETED)** — Built [Pages/Features/BytePacket.cshtml](Pages/Features/BytePacket.cshtml) with hex display component with 4/8/16 byte grouping options, expandable byte cards showing parameter correlation (Byte 5 = DirectionalityPerMemory.FrontGain), feature → parameter → byte correlation tree visualization, side-by-side comparison view (Setting 1 vs Setting 2), and export options (C array, JSON, CSV, binary) per [ui-mockup-byte-packet-viewer.md](specs/ui-mockup-byte-packet-viewer.md) and [use-case-4-inspect-byte-packets.md](specs/use-case-4-inspect-byte-packets.md)

✅ **Step 6: Implement workflow designer with collapsible operations (COMPLETED)** — Built [Pages/Workflow/Designer.cshtml](Pages/Workflow/Designer.cshtml) with drag-and-drop workflow canvas featuring collapsible operation/step panels, step type picker (Command, Loop, Conditional, Delay, Calculation), loop configuration dialog with condition builder, decision point dropdowns (Continue, Retry, Abort, Jump), and visual execution flow preview with timing estimates per [ui-mockup-workflow-designer.md](specs/ui-mockup-workflow-designer.md) and [use-case-2-create-feature-with-loop.md](specs/use-case-2-create-feature-with-loop.md)

7. **Add composite feature builder wizard** — Create 4-step wizard (select sub-features → drag-and-drop ordering → configure execution policy (atomic/sequential/parallel) → review aggregated metrics), dual-pane layout (Available vs Selected), dependency graph visualization, rollback strategy configuration, and conditional logic between sub-features per [ui-mockup-composite-feature-builder.md](specs/ui-mockup-composite-feature-builder.md) and [use-case-3-composite-feature.md](specs/use-case-3-composite-feature.md)

8. **Polish demo experience and authentication** — Add ASP.NET Core Identity for user-scoped collections, refine UI/UX consistency, improve validation feedback, and create demo walkthrough scenarios covering all 5 use cases

---

## CRITICAL: Collection Manager & Landing Page Redesign (Feb 13, 2026)

### Problem Statement

The current collection manager and landing page fail to support real-world multi-program feature engineering workflows. Feature engineers frequently need to:

- **Pull features from parallel efforts** (e.g., Edina development + Fridley testing + TMI tech maturity)
- **Review multiple collections simultaneously** while maintaining context in their active collection
- **Make incompatible features compatible** by adapting parameters/commands from one firmware version to another
- **Combine features from different sources** (operational flow from Edina + enhancements from Fridley)

Current system restricts users to single-context, single-collection work with no cross-collection feature pulling or compatibility adaptation tools.

### Missing Functionality (Gap Analysis)

#### Landing Page (Pages/Index.cshtml) - Current Issues:
- ❌ **Duplicate context displays** - Global firmware context switcher AND hardcoded "Current Device Context" panel showing same information
- ❌ **Unnecessary "fluff"** - Quick start guide takes valuable screen space
- ❌ **Limited compatibility heatmap** - Shows only 10 features with no actionability
- ❌ **Simplistic stats cards** - Just counts (total features, total collections) with no insights
- ❌ **No collection workspace view** - Can't see which collections user is actively working on
- ❌ **No recent collections quick access** - Must navigate to Collections page every time
- ❌ **No cross-collection feature reuse insights** - Can't see which features appear in multiple collections
- ❌ **No version distribution visualization** - No charts showing collection distribution across firmware versions

**Expected Landing Page Functionality:**
1. **Active Collections Workspace Panel** - Show 3-5 collections currently being worked on with quick access links
2. **Recent Collections List** - Last 5 accessed collections with "Open" and "Compare" actions
3. **Cross-Collection Feature Insights** - Stats showing which features are reused across collections (e.g., "Adaptive Directionality appears in 8 collections")
4. **Version Distribution Charts** - Visual breakdown of how many collections target each firmware version (Edina 3.2.1 = 5 collections, Fridley 2.5.0 = 3 collections)
5. **Consolidated Context Switcher** - Single, clean context selector (remove duplicate panels)
6. **Collection Health Indicators** - Show which collections have compatibility warnings or outdated features

#### Collection Manager (Pages/Collections/Index.cshtml, Details.cshtml) - Current Issues:
- ❌ **Single-context restriction** - Can only view collections compatible with current firmware context
- ❌ **No multi-collection workspace** - Can't view/compare more than 2 collections simultaneously
- ❌ **No cross-collection feature browsing** - Can't explore features from other collections while working in Details view
- ❌ **No "pull feature" action** - Can't grab a feature from Collection A and add to Collection B
- ❌ **No incompatibility handling** - System filters out incompatible features instead of helping adapt them
- ❌ **No side-by-side collection panels** - Can't keep one collection open while browsing others
- ❌ **No batch operations** - Can't select multiple features from different collections for merging

**Expected Collection Manager Functionality:**
1. **Multi-Collection Workspace** - Side-by-side panels showing 2-3 collections simultaneously (active collection + reference collections)
2. **Cross-Collection Feature Browser** - Modal/panel showing features from ALL collections (not filtered by context) with search and filter
3. **"Pull Feature" Action** - Button on each feature in browser to copy/add to active collection
4. **Compatibility Status Display** - When viewing feature from incompatible version, show clear compatibility issues (e.g., "Uses LPI parameter DirectionalityPerMemory.FrontGain_v2 not available in Edina 3.2.1")
5. **"Make Compatible" Wizard** - When pulling incompatible feature, launch wizard to help adapt it (parameter ID mapping, command substitution)
6. **Collection Context Tabs** - Quick-switch between multiple open collections without losing place
7. **Feature Comparison Grid** - When selecting features from multiple collections, show side-by-side parameter comparison

#### Comparison Page (Pages/Collections/Compare.cshtml) - Current Issues:
- ❌ **Limited to 2 collections** - Real workflows need to compare 3+ collections (current program + future release + tech maturity)
- ❌ **Read-only comparison** - No actions available (can't pull features, can't merge)
- ❌ **No cross-program comparison** - Dropdowns filtered by current context, can't compare Edina vs Fridley
- ❌ **No feature merging** - Can't select operational flow from Collection A + parameters from Collection B
- ❌ **No conflict resolution** - When features differ, no tooling to help choose which version to use or merge

**Expected Comparison Page Functionality:**
1. **Multi-Collection Comparison** - Support comparing 3-5 collections simultaneously (not just 2)
2. **Cross-Program Comparison** - Allow selecting collections from different NPI programs (Edina + Fridley + Chaska)
3. **Feature Merge Wizard** - Select features from multiple collections and merge into new combined feature
4. **Operational Flow Selection** - When comparing same feature across collections, pick which operational flow to use
5. **Parameter Cherry-Picking** - Select specific parameters from Collection A, others from Collection B
6. **Conflict Resolution Tools** - When parameter values differ, show side-by-side with recommendation and manual override
7. **Batch Pull Actions** - Checkboxes to select multiple features and pull all into active collection

#### Services (Services/FirmwareDataService.cs) - Missing Methods:
- ❌ **CopyFeatureToCollection()** - No method to copy feature from Collection A to Collection B
- ❌ **CloneFeature()** - No method to duplicate feature within or across collections
- ❌ **AdaptFeatureToVersion()** - No method to adapt feature from one firmware version to another (parameter ID mapping)
- ❌ **GetCompatibilityIssues()** - No method to analyze what breaks when moving feature to different version
- ❌ **SuggestCompatibilityFixes()** - No method to recommend parameter/command substitutions for compatibility
- ❌ **GetFeaturesFromMultipleCollections()** - No method to query features across collection boundaries
- ❌ **MergeFeatures()** - No method to combine parameters/commands from multiple feature sources
- ❌ **CompareFeatures()** - No method to return structured diff of two features

**Expected Service Methods:**
1. `Task<Feature> CopyFeatureToCollection(int sourceFeatureId, int targetCollectionId, bool adaptToTargetVersion = false)`
2. `Task<Feature> CloneFeature(int featureId, string newName)`
3. `Task<Feature> AdaptFeatureToVersion(Feature feature, int targetLpiVersionId, int targetPiccoloVersionId)`
4. `Task<List<CompatibilityIssue>> GetCompatibilityIssues(Feature feature, int lpiVersionId, int piccoloVersionId)`
5. `Task<List<CompatibilityFix>> SuggestCompatibilityFixes(Feature feature, int lpiVersionId, int piccoloVersionId)`
6. `Task<List<Feature>> GetFeaturesFromMultipleCollections(List<int> collectionIds, string searchTerm = null)`
7. `Task<Feature> MergeFeatures(List<int> featureIds, MergeStrategy strategy)`
8. `Task<FeatureDiff> CompareFeatures(int feature1Id, int feature2Id)`

#### Context Management System - Current Issues:
- ❌ **Ad-hoc sessionStorage handling** - Each page manually reads/writes context to sessionStorage
- ❌ **No persistence** - Context resets on page refresh
- ❌ **No centralized service** - Context logic duplicated across pages
- ❌ **Incomplete propagation** - Global context doesn't flow to all pages
- ❌ **No multi-context support** - Can only work in one firmware version context at a time
- ❌ **No saved context profiles** - Can't save "Edina T3 Setup" or "Fridley Testing Profile" for quick switching
- ❌ **No context history** - Can't see previous contexts or quickly switch back

**Expected Context Management Functionality:**
1. **ContextService class** - Centralized service for context management (Services/ContextService.cs)
2. **Persistent context storage** - Save to database or local storage (not just sessionStorage)
3. **Context profiles** - Save named profiles (e.g., "Edina 3.2.1 T2", "Fridley 2.5.0 T3")
4. **Multi-context workspaces** - Support working in multiple contexts simultaneously (Edina in left panel, Fridley in right panel)
5. **Context history** - Track last 5 contexts and provide quick-switch dropdown
6. **Context propagation** - Automatically propagate context changes to all relevant pages via service
7. **Context validation** - Verify selected NPI program + versions are valid combinations

### Implementation Priority

**Phase 1: Foundation (Week 1)**
1. Create Services/ContextService.cs for centralized context management
2. Add missing service methods to FirmwareDataService (CopyFeatureToCollection, GetCompatibilityIssues, etc.)
3. Create shared models: CompatibilityIssue, CompatibilityFix, FeatureDiff, MergeStrategy

**Phase 2: Landing Page Redesign (Week 1-2)**
1. Remove fluff: Delete quick start guide, remove duplicate context panels
2. Add Active Collections Workspace panel (show user's 3-5 in-progress collections)
3. Add Recent Collections quick access list
4. Add Cross-Collection Feature Insights (feature reuse stats)
5. Add Version Distribution charts (collections per firmware version)
6. Consolidate context switcher (single clean UI)

**Phase 3: Multi-Collection Workspace (Week 2-3)**
1. Redesign Pages/Collections/Index.cshtml for side-by-side collection panels
2. Add Cross-Collection Feature Browser modal (show all features, not filtered by context)
3. Add "Pull Feature" action buttons
4. Implement "Make Compatible" wizard (parameter mapping UI)
5. Add collection context tabs for quick-switching

**Phase 4: Enhanced Comparison & Merging (Week 3-4)**
1. Extend Pages/Collections/Compare.cshtml to support 3+ collections
2. Add cross-program comparison (remove context filtering)
3. Build Feature Merge Wizard (select features from multiple sources)
4. Add conflict resolution UI (side-by-side parameter comparison)
5. Add batch pull actions (multi-select features)

### Design Decisions Needed

1. **Multi-collection workspace layout**: Side-by-side panels (2-3 columns) or tabbed interface with quick-switch? Recommend side-by-side for better comparison visibility.

2. **Compatibility adaptation approach**: Automatic parameter ID mapping with manual review, or fully manual mapping? Recommend automatic with review (faster workflow, safety net).

3. **Landing page focus**: Personalized "my active collections" workspace or organization-wide "all collections overview"? Recommend personalized (task-focused, less noise).

4. **Feature pull behavior**: Copy feature (creates duplicate) or reference feature (links to original)? Recommend copy (independence, version-specific adaptations).

5. **Context profile storage**: Database-backed (shareable across devices) or localStorage (per-browser only)? Recommend database for multi-device support.

6. **Cross-program feature pulling**: Always require compatibility wizard, or allow "pull as-is" with warnings? Recommend warnings-only for same-version pulls, wizard required for cross-version.

### Technology Stack Recommendations

**Frontend JavaScript**: Alpine.js (lightweight, Razor-friendly) or Vue.js (richer interactions) for reactive components (parameter editor, workflow designer)

**Visualization**: Mermaid.js for workflow diagrams (simple, markdown-based), D3.js for version matrices and dependency graphs, custom SVG for byte packet correlation trees

**Icons**: FontAwesome or Bootstrap Icons for consistent iconography

### Further Considerations

1. **JavaScript framework choice** — Use Alpine.js (simpler, Razor Pages compatible) or Vue.js (more powerful for complex interactions like workflow designer)? Alpine.js recommended for faster POC development.

2. **Workflow visualization library** — Mermaid.js (simple, markdown-based), D3.js (powerful, steeper learning curve), or custom SVG rendering? Mermaid.js recommended for execution flow diagrams.

3. **Mock data depth** — Current enhanced features provide 7 realistic features with PICCOLO/LPI mappings. Should we add more features (Frequency Lowering, Output Equalization, Wind Noise Reduction) or focus on polishing UI for existing 7? Recommend focusing on UI polish for existing features.

4. **Database choice confirmation** — SQLite for portable demo database file works well for laptop presentations; confirm this meets leadership demo logistics, or prefer Azure SQL for cloud-hosted shared environment?

5. **Workflow execution simulation** — Should workflows be executable in POC (simulated PICCOLO responses showing parameter changes), or visualization-only? Execution dramatically improves stakeholder comprehension but adds 2-3 weeks development time.
