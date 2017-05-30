

// ReSharper disable once CheckNamespace
namespace AigioSoft.Common
{
    /// <summary>
    /// Api返回模型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResult<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
    }

    /// <summary>
    /// Api返回模型
    /// </summary>
    public class ApiResult : ApiResult<object>
    {

    }
}
