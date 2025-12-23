using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WgMod.Common.Players;

namespace WgMod
{
    // Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
    public partial class WgMod : Mod
    {
        public override void Load()
        {
            On_Player.AddBuff += OnPlayerAddBuff;
        }

        public override void Unload()
        {
            On_Player.AddBuff -= OnPlayerAddBuff;
        }

        public static void OnPlayerAddBuff(On_Player.orig_AddBuff orig, Player self, int type, int timeToAdd, bool quiet, bool foodHack)
        {
            if (self.TryGetModPlayer(out WgPlayer wg) && WgPlayer.BuffTable.TryGetValue(type, out float mass))
                wg.SetWeight(wg.Weight + mass);
            orig(self, type, timeToAdd, quiet, foodHack);
        }
    }
}
