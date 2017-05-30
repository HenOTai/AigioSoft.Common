using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace AigioSoft.Common
{
    /// <summary>
    /// 分页模型
    /// </summary>
    /// <typeparam name="T">实体</typeparam>
    [JsonObject]
    public class PagedModel<T> : IEnumerable<T>
    {
        public PagedModel(int count, int? pagesize, int? currentPageIndex)
        {
            Total = count;
            if (pagesize.HasValue)
                PageSize = pagesize.Value;
            if (currentPageIndex.HasValue)
                CurrentPageIndex = currentPageIndex.Value;
        }

        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize { get; set; } = 25;

        /// <summary>
        /// 总条目
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount => (int)Math.Ceiling(Total * 1.0 / PageSize);

        /// <summary>
        /// 当前页码
        /// </summary>
        public int CurrentPageIndex { get; set; } = 1;

        /// <summary>
        /// 数据集
        /// </summary>
        public List<T> Table { get; set; }


        public IEnumerator<T> GetEnumerator() => Table.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Table.GetEnumerator();
    }

    public class PagedRequestModel
    {
        public int Pageindex { get; set; } = 1;
        public int Pagesize { get; set; } = 25;
    }
}
