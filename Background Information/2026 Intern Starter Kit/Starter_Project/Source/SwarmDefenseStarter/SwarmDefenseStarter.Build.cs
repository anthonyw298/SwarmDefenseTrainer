using UnrealBuildTool;

public class SwarmDefenseStarter : ModuleRules
{
    public SwarmDefenseStarter(ReadOnlyTargetRules Target) : base(Target)
    {
        PCHUsage = PCHUsageMode.UseExplicitOrSharedPCHs;

        PublicDependencyModuleNames.AddRange(new string[] {
            "Core",
            "CoreUObject",
            "Engine",
            "InputCore",
            "VN100Input",
            "HardwareTrigger"
        });
    }
}
