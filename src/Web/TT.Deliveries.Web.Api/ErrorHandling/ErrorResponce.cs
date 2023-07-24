using System.Text.Json;

namespace  TT.Deliveries.Web.Api.ErrorHandling;

/// <summary>
/// Error payload
/// </summary>
public class ErrorResponce
{
    public int StatusCode { get; set; }
    public string Message { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}