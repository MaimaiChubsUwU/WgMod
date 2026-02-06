using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using WgMod.Common.Players;

namespace WgMod.Content.Items.Armor.CrimatriarchArmor;

[AutoloadEquip(EquipType.Head)]
public class CrimatriarchHood : ModItem
{
    public const int SetBonusMinions = 1;

    float _damage;
    float _attackSpeed;
    float _setBonusDamage;
    float _setBonusAttackSpeed;

    public override void SetDefaults()
    {
        Item.width = 18;
        Item.height = 18;
        Item.value = Item.sellPrice(gold: 1);
        Item.rare = ItemRarityID.Orange;
        Item.defense = 4;
    }

    public override void SetStaticDefaults()
    {
        SetBonusText = this.GetLocalization("CrimatriarchSetBonus");
    }

    public static LocalizedText SetBonusText { get; private set; }

    public override void UpdateEquip(Player player)
    {
        if (!player.TryGetModPlayer(out WgPlayer wg))
            return;
        
        float immobility = wg.Weight.ClampedImmobility;
        _damage = float.Lerp(0.03f, 0.09f, immobility);
        _attackSpeed = float.Lerp(0.98f, 0.94f, immobility);

        player.GetDamage(DamageClass.Summon) += _damage;
        player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) *= _attackSpeed;
    }

    public override bool IsArmorSet(Item head, Item body, Item legs)
    {
        return body.type == ModContent.ItemType<CrimatriarchGown>()
            && legs.type == ModContent.ItemType<CrimatriarchLeggings>();
    }

    public override void UpdateArmorSet(Player player)
    {
        if (!player.TryGetModPlayer(out WgPlayer wg))
            return;

        float immobility = wg.Weight.ClampedImmobility;
        _setBonusDamage = float.Lerp(0.05f, 0.10f, immobility);
        _setBonusAttackSpeed = float.Lerp(0.95f, 0.9f, immobility);

        player.GetDamage(DamageClass.Summon) += _setBonusDamage;
        player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) *= _setBonusAttackSpeed;
        player.maxMinions += SetBonusMinions;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.CrimtaneBar, 20)
            .AddIngredient(ItemID.Bone, 15)
            .AddIngredient(ItemID.TissueSample, 5)
            .AddTile(TileID.Anvils)
            .Register();
    }
}
