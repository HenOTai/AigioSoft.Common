using Microsoft.AspNetCore.Mvc;

// ReSharper disable once CheckNamespace
namespace AigioSoft.Common
{
    /// <summary>
    /// 页面控制器基类
    /// </summary>
    [LowBrowserVerFilter]
    public abstract class BaseController : Controller
    {

    }
}
