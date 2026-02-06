using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using WgMod.Common.Players;

namespace WgMod.Content.Items.Armor.VacuumArmor;

[AutoloadEquip(EquipType.Head)]
public class VacuumHelmet : ModItem
{
    float _critChance;
    int _health;
    int _defense;
    float _resist;
    float _movePenalty;
    int _setBonusRegen;

    public override void SetDefaults()
    {
        Item.width = 18;
        Item.height = 18;
        Item.value = Item.sellPrice(gold: 1);
        Item.rare = ItemRarityID.Red;
        Item.defense = 36;
    }

    public override void SetStaticDefaults()
    {
        SetBonusText = this.GetLocalization("VacuumSetBonus");
    }

    public static LocalizedText SetBonusText { get; private set; }

    public override bool IsArmorSet(Item head, Item body, Item legs)
    {
        return body.type == ModContent.ItemType<VacuumCrop>()
            && legs.type == ModContent.ItemType<VacuumSkirt>();
    }

    public override void UpdateArmorSet(Player player)
    {
        if (!player.TryGetModPlayer(out WgPlayer wg))
            return;
        wg._vacuumSetBonus = true;
        player.setBonus = SetBonusText.Value;
    }

    public override void UpdateEquip(Player player)
    {
        if (!player.TryGetModPlayer(out WgPlayer wg))
            return;

        float immobility = wg.Weight.ClampedImmobility;
        _critChance = float.Lerp(1.04f, 1.08f, immobility);
        _health = (int)MathF.Floor((int)float.Lerp(50, 100, immobility) / 5f) * 5;
        _defense = (int)float.Lerp(6f, 12f, immobility);
        _resist = float.Lerp(0.02f, 0.04f, immobility);
        _movePenalty = float.Lerp(1.1f, 0.95f, immobility);

        player.GetCritChance(DamageClass.Generic) *= _critChance;
        player.statLifeMax2 += _health;
        player.statDefense += _defense;
        player.endurance += _resist;
        wg.MovementPenalty *= _movePenalty;

        player.aggro += 5;

        if (!wg._vacuumSetBonus)
            return;
        _setBonusRegen = (int)float.Lerp(5f, 20f, immobility);
        player.lifeRegen += _setBonusRegen;
        wg.WeightLossRate *= 0.5f;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.FragmentSolar, 2)
            .AddIngredient(ItemID.FragmentNebula, 2)
            .AddIngredient(ItemID.FragmentVortex, 2)
            .AddIngredient(ItemID.FragmentStardust, 2)
            .AddIngredient(ItemID.LunarBar, 8)
            .AddTile(TileID.LunarCraftingStation)
            .Register();
    }
}
