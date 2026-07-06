using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Unity.AI.MCP.Editor.Helpers;
using Unity.AI.MCP.Editor.ToolRegistry;
using UnityEngine.UIElements;

namespace TestLab.AI.Tools
{
    public static class ConvertBuiltinMaterialsTool
    {
        public const string ToolName = "Unity.ConvertBuiltinMaterials";
        public const string Description =
            "Converts Built-in Render Pipeline materials to the active Scriptable Render Pipeline (URP or HDRP).";

        [McpTool(ToolName, Description, Groups = new [] { "assets", "rendering" }, EnabledByDefault = true)]
        public static object Handle()
        {
            const string menuPath =
                "Edit/Rendering/Materials/Convert All Built-in Materials to Current SRP";
            bool executed = EditorApplication.ExecuteMenuItem(menuPath);
            return new
            {
                success = executed,
                message = executed
                    ? $"[{menuPath}] Successfully converted all built-in materials to current SRP"
                    : $"[{menuPath}] Failed to convert all built-in materials to current SRP",
            };
        }
    }
}
