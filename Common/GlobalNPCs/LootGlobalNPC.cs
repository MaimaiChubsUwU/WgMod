using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using WgMod.Content.Items.Accessories;

namespace WgMod.Common.GlobalNPCs;

public class LootGlobalNPC : GlobalNPC
{
    public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
    {
        if (npc.type == NPCID.Deerclops)
        {
            npcLoot.Add(
                ItemDropRule.ByCondition(
                    Condition.DownedDeerclops.ToDropCondition(ShowItemDropInUI.Always),
                    ModContent.ItemType<WeightLossPendant>(),
                    chanceDenominator: 2,
                    chanceNumerator: 1
                )
            );
        }
        if (npc.type == NPCID.QueenSlimeBoss)
        {
            npcLoot.Add(
                ItemDropRule.ByCondition(
                    Condition.DownedQueenSlime.ToDropCondition(ShowItemDropInUI.Always),
                    ModContent.ItemType<QueenlyGluttony>(),
                    chanceDenominator: 2,
                    chanceNumerator: 1
                )
            );
        }
    }
}
