using System.IO;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace DS.Windows
{
    using System;
    using Utilities;

    public class DSEditorWindow : EditorWindow
    {
        private DSGraphView _graphView;
        private string _defaultFileName = "Untitled";
        private static TextField _fileNameTextField;
        private Button _saveButton;
        private Button _clearButton;
        private Button _resetButton;
        private Button _loadButton;
        private Button _miniMapButton;

        [MenuItem("Window/DialogueSystem/Dialogue Graph")]
        public static void Open()
        {
            GetWindow<DSEditorWindow>("Dialogue Graph");
        }

        private void OnEnable()
        {
            AddGraphView();
            AddToolBar();
            AddStyles();
        }

        #region Elements Addition
        private void AddGraphView()
        {
            _graphView = new DSGraphView(this);
            _graphView.StretchToParentSize();
            rootVisualElement.Add(_graphView);
        }

        private void AddToolBar()
        {
            Toolbar toolbar = new Toolbar();

            _fileNameTextField = DSElementUtility.CreateTextField(_defaultFileName, "File Name: ", callback =>
            {
                _fileNameTextField.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();
            });

            _loadButton = DSElementUtility.CreateButton("Load", () => Load());
            _saveButton = DSElementUtility.CreateButton("Save", () => Save());

            _clearButton = DSElementUtility.CreateButton("Clear", () => Clear());
            _resetButton = DSElementUtility.CreateButton("Reset", () => ResetGraph());
            _miniMapButton = DSElementUtility.CreateButton("Mini Map", () => ToggleMiniMap());

            toolbar.Add(_fileNameTextField);
            toolbar.Add(_saveButton);
            toolbar.Add(_loadButton);
            toolbar.Add(_clearButton);
            toolbar.Add(_resetButton);
            toolbar.Add(_miniMapButton);

            toolbar.AddStyleSheets("Assets/Dialogue DevTool/Editor Default Resources/DialogueSystem/DSToolbarStyles.uss");

            rootVisualElement.Add(toolbar);
        }

        private void AddStyles()
        {
            rootVisualElement.AddStyleSheets("Assets/Dialogue DevTool/Editor Default Resources/DialogueSystem/DSVariables.uss");
        }
        #endregion

        #region Toolbar Actions
        private void Load()
        {
            string filePath = EditorUtility.OpenFilePanel("Dialogue Graphs", "Assets/Editor/DialougeSystem/Graphs", "asset");

            if (string.IsNullOrEmpty(filePath))
                return;

            Clear();

            DSIOUtility.Initialize(_graphView, Path.GetFileNameWithoutExtension(filePath));
            DSIOUtility.Load();
        }

        private void Save()
        {
            if (string.IsNullOrEmpty(_fileNameTextField.value))
            {
                EditorUtility.DisplayDialog(
                    "Invaild file name.",
                    "Please ensure the file name you've typed in is valid",
                    "Ok."
                );

                return;
            }

            DSIOUtility.Initialize(_graphView, _fileNameTextField.value);
            DSIOUtility.Save();
        }

        private void Clear()
        {
            _graphView.ClearGraph();
        }

        private void ResetGraph()
        {
            Clear();
            UpdateFileName(_defaultFileName);
        }

        private void ToggleMiniMap()
        {
            _graphView.ToggleMiniMap();

            _miniMapButton.ToggleInClassList(".ds-toolbar__button__selected");
        }
        #endregion

        #region Utility Methods
        public static void UpdateFileName(string newFileName)
        {
            _fileNameTextField.value = newFileName;
        }

        public void EnableSaving()
        {
            _saveButton.SetEnabled(true);
        }

        public void DisableSaving()
        {
            _saveButton.SetEnabled(false);
        }
        #endregion
    }
}