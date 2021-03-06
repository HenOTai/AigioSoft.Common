﻿#if !(NET20 || NET35 || NET40)
using System.ComponentModel.DataAnnotations;


// ReSharper disable once CheckNamespace
namespace AigioSoft.Common
{
    /// <summary>
    /// 泛型实体基类
    /// </summary>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    public abstract class Entity<TPrimaryKey>
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public virtual TPrimaryKey Id { get; set; }
    }

    /// <summary>
    /// 定义默认主键类型为Int32的实体基类
    /// </summary>
    public abstract class Entity : Entity<int>
    {

    }
}
#endif
