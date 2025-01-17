﻿using ComfyLib;

using HarmonyLib;

using UnityEngine;
using UnityEngine.UI;

using static ZoneScouter.PluginConfig;

namespace ZoneScouter {
  [HarmonyPatch(typeof(Menu))]
  static class MenuPatch {
    [HarmonyPostfix]
    [HarmonyPatch(nameof(Menu.Start))]
    static void StartPostfix(ref Menu __instance) {
      if (!IsModEnabled.Value) {
        return;
      }

      foreach (GameObject child in __instance.m_menuDialog.gameObject.Children()) {
        if (child.name.StartsWith("darken") && child.TryGetComponent(out Image image)) {
          image.raycastTarget = false;
        }
      }
    }
  }
}
