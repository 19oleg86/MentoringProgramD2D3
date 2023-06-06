namespace Categories.WebApi.Models.Output
{
    public class OutputErrorDataModel : OutputDataModel<OutputErrorDataType>
    {
        public OutputErrorDataModel(OutputErrorDataType data) 
            : base(data)
        {
            IsError = true;
        }
    }
}
