namespace Core.Shared.Tools;

public class OperationResult<TDataType>
{
    public OperationResultType Type { get; set; }
    public TDataType Record { get; set; }
    public List<TDataType> Records { get; set; }
    public string Message { get; set; }
    public Response Response { get; set; }
}

public enum OperationResultType
{
    Single,
    Bulk
}

public enum Response
{
    Success,
    Warning,
    Failed
}