using Kingmaker;
using Kingmaker.UnitLogic.Commands;
using Kingmaker.UnitLogic.Commands.Base;
using System.Reflection;
 
//class UnitAttackOfOpportunity_OnRun_Patch
namespace BetterCombat.Patches.Vanilla.AttackOfOpportunity
{
    [Harmony12.HarmonyPatch(typeof(UnitAttackOfOpportunity), nameof(UnitAttackOfOpportunity.OnRun))]
    // if you want to change the class name, you'll have to modify the following line in Main.cs:
    // if (HarmonyPatcher.ApplyPatch(typeof(UnitCombatState_AttackOfOpportunity_Patch), "AoO Hard Patch"))
    class UnitCombatState_AttackOfOpportunity_Patch
    {
        [Harmony12.HarmonyPrefix]
        static bool Prefix(UnitAttackOfOpportunity __instance)
        {
            if (Game.Instance.IsPaused && __instance.Target != null && __instance.Target != __instance.Executor)
            {
                __instance.Executor.ForceLookAt(__instance.Target.Position);
            }
            typeof(UnitCommand).GetProperty("DidRun", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(__instance, true);
            if (__instance.NeedEquipWeapons)
            {
                __instance.Executor.View.HandleAttackCommandRun();
            }
            if (!(__instance.Executor.View.AnimationManager == null) && __instance.Executor.View.AnimationManager.CanRunIdleAction())
            {
                AttackHandInfo attackHandInfo = new AttackHandInfo(__instance.Hand, 0, 0);
                attackHandInfo.CreateAnimationHandleForAttack();
                if (attackHandInfo.AnimationHandle != null)
                {
                    attackHandInfo.AnimationHandle.SpeedScale = 1.5f;
                    __instance.Executor.LookAt(__instance.Target.Position);
                    __instance.Executor.View.AnimationManager.ExecuteIfIdle(attackHandInfo.AnimationHandle);
                }
            }
            return false;
        }
    }
}
