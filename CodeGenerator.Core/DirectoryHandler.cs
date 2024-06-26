﻿using CodeGenerator.Core.Dtos;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.IO;
using System.Reflection;

namespace CodeGenerator.Core
{
    public class DirectoryHandler
    {
        public DirectoryHandler()
        {
        }

        public static void CreateDirectory(string path, string fileName)
        {
            var newPath = Path.Combine(path, fileName);
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
        }

        public static string GetAppRoot()
        {
            
            var assembly = Assembly.GetAssembly(typeof(DirectoryHandler));
            var dir= Path.GetDirectoryName(assembly.Location);
            var root= Directory.GetParent(dir).Parent.Parent.Parent.FullName;
            return root;
        }

        public static string GetNameSpace(string path)
        {
            var root = GetAppRoot();

            if (path.Last() == '/')
            {
                path = path.Substring(path.Length - 1);
            }

            var editedRoot = root.Replace("\\", "/");

            var editedPath = path.Replace(editedRoot, "");
            editedPath = editedPath.Substring(1, editedPath.Length - 1);

            var nameSpace = editedPath.Replace('/', '.');
            return nameSpace;

        }

        public static string? GetClassNameSpace(string path)
        {
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText(path));
            var rootNode = syntaxTree.GetRoot();
            var namespaceString = rootNode.DescendantNodes().OfType<NamespaceDeclarationSyntax>()?.Select(x => x.Name.ToString()).FirstOrDefault();

            return namespaceString;

        }

        public static List<FileModel> GetViewFiles(string? controllerPath)
        {
                var views = new List<FileModel>();
            try
            {
                var controllerName = Path.GetFileName(controllerPath);
                var name = controllerName.Substring(0, controllerName.LastIndexOf("Controller"));
                var dir = Path.GetDirectoryName(controllerPath);
                var viewpath = Path.Combine(Directory.GetParent(dir).FullName, "Views", name);



                var a = Directory.GetFiles(viewpath);
                if (!Directory.GetFiles(viewpath).Any())
                {
                    return views;
                }
                views = Directory.GetFiles(viewpath).Select(x =>
               new FileModel
               {
                   Name = Path.GetFileName(x),
                   Path = x
               }).ToList();
            }
            catch (Exception)
            {

                return views;
            }

            return views;

        }

        public static List<string> FindFilePaths(string rootDirectoryPath, string fileName)
        {
            return Directory.GetFiles(rootDirectoryPath, fileName, SearchOption.AllDirectories).ToList();
            
        }

       
    }
}