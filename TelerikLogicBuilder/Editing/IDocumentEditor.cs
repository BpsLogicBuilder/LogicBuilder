namespace ABIS.LogicBuilder.FlowBuilder.Editing
{
    internal interface IDocumentEditor
    {
        void Close();
        void Delete();
        void Edit();
        void FindConstructor();
        void FindFunction();
        void FindText();
        void FindVariable();
        void ReplaceConstructor();
        void ReplaceFunction();
        void ReplaceText();
        void ReplaceVariable();
        void Save();
        string Caption { get; }
        bool IsOpenReadOnly { get; }
        string SourceFile { get; }
    }
}
