using System.IO;
using System.Text;
using System.Xml.Linq;

// ReSharper disable once CheckNamespace
namespace AigioSoft.Common
{
    /// <summary>
    /// System.Xml.Linq.XDocument 扩展
    /// </summary>
    public static class XDocumentExtension
    {
        /// <summary>
        /// 将 XML 文档对象转换为 XML 字符串。
        /// </summary>
        /// <param name="xdoc"></param>
        /// <returns></returns>
        public static string ToXmlString(this XDocument xdoc)
        {
            StringBuilder builder = new StringBuilder();
            using (TextWriter writer = new StringWriter(builder))
                xdoc.Save(writer);
            return builder.ToString();
        }
    }
}