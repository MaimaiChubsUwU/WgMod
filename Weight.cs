using System;

namespace WgMod;

// Mass in Kg.
public readonly record struct Weight(float Mass)
{
    public const float Base = 70f;
    public const float Immobile = 400f;

    public const int StageCount = 8;
    public const int ImmobileStage = StageCount - 1;
    public const int BuffStage = 3;

    public readonly float Immobility => (Mass - Base) / (Immobile - Base);
    public readonly float ClampedImmobility => Math.Clamp(Immobility, 0f, 1f);

    public override readonly string ToString() => $"{Mass} Kg";

    public readonly float ToPounds() => Mass * 2.2046226218f;
    public readonly int GetStage() => (int)float.Lerp(0f, ImmobileStage, Immobility);

    public static Weight FromPounds(float pounds) => new(pounds / 2.2046226218f);
    public static Weight Clamp(Weight weight) => new(Math.Clamp(weight.Mass, Base, Immobile + 10f));

    public static Weight operator +(Weight w, float mass) => new(w.Mass + mass);
    public static Weight operator -(Weight w, float mass) => new(w.Mass - mass);
}
