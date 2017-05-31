#if !(NET20 || NET35 || NET40)
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace AigioSoft.Common.Helpers
{
    /// <summary>
    /// IP操作
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public static class IPAddress
    {
        /// <summary>
        /// IP定位
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="key"></param>
        /// <param name="apiData"></param>
        /// <returns></returns>
        public static (string responseText, IPositioningApiResult responseData) Positioning(string ip, string key,
            IPositioningApi apiData)
        {
            var args = apiData.Args?.ToList() ?? new List<KeyValuePair<string, string>>();
            args.Add(new KeyValuePair<string, string>(apiData.DeveloperKeyName, key));
            args.Add(new KeyValuePair<string, string>(apiData.IpKeyName, ip));
            var responseText = HttpClient.Send(apiData.Url, nameValueCollection: args);
            var responseData = JsonConvert.DeserializeObject(responseText, apiData.ResultType) as IPositioningApiResult;
            return (responseText, responseData);
        }
    }
}

namespace AigioSoft.Common
{

    public interface IPositioningApi
    {
        /// <summary>
        /// 请求地址
        /// </summary>
        string Url { get; }

        /// <summary>
        /// 请求参数
        /// </summary>
        KeyValuePair<string, string>[] Args { get; }

        /// <summary>
        /// 开发者密钥参数名
        /// </summary>
        string DeveloperKeyName { get; }

        /// <summary>
        /// IP参数名
        /// </summary>
        string IpKeyName { get; }

        /// <summary>
        /// 并发量
        /// </summary>
        uint? Concurrency { get; }

        /// <summary>
        /// 并发量周期
        /// </summary>
        TimeSpan? ConcurrencyTimeSpan { get; }

        /// <summary>
        /// 日配额
        /// </summary>
        uint? DayQuota { get; }

        Type ResultType { get; }
    }
    public interface IPositioningApiResult
    {
        string Province { get; }
        string City { get; }
    }

    /// <summary>
    /// http://lbsyun.baidu.com/index.php?title=webapi/ip-api
    /// </summary>
    public class BaiduMapPositioning : IPositioningApi
    {
        public string Url => "https://api.map.baidu.com/location/ip";
        public KeyValuePair<string, string>[] Args =>
            new[]
            {
                new KeyValuePair<string, string>("coor", "bd09ll")
            };
        public string DeveloperKeyName => "ak";
        public string IpKeyName => "ip";
        public uint? Concurrency => 6000;
        public TimeSpan? ConcurrencyTimeSpan => new TimeSpan(0, 1, 0);
        public uint? DayQuota => 100000;
        public Type ResultType => typeof(BaiduMapPositioningResult);
    }

    public class BaiduMapPositioningResult : IPositioningApiResult
    {
        // ReSharper disable InconsistentNaming
        /// <summary>
        /// 地址
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// 详细内容
        /// </summary>
        public BaiduMapPositioningResultContent content { get; set; }
        /// <summary>
        /// 返回状态码 http://lbsyun.baidu.com/index.php?title=lbscloud/api/appendix 附录3：控制服务返回码定义
        /// </summary>
        public int status { get; set; }
        // ReSharper restore InconsistentNaming

        public string Province => content.address_detail.province;
        public string City => content.address_detail.city;
    }

    public class BaiduMapPositioningResultContent
    {
        // ReSharper disable InconsistentNaming
        /// <summary>
        /// 简要地址
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// 详细地址信息
        /// </summary>
        public BaiduMapPositioningResultContentAddressDetail address_detail { get; set; }
        /// <summary>
        /// 当前城市中心点，注意当前坐标返回类型
        /// </summary>
        public BaiduMapPositioningResultContentPoint point { get; set; }
        // ReSharper restore InconsistentNaming
    }

    public class BaiduMapPositioningResultContentAddressDetail
    {
        // ReSharper disable InconsistentNaming
        /// <summary>
        /// 城市
        /// </summary>
        public string city { get; set; }
        /// <summary>
        /// 百度城市代码
        /// </summary>
        public int city_code { get; set; }
        /// <summary>
        /// 区县
        /// </summary>
        public string district { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        public string province { get; set; }
        /// <summary>
        /// 街道
        /// </summary>
        public string street { get; set; }
        /// <summary>
        /// 门址
        /// </summary>
        public string street_number { get; set; }
        // ReSharper restore InconsistentNaming
    }

