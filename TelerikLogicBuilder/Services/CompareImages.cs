using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Drawing;
using System.IO;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal class CompareImages : ICompareImages
    {
        public bool AreEqual(Image? firstImage, Image? secondImage)
            => ByteArraysEqual(ImageToByteArray(firstImage), ImageToByteArray(secondImage));

        static byte[] ImageToByteArray(Image? image)
        {
            if (image == null)
                return Array.Empty<byte>();

            using MemoryStream memoryStream = new();
            image.Save(memoryStream, image.RawFormat);
            return memoryStream.ToArray();
        }

        private static bool ByteArraysEqual(byte[]? left, byte[]? right)
        {
            if (object.ReferenceEquals(left, right))
                return true;

            if (left == null || right == null)
                return false;

            if (left.Length != right.Length)
                return false;

            for (int i = 0; i < left.Length; i++)
            {
                if (left[i] != right[i])
                    return false;
            }

            return true;
        }
    }
}
