using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

// ReSharper disable once CheckNamespace
namespace AigioSoft.Common.Helpers
{
    /// <summary>
    /// 功能-正式环境操作
    /// </summary>
    public static class Release
    {
        private const string WebConfigFileName = "web.config";

        /// <summary>
        /// 添加Web.Config节点屏蔽Http响应头中X-Powered-By
        /// </summary>
        public static string[] RemoveXPoweredBy()
        {
            var result = new List<string>();
            var configPath = Path.Combine(Server.BaseDirectory, WebConfigFileName);
            var exists = File.Exists(configPath);
            result.Add($"configPath:{configPath}.exists:{exists}");
            if (exists)
            {
                var doc = new XmlDocument();
                using (var fileStream = new FileStream(configPath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    result.Add("fileStream:Start");
                    doc.Load(fileStream);
                    if (doc.HasChildNodes)
                    {
                        result.Add("HasChildNodes:true");
                        var configurationNode = doc.ChildNodes.Cast<XmlNode>().FirstOrDefault(x => x.Name.Equals("configuration", StringComparison.OrdinalIgnoreCase));
                        if (configurationNode != null)
                        {
                            result.Add("configuration:have");
                            if (configurationNode.HasChildNodes)
                            {
                                var webServerNode = configurationNode.ChildNodes.Cast<XmlNode>()
                                    .FirstOrDefault(x => x.Name.Equals("system.webServer",
                                        StringComparison.OrdinalIgnoreCase));
                                var httpProtocolNode = webServerNode.ChildNodes.Cast<XmlNode>()
                                    .FirstOrDefault(
                                        x => x.Name.Equals("httpProtocol", StringComparison.OrdinalIgnoreCase));
                                if (httpProtocolNode == null)
                                {
                                    httpProtocolNode = doc.CreateElement("httpProtocol");
                                    webServerNode.AppendChild(httpProtocolNode);

                                    var customHeadersNode = webServerNode.ChildNodes.Cast<XmlNode>()
                                        .FirstOrDefault(
                                            x => x.Name.Equals("customHeaders", StringComparison.OrdinalIgnoreCase));
                                    if (customHeadersNode == null)
                                    {
                                        customHeadersNode = doc.CreateElement("customHeaders");
                                        httpProtocolNode.AppendChild(customHeadersNode);

                                        var removeNode = webServerNode.ChildNodes.Cast<XmlNode>()
                                            .FirstOrDefault(x => x.Name.Equals("remove", StringComparison.OrdinalIgnoreCase)
                                                                 &&
                                                                 (x.Attributes["name"]
                                                                      ?.ToString()
                                                                      .Equals("X-Powered-By",
                                                                          StringComparison.OrdinalIgnoreCase) ?? false));
                                        if (removeNode == null)
                                        {
                                            removeNode = doc.CreateElement("remove");
                                            removeNode.Attributes.Append(doc.CreateAttribute("name"));
                                            removeNode.Attributes["name"].Value = "X-Powered-By";
                                            customHeadersNode.AppendChild(removeNode);
                                            result.Add("add:true");

                                            fileStream.Seek(0, SeekOrigin.Begin);
                                            doc.Save(fileStream);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return result.ToArray();
        }

        /// <summary>
        /// 删除多余的静态文件
        /// </summary>
        /// <param name="removeJavaScriptFiles">删除JS</param>
        /// <param name="removeTypeScriptFiles">删除TS</param>
        /// <param name="removeStyleSheetsFiles">删除CSS</param>
        /// <param name="removeMapFiles">删除Map</param>
        public static string[] RemoveStaticFiles(bool removeJavaScriptFiles = true, bool removeTypeScriptFiles = true, bool removeStyleSheetsFiles = true, bool removeMapFiles = true)
        {
#if DEBUG
            return null;
#endif
            var result = new List<string>();
            var rootPath = Server.BaseDirectory;
            RemoveDir(new DirectoryInfo(rootPath));
            return result.ToArray();
            void RemoveDir(params DirectoryInfo[] dirinfos)
            {
                result.Add($"dirinfos:{string.Join(";", dirinfos.Select(x => x.Name))}");
                foreach (var dirinfo in dirinfos.Where(x => x.Exists))
                {
                    RemoveFiles(dirinfo.GetFiles());
                    RemoveDir(dirinfo.GetDirectories());
                }
            }
            void RemoveFiles(FileInfo[] fileinfos)
            {
                if (fileinfos != null && fileinfos.Any())
                {
                    fileinfos = fileinfos.Where(x => x.Exists).ToArray();
                    result.Add($"fileinfos:{string.Join(";", fileinfos.Select(x => x.Name))}");
                    var deleteList = new List<FileInfo>();
                    if (removeJavaScriptFiles)
                        deleteList.AddRange(fileinfos.Where(fileinfo => fileinfo.Extension.EndsWith(".js", StringComparison.OrdinalIgnoreCase)
                        && !fileinfo.Name.EndsWith(".min.js", StringComparison.OrdinalIgnoreCase)));
                    if (removeTypeScriptFiles)
                    {
                        deleteList.AddRange(fileinfos.Where(fileinfo => fileinfo.Extension.EndsWith(".ts", StringComparison.OrdinalIgnoreCase)));
                        deleteList.AddRange(fileinfos.Where(
                            fileinfo => fileinfo.Name.Equals("tsconfig.json", StringComparison.OrdinalIgnoreCase)));
                    }
                    if (removeStyleSheetsFiles)
                        deleteList.AddRange(fileinfos.Where(fileinfo => fileinfo.Extension.EndsWith(".css", StringComparison.OrdinalIgnoreCase)
                        && !fileinfo.Name.EndsWith(".min.css", StringComparison.OrdinalIgnoreCase)));
                    if (removeMapFiles)
                        deleteList.AddRange(fileinfos.Where(fileinfo => fileinfo.Extension.EndsWith(".map", StringComparison.OrdinalIgnoreCase)));
                    if (deleteList.Any())
                        foreach (var item in deleteList)
                        {
                            if (item.Extension.EndsWith(".css", StringComparison.OrdinalIgnoreCase) &&
                                !File.Exists(item.FullName.Substring(0, item.FullName.Length - 4) + ".min.css"))
                                continue;
                            if (item.Extension.EndsWith(".js", StringComparison.OrdinalIgnoreCase) &&
                                !File.Exists(item.FullName.Substring(0, item.FullName.Length - 3) + ".min.js"))
                                continue;
#if !DEBUG
                            item.Delete();
#endif
                            result.Add($"Delete:{item.FullName}");
                        }
                }
            }
        }
    }
}
