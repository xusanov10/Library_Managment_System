using Microsoft.AspNetCore.Mvc;

namespace Libray_Managment_System.DTOModels
{
    public class ResultDTO : IActionResult
    {
        public string? Message { get; set; }
        public int StatusCode { get; set; }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            JsonResult result = new JsonResult(this)
            {
                StatusCode = this.StatusCode
            };
            await result.ExecuteResultAsync(context);
        }
    }

    public class ResultDTO<T> : ResultDTO
    {
        public T Data { get; set; }
    }
}
