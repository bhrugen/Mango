using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mango.Web.Models
{
    public class ResponseDto
    {
        public object? Result { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string? ErrorMessage { get; set; }

        public T? GetResult<T>() =>
            JsonConvert.DeserializeObject<T>(Convert.ToString(Result) ?? string.Empty);
    }
}
