namespace Contoso.Test.Flow.Test
{
    internal struct Constants
    {
        public const string FilterResultsFolder = "FilterResults";
        public const string TargetFilterFolder = "TargetFilter";
        public static readonly string ProjectFileFullPath = @$"{ProjectDirectory.GetSolutionPath()}\FlowProjects\Contoso.Test\Contoso.Test.lbproj";
        public static readonly string ShopBslProjectFileFullPath = @$"{ProjectDirectory.GetSolutionPath()}\FlowProjects\Shop.Bsl\Shop.Bsl.lbproj";
    }
}
