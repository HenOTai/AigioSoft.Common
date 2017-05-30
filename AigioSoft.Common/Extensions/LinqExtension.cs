using System.Linq;

// ReSharper disable once CheckNamespace
namespace AigioSoft.Common
{
    /// <summary>
    /// System.Linq.* 扩展
    /// </summary>
    public static class LinqExtension
    {
        /// <summary>
        /// 分页（调用此方法前需要进行排序）
        /// </summary>
        /// <typeparam name="TEntity">模型类</typeparam>
        /// <param name="entities">集合</param>
        /// <param name="pagesize">页大小 默认值25</param>
        /// <param name="currentPageIndex">页码（下标） 默认值1</param>
        /// <param name="startFromZero">页码是否从0开始 默认值从1开始</param>
        /// <returns></returns>
        public static PagedModel<TEntity> Paging<TEntity>(this IQueryable<TEntity> entities,
            int? currentPageIndex = null, int? pagesize = null, bool startFromZero = false)
        {
            var result = new PagedModel<TEntity>(entities.Count(), pagesize, currentPageIndex);
            var index = result.CurrentPageIndex;
            if (!startFromZero)
                index -= 1;
            result.Table = entities.Skip(index * result.PageSize).Take(result.PageSize).ToList();
            return result;
        }
    }
}