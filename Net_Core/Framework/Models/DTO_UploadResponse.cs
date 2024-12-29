using System;

namespace GrowthWare.Framework.Models;

[Serializable(), CLSCompliant(true)]
public class DTO_UploadResponse
{
    public dynamic Data { get; set; }
    public string ErrorMessage { get; set; }
    public string FileName { get; set; }
    public bool IsSuccess { get; set; } = true;
}