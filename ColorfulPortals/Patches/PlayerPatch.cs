﻿using System;
using System.Collections.Generic;
using System.Reflection.Emit;

using HarmonyLib;

using static ColorfulPortals.PluginConfig;

namespace ColorfulPortals {
  [HarmonyPatch(typeof(Player))]
  static class PlayerPatch {
    [HarmonyTranspiler]
    [HarmonyPatch(nameof(Player.Update))]
    static IEnumerable<CodeInstruction> UpdateTranspiler(IEnumerable<CodeInstruction> instructions) {
      return new CodeMatcher(instructions)
          .MatchForward(
              useEnd: false,
              new CodeMatch(OpCodes.Ldarg_0),
              new CodeMatch(OpCodes.Call, AccessTools.Method(typeof(Player), nameof(Player.UpdateHover))))
          .Advance(offset: 2)
          .InsertAndAdvance(
              new CodeInstruction(OpCodes.Ldloc_1),
              Transpilers.EmitDelegate<Action<bool>>(UpdateHoverPostDelegate))
          .InstructionEnumeration();
    }

    static void UpdateHoverPostDelegate(bool takeInput) {
      if (IsModEnabled.Value
          && ChangePortalColorShortcut.Value.IsDown()
          && Player.m_localPlayer
          && Player.m_localPlayer.m_hovering) {
        ColorfulPortals.ChangePortalColor(Player.m_localPlayer.m_hovering.GetComponentInParent<TeleportWorld>());
      }
    }
  }
}
