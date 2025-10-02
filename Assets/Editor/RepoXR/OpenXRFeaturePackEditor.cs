using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using RepoXR.Data;
using UnityEditor;
using UnityEditor.XR.OpenXR.Features;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.XR.OpenXR.Features;

namespace Editor.RepoXR
{
    internal static class FeaturesHelper
    {
        public struct FeatureInfo
        {
            public string PluginPath;
            public OpenXRFeatureAttribute Attribute;
            public OpenXRFeature Feature;
        }

        private static OpenXRFeature[] features = {};

        public static List<FeatureInfo> GetAllFeatureInfo()
        {
            var ret = new List<FeatureInfo>();

            var currentExts = new Dictionary<OpenXRFeatureAttribute, OpenXRFeature>();
            foreach (var ext in features.Where(ext => ext != null))
            {
                foreach (var attr in Attribute.GetCustomAttributes(ext.GetType()))
                {
                    if (attr is not OpenXRFeatureAttribute xrAttr) continue;

                    currentExts[xrAttr] = ext;
                    break;
                }
            }

            var all = new List<OpenXRFeature>();
            foreach (var extType in TypeCache.GetTypesWithAttribute<OpenXRFeatureAttribute>())
            {
                foreach (var attr in Attribute.GetCustomAttributes(extType))
                {
                    if (attr is not OpenXRFeatureAttribute extAttr) continue;
                    if (!IsInteractionExtension(extAttr.Category)) continue;

                    if (!currentExts.TryGetValue(extAttr, out var extObj))
                    {
                        extObj = (OpenXRFeature)ScriptableObject.CreateInstance(extType);
                        extObj.name = extType.Name;
                    }

                    if (extObj == null)
                        continue;

                    var enabled = extObj.enabled;
                    var ms = MonoScript.FromScriptableObject(extObj);
                    var path = AssetDatabase.GetAssetPath(ms);

                    var dir = "";
                    if (!string.IsNullOrEmpty(path))
                        dir = Path.GetDirectoryName(path);

                    ret.Add(new FeatureInfo
                    {
                        PluginPath = dir,
                        Attribute = extAttr,
                        Feature = extObj
                    });
                    all.Add(extObj);
                }
            }

            features = all.OrderBy(f => f.name).ToArray();

            foreach (var feature in features)
            {
                if (feature.GetField<bool>("internalFieldsUpdated"))
                    continue;

                feature.SetField("internalFieldsUpdated", true);

                foreach (var attr in feature.GetType().GetCustomAttributes<OpenXRFeatureAttribute>())
                {
                    feature.SetField("nameUi", attr.UiName);
                    feature.SetField("version", attr.Version);
                    feature.SetField("featureIdInternal", attr.FeatureId);
                    feature.SetField("openxrExtensionStrings", attr.OpenxrExtensionStrings);
                    feature.SetField("priority", attr.Priority);
                    feature.SetField("required", attr.Required);
                    feature.SetField("company", attr.Company);
                }
            }

            return ret;
        }

        private static bool IsInteractionExtension(string extensionCategoryString)
        {
            return string.CompareOrdinal(extensionCategoryString, FeatureCategory.Interaction) == 0;
        }

        public static T GetField<T>(this OpenXRFeature feature, string fieldName)
        {
            return (T)typeof(OpenXRFeature)
                .GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)!
                .GetValue(feature);
        }

