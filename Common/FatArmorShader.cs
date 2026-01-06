using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace WgMod.Common;

/// <summary> Internal, do not use outside as an item. </summary>
internal class FatArmorShader : ModItem
{
    public override string Texture => "Terraria/Images/Item_1007";

    public override void SetStaticDefaults()
    {
        Asset<Texture2D> bellyTexture = Mod.Assets.Request<Texture2D>("Assets/Textures/Belly_ArmorFem");
        GameShaders.Armor.BindShader(Type, new ArmorShaderData(Mod.Assets.Request<Effect>("Assets/Effects/FatArmor"), "MainPass").UseImage(bellyTexture));
    }
}
