using System.Drawing;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces
{
    internal interface ICompareImages
    {
        bool AreEqual(Image? firstImage, Image? secondImage);
    }
}
