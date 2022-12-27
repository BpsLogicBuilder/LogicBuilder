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
                        Properties.Resources.OpenedFolderCut,
                        Properties.Resources.closedFolderCut,
                        Properties.Resources.File,
                        Properties.Resources.Open,
                        Properties.Resources.Delete,
                        Properties.Resources.newFolder,
                        Properties.Resources.Cut,
                        Properties.Resources.refresh,
                        Properties.Resources.Copy,
                        Properties.Resources.Edit,
                        Properties.Resources.functionSelector,
                        Properties.Resources.Visio_Application_16xLGCut,
                        Properties.Resources.Visio_16Cut,
                        Properties.Resources.TABLECUT,
                        Properties.Resources.Application,
                        Properties.Resources.Type,
                        Properties.Resources.LiteralParameter,
                        Properties.Resources.ObjectParameter,
                        Properties.Resources.LiteralListParameter,
                        Properties.Resources.ObjectListParameter,
                        Properties.Resources.CutLiteralParameter,
                        Properties.Resources.CutObjectParameter,
                        Properties.Resources.CutLiteralListParameter,
                        Properties.Resources.CutObjectListParameter,
                        Properties.Resources.constructorButtonImage,
                        Properties.Resources.CutConstructorButtonImage,
                        Properties.Resources.CutFunctionSelector,
                        Properties.Resources.VariableSelector,
                        Properties.Resources.CutVariableSelector,
                        Properties.Resources.Generic,
                        Properties.Resources.GenericList,
                        Properties.Resources.CutGeneric,
                        Properties.Resources.CutGenericList,
                        Properties.Resources.Field,
                        Properties.Resources.Property,
                        Properties.Resources.Indexer,
                        Properties.Resources.Method,
                    }
                };

                return _imageList;
            }
        }
    }
}
