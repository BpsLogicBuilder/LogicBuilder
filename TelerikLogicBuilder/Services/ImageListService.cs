using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal class ImageListService : IImageListService
    {
        private ImageList? _imageList;
        public ImageList ImageList
        {
            get
            {
                if (_imageList != null)
                    return _imageList;

                _imageList = new ImageList
                {
                    Images =
                    {
                        Properties.Resources.OpenedFolder,
                        Properties.Resources.closedFolder,
                        Properties.Resources.Visio_Application_16xLG,
                        Properties.Resources.Visio_16,
                        Properties.Resources.TABLE,
                        Properties.Resources.Project,
                        Properties.Resources.OPENFOLDCUT,
                        Properties.Resources.CLSDFOLDCUT,
                        Properties.Resources.File,
                        Properties.Resources.Open,
                        Properties.Resources.Delete,
                        Properties.Resources.newFolder,
                        Properties.Resources.Cut,
                        Properties.Resources.refresh,
                        Properties.Resources.Copy
                    }
                };

                return _imageList;
            }
        }
    }
}