    public class BaiduMapPositioningResultContentPoint
    {
        // ReSharper disable InconsistentNaming
        public decimal x { get; set; }
        public decimal y { get; set; }
        // ReSharper restore InconsistentNaming
    }

    /// <summary>
    /// http://lbs.qq.com/webservice_v1/guide-ip.html
    /// </summary>
    public class TencentMapPositioning : IPositioningApi
    {
        public string Url => "http://apis.map.qq.com/ws/location/v1/ip";
        public KeyValuePair<string, string>[] Args => null;
        public string DeveloperKeyName => "key";
        public string IpKeyName => "ip";
        public uint? Concurrency => 5;
        public TimeSpan? ConcurrencyTimeSpan => new TimeSpan(0, 0, 1);
        public uint? DayQuota => 50000;
        public Type ResultType => typeof(TencentMapPositioningResult);
    }

    public class TencentMapPositioningResult : IPositioningApiResult
    {
        // ReSharper disable InconsistentNaming
        /// <summary>
        /// 状态码
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 状态说明
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// ip定位结果
        /// </summary>
        public TencentMapPositioningResultResult result { get; set; }

        public string Province => result.ad_info.province;

        public string City => result.ad_info.city;
        // ReSharper restore InconsistentNaming
    }

    public class TencentMapPositioningResultResult
    {
        // ReSharper disable InconsistentNaming
        /// <summary>
        /// 定位坐标
        /// </summary>
        public TencentMapPositioningResultResultLocation location { get; set; }
        /// <summary>
        /// 地址部件，address不满足需求时可自行拼接
        /// </summary>
        public TencentMapPositioningResultResultAdInfo ad_info { get; set; }
        // ReSharper restore InconsistentNaming
    }

    public class TencentMapPositioningResultResultLocation
    {
        // ReSharper disable InconsistentNaming
        /// <summary>
        /// 纬度
        /// </summary>
        public decimal lat { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public decimal lng { get; set; }
        // ReSharper restore InconsistentNaming
    }

    public class TencentMapPositioningResultResultAdInfo
    {
        // ReSharper disable InconsistentNaming
        /// <summary>
        /// 国家
        /// </summary>
        public string nation { get; set; }
        /// <summary>
        /// 省
        /// </summary>
        public string province { get; set; }
        /// <summary>
        /// 市
        /// </summary>
        public string city { get; set; }
        /// <summary>
        /// 行政区划代码
        /// </summary>
        public int adcode { get; set; }
        // ReSharper restore InconsistentNaming
    }

    /// <summary>
    /// http://lbs.amap.com/api/webservice/guide/api/ipconfig
    /// </summary>
    public class AmapPositioning : IPositioningApi
    {
        public string Url => "http://restapi.amap.com/v3/ip";
        public KeyValuePair<string, string>[] Args => null;
        public string DeveloperKeyName => "key";
        public string IpKeyName => "ip";
        public uint? Concurrency => 10000;
        public TimeSpan? ConcurrencyTimeSpan => new TimeSpan(0, 1, 0);
        public uint? DayQuota => null;
        public Type ResultType => typeof(AmapPositioningResult);
    }

    public class AmapPositioningResult : IPositioningApiResult
    {
        // ReSharper disable InconsistentNaming
        /// <summary>
        /// 返回结果状态值
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 返回状态说明
        /// </summary>
        public string info { get; set; }
        /// <summary>
        /// 状态码
        /// </summary>
        public string infocode { get; set; }
        /// <summary>
        /// 省份名称
        /// </summary>
        public string province { get; set; }
        /// <summary>
        /// 城市名称
        /// </summary>
        public string city { get; set; }
        /// <summary>
        /// 城市的adcode编码
        /// </summary>
        public string adcode { get; set; }
        /// <summary>
        /// 所在城市矩形区域范围
        /// </summary>
        public string rectangle { get; set; }
        public string Province => province;
        public string City => city;
        // ReSharper restore InconsistentNaming
    }
}
#endif