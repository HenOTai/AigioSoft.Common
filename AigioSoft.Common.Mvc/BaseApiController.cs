using Microsoft.AspNetCore.Mvc;

// ReSharper disable once CheckNamespace
namespace AigioSoft.Common
{
    /// <summary>
    /// Api控制器基类
    /// </summary>
    public abstract class BaseApiController : Controller
    {
        protected static ApiResult Result<T>(T data, bool success = false)
        {
            return new ApiResult
            {
                Success = success,
                Data = data
            };
        }

        protected static ApiResult ResultTrue<T>(T data) => Result(data, true);
        protected static ApiResult ResultTrue(object data) => Result(data, true);

        protected static ApiResult Result(object data, bool success = false)
        {
            return new ApiResult
            {
                Success = success,
                Data = data
            };
        }
    }
}
