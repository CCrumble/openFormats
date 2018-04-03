using System.Collections.Generic;

namespace OpenIV.Formats.GTA5
{
    public class Skeleton
    {
        public uint DataCRC { get; set; }
        public ushort NumBones { get; set; }
        public Bone RootBone;

        public static FormatsType Type => FormatsType.Drawable;
        public static FormatsVersion Version => FormatsVersion.Skeleton;
    }

    public class Bone
    {
        public string Name { get; set; }
        public ushort Id { get; set; }

        public ushort MirrorBoneId { get; set; }
        public List<string> Flags { get; set; }
        public Quaternion RotationQuaternion { get; set; }
        public Vector3 LocalOffset { get; set; }
        public Vector3 Scale { get; set; }
        public List<Bone> Children { get; set; }
    }
}
