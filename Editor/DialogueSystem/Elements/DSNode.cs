using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DS.Elements
{
    using System.Security.Cryptography;
    using Data.Save;
    using Enumerations;
    using UnityEditor.UIElements;
    using Utilities;
    using Windows;
    using Object = UnityEngine.Object;

    public class DSNode : Node
    {
        public string ID { get; set; }
        public string DialogueName { get; set; }
        public string SpeakerName { get; set; }
        public string SpriteName { get; set; }
        public List<DSChoiceSaveData> Choices { get; set; }
        public string Text { get; set; }
        public Sprite CharacterSprite { get; set; }
        public DSDialogueType DialogueType { get; set; }
        public DSGroup Group { get; set; }

        protected DSGraphView _graphView;

        private Color _defualtBackgroundColor;

        public virtual void Initialize(string nodeName, string characterName, DSGraphView dSGraphView, Vector2 position)
        {
            ID = Guid.NewGuid().ToString();
            DialogueName = nodeName;
            SpeakerName = "Speaker name";
            SpriteName = characterName;
            Choices = new List<DSChoiceSaveData>();
            Text = "Dialouge text";
            CharacterSprite = null;

            _graphView = dSGraphView;
            _defualtBackgroundColor = new Color(29F / 255f, 29 / 255f, 30 / 255f);

            SetPosition(new Rect(position, Vector2.zero));

            mainContainer.AddToClassList("ds-node__main-container");
            extensionContainer.AddToClassList("ds-node__extension-container");
        }

        public virtual void Draw()
        {
            /* TITLE CONTAINER */

            TextField dialogueNameTextField = DSElementUtility.CreateTextField(DialogueName, null, callback =>
            {
                TextField target = (TextField)callback.target;

                target.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();

                if (string.IsNullOrEmpty(target.value))
                {
                    if (!string.IsNullOrEmpty(DialogueName))
                    {
                        ++_graphView.NameErrorsAmount;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(DialogueName))
                        {
                            --_graphView.NameErrorsAmount;
                        }
                    }
                }

                if (Group == null)
                {
                    _graphView.RemoveUngroupedNode(this);
                    DialogueName = target.value;
                    _graphView.AddUngroupedNode(this);

                    return;
                }

                DSGroup currentGroup = Group;

                _graphView.RemoveGroupedNode(this, Group);

                DialogueName = callback?.newValue;

                _graphView.AddGroupedNode(this, currentGroup);
            });

            dialogueNameTextField.AddClasses(
                    "ds-node__text-field",
                    "ds-node__text-field__hidden",
                    "ds-node__filename-text-field"
                );

            titleContainer.Insert(0, dialogueNameTextField);

            /* INPUT CONTAINER */

            Port inputPort = this.CreatePort("Dialogue Connection", Orientation.Horizontal,
                Direction.Input, Port.Capacity.Multi);

            inputPort.portName = "Dialogue Connection";

            inputContainer.Add(inputPort);

            /* EXTENSIONS CONTAINER */

            VisualElement customDataContainer = new VisualElement();

            customDataContainer.AddToClassList("ds-node__custom-data-container");

            TextField speakerField = DSElementUtility.CreateSpeakerArea(SpeakerName, null, callback =>
            {
                SpeakerName = callback.newValue;
            });

            Foldout textFoldout = DSElementUtility.CreateFoldout("Dialogue Text");

            ObjectField characterSprite = DSElementUtility.CreateSprite(CharacterSprite, null, callback =>
            {
                CharacterSprite = (Sprite)callback.newValue;

            });

            TextField textTextField = DSElementUtility.CreateTextArea(Text, null, callback =>
            {
                Text = callback.newValue;
            });

            speakerField.AddClasses(
                "ds-node__text-field",
                "ds-node__text-field__hidden",
                "ds-node__speaker-text-field"
            );

            customDataContainer.Add(speakerField);
            

            customDataContainer.Add(characterSprite);

            textTextField.AddClasses(
                    "ds-node__text-field",
                    "ds-node__quote-text-field"
            );

            textFoldout.Add(textTextField);
            customDataContainer.Add(textFoldout);
            extensionContainer.Add(customDataContainer);
        }

        #region Overrided Methods
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("Disconnect Input Ports", actionEvent => DisconnectInputPorts());
            evt.menu.AppendAction("Disconnect Input Ports", actionEvent => DisconnectOutputPorts());

            base.BuildContextualMenu(evt);
        }
        #endregion

        #region Utility Methods
        public void DisconnetAllPorts()
        {
            DisconnectPorts(inputContainer);
            DisconnectPorts(outputContainer);
        }

        private void DisconnectInputPorts()
        {
            DisconnectPorts(inputContainer);
        }

        private void DisconnectOutputPorts()
        {
            DisconnectPorts(outputContainer);
        }

        private void DisconnectPorts(VisualElement container)
        {
            foreach (Port port in container.Children())
            {
                if (!port.connected) continue;

                _graphView.DeleteElements(port.connections);
            }
        }

        public bool IsStartingNode()
        {
            Port importPort = (Port)inputContainer.Children().First();

            return importPort.connected;
        }

        public void SetErrorStyle(Color color)
        {
            mainContainer.style.backgroundColor = color;
        }

        public void ResetStyle()
        {
            mainContainer.style.backgroundColor = _defualtBackgroundColor;
        }
        #endregion
    }
}