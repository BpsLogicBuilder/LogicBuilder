namespace Contoso.Test.Business.Responses
{
    public class GenericResponse<T>
    {
        public GenericResponse(T tProperty)
        {
            TProperty = tProperty;
        }

        public T TProperty { get; set; }
    }
}