        private static void SetField<T>(this OpenXRFeature feature, string fieldName, T fieldValue)
        {
            typeof(OpenXRFeature)
                .GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)!
                .SetValue(feature, fieldValue);
        }
    }

    internal class ChildListItem
    {
        public GUIContent uiName;
        public GUIContent documentationIcon;
        public string documentationLink;
        public OpenXRFeature feature;
        public string featureId;
    }

    [CustomEditor(typeof(OpenXRFeaturePack))]
    internal class OpenXRFeaturePackEditor : UnityEditor.Editor
    {
        static class Content
        {
            public static readonly GUIContent k_HelpIcon = EditorGUIUtility.IconContent("_Help");

            public static readonly GUIContent k_InteractionProfilesTitle = new("Enabled Interaction Profiles");
        }

        private Dictionary<string, ChildListItem> interactionItems = new();
        private List<string> selectedFeatureIds = new();
        private ReorderableList interactionFeaturesList;

        private bool mustInitializeFeatures = true;

        private void OnEnable()
        {
            SetupInteractionListUI();
        }

        (Rect, Rect) TakeFromFrontOfRect(Rect rect, float width)
        {
            var newRect = new Rect(rect);
            newRect.x = rect.x + 5;
            newRect.width = width;
            rect.x = (newRect.x + newRect.width) + 1;
            rect.width -= width + 1;
            return (newRect, rect);
        }

        private void SetupInteractionListUI()
        {
            if (interactionFeaturesList != null)
                return;

            var featuresProp = serializedObject.FindProperty("features");

            interactionFeaturesList =
                new ReorderableList(selectedFeatureIds, typeof(ChildListItem), false, true, true, true);

            interactionFeaturesList.drawHeaderCallback = (rect) =>
            {
                var labelSize = EditorStyles.label.CalcSize(Content.k_InteractionProfilesTitle);
                var labelRect = new Rect(rect)
                {
                    width = labelSize.x
                };

                EditorGUI.LabelField(labelRect, Content.k_InteractionProfilesTitle, EditorStyles.label);
            };

            interactionFeaturesList.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                Rect fieldRect;
                var featureId = selectedFeatureIds[index];
                var item = interactionItems[featureId];
                var labelSize = EditorStyles.label.CalcSize(item.uiName);
                (fieldRect, rect) = TakeFromFrontOfRect(rect, labelSize.x);
                EditorGUI.LabelField(fieldRect, item.uiName, EditorStyles.label);

                if (!string.IsNullOrEmpty(item.documentationLink))
                {
                    var size = EditorStyles.label.CalcSize(item.documentationIcon);
                    (fieldRect, _) = TakeFromFrontOfRect(rect, size.x);
                    if (GUI.Button(fieldRect, item.documentationIcon, EditorStyles.label))
                        Application.OpenURL(item.documentationLink);
                }
            };

            interactionFeaturesList.onAddDropdownCallback = (rect, list) =>
            {
                var menu = new GenericMenu();
                foreach (var kvp in interactionItems)
                {
                    if (selectedFeatureIds.IndexOf(kvp.Key) == -1)
                    {
                        menu.AddItem(kvp.Value.uiName, false, obj =>
                        {
                            var featureId = obj as string;
                            if (!string.IsNullOrEmpty(featureId))
                            {
                                selectedFeatureIds.Add(featureId);

                                var interactionItem = interactionItems[featureId];
                                interactionItem.feature.enabled = true;

                                AssetDatabase.AddObjectToAsset(interactionItem.feature, serializedObject.targetObject);

                                var idx = featuresProp.arraySize;
                                featuresProp.InsertArrayElementAtIndex(idx);
                                featuresProp.GetArrayElementAtIndex(idx).objectReferenceValue = interactionItem.feature;

                                serializedObject.ApplyModifiedProperties();
                                EditorUtility.SetDirty(serializedObject.targetObject);
                                AssetDatabase.SaveAssets();
                            }
                        }, kvp.Key);
                    }
                }
                menu.DropDown(rect);
            };

            interactionFeaturesList.onRemoveCallback = (list) =>
            {
                var featureId = selectedFeatureIds[list.index];
                var interactionItem = interactionItems[featureId];

                interactionItem.feature.enabled = false;
                selectedFeatureIds.RemoveAt(list.index);

                OpenXRFeature feature = null;

                for (var i = 0; i < featuresProp.arraySize; i++)
                {
                    var element = featuresProp.GetArrayElementAtIndex(i);
                    feature = (OpenXRFeature)element.objectReferenceValue;
                    if (feature.GetField<string>("featureIdInternal") != interactionItem.featureId) continue;

                    featuresProp.DeleteArrayElementAtIndex(i);
                    break;
                }

                if (feature != null)
                    DestroyImmediate(feature, true);

                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(serializedObject.targetObject);
                AssetDatabase.SaveAssets();
            };
        }

        private void DrawInteractionList()
        {
            EditorGUILayout.Space();

            if (interactionItems.Count == 0)
                return;

            EditorGUILayout.BeginVertical();
            interactionFeaturesList.DoLayoutList();
            EditorGUILayout.EndVertical();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (mustInitializeFeatures)
            {
                InitializeFeatures();

                mustInitializeFeatures = false;
            }

            DrawInteractionList();
        }

        private void InitializeFeatures()
        {
            interactionItems.Clear();
            selectedFeatureIds.Clear();

            var features = FeaturesHelper.GetAllFeatureInfo();

            foreach (var ext in features)
            {
                if (ext.Attribute.Hidden)
                    continue;

                var listItem = new ChildListItem()
                {
                    uiName = new GUIContent(ext.Attribute.UiName),
                    documentationIcon = new GUIContent("", Content.k_HelpIcon.image, "Click for documentation"),
                    documentationLink = ext.Attribute.DocumentationLink,
                    feature = ext.Feature,
                    featureId = ext.Attribute.FeatureId
                };

                interactionItems.Add(listItem.featureId, listItem);
                listItem.feature.enabled = IsInList(listItem.featureId);
                if (listItem.feature.enabled)
                    selectedFeatureIds.Add(listItem.featureId);
            }
        }

        private bool IsInList(string featureId)
        {
            var featuresProp = serializedObject.FindProperty("features");

            for (var i = 0; i < featuresProp.arraySize; i++)
            {
                var element = featuresProp.GetArrayElementAtIndex(i);
                if (element.objectReferenceValue is OpenXRFeature f &&
                    f.GetField<string>("featureIdInternal") == featureId)
                    return true;
            }

            return false;
        }
    }
}