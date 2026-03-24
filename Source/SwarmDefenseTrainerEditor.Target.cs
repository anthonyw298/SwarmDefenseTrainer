using UnrealBuildTool;
using System.Collections.Generic;

public class SwarmDefenseTrainerEditorTarget : TargetRules
{
    public SwarmDefenseTrainerEditorTarget(TargetInfo Target) : base(Target)
    {
        Type = TargetType.Editor;
        DefaultBuildSettings = BuildSettingsVersion.Latest;
        IncludeOrderVersion = EngineIncludeOrderVersion.Latest;
        ExtraModuleNames.Add("SwarmDefenseTrainer");
    }
}
