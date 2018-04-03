using System.Collections.Generic;

namespace OpenIV.Formats.GTA5
{
    public class Light
    {
        public List<LightAttribute> Lights { get; set; }
        public static FormatsType Type => FormatsType.Drawable;
        public static FormatsVersion Version => FormatsVersion.Light;
    }

    public class LightAttribute
    {
        public Vector3 Position { get; set; }
        public Color3 Color { get; set; }
        public byte Flashiness { get; set; }
        public float Intensity { get; set; }
        public uint Flags { get; set; }
        public ushort BoneTag { get; set; }
        public byte LightType { get; set; }
        public byte GroupId { get; set; }
        public uint TimeFlags { get; set; }
        public float Falloff { get; set; }
        public float FalloffExponent { get; set; }
        public Vector4 CullingPlane { get; set; }
        public byte ShadowBlur { get; set; }
        public float VolumeIntensity { get; set; }
        public float VolumeSizeScale { get; set; }
        public Color3 VolumeOuterColor { get; set; }
        public byte LightHash { get; set; }
        public float VolumeOuterIntensity { get; set; }
        public float CoronaSize { get; set; }
        public float VolumeOuterExponent { get; set; }
        public byte LightFadeDistance { get; set; }
        public byte ShadowFadeDistance { get; set; }
        public byte SpecularFadeDistance { get; set; }
        public byte VolumetricFadeDistance { get; set; }
        public float ShadowNearClip { get; set; }
        public float CoronaIntensity { get; set; }
        public float CoronaZBias { get; set; }
        public Vector3 Direction { get; set; }
        public Vector3 Tangent { get; set; }
        public float ConeInnerAngle { get; set; }
        public float ConeOuterAngle { get; set; }
        public Vector3 Extents { get; set; }
        public uint ProjectedTextureHash { get; set; }
    }
}
