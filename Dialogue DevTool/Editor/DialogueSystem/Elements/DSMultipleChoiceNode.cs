using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DS.Elements
{
    using Data.Save;
    using Enumerations;
    using Utilities;
    using Windows;

    public class DSMultipleChoiceNode : DSNode
    {
        public override void Initialize(string nodeName, string characterName, DSGraphView dSGraphView, Vector2 position)
        {
            base.Initialize(nodeName,characterName, dSGraphView, position);

            DialogueType = DSDialogueType.MultipleChoice;

            DSChoiceSaveData choiceSaveData = new DSChoiceSaveData()
            {
                Text = "New Choice"
            };

            Choices.Add(choiceSaveData);
        }

        public override void Draw()
        {
            base.Draw();

            /* MAIN CONTINAER */

            Button addChoiceButton = DSElementUtility.CreateButton("Add Choice", () =>
            {
                DSChoiceSaveData choiceSaveData = new DSChoiceSaveData()
                {
                    Text = "New Choice"
                };

                Choices.Add(choiceSaveData);

                Port choicePort = CreateChoicePort(choiceSaveData);

                Choices.Add(choiceSaveData);

                outputContainer.Add(choicePort);
            });

            addChoiceButton.AddToClassList("ds-node__button");

            mainContainer.Insert(1, addChoiceButton);

            /* OUTPUT CONTINAER */

            foreach (DSChoiceSaveData choice in Choices)
            {
                Port choicePort = CreateChoicePort(choice);

                choicePort.portName = "";

                outputContainer.Add(choicePort);
            }

            RefreshExpandedState();
        }

        #region Elements Creation
        private Port CreateChoicePort(object userData)
        {
            Port choicePort = this.CreatePort();

            choicePort.userData = userData;

            DSChoiceSaveData choiceData = (DSChoiceSaveData)userData;

            Button deleteChoiceButton = DSElementUtility.CreateButton("X", () =>
                {
                    if (Choices.Count == 1)
                        return;

                    if (choicePort.connected)
                        _graphView.DeleteElements(choicePort.connections);

                    Choices.Remove(choiceData);

                    _graphView.RemoveElement(choicePort);
                });

            deleteChoiceButton.AddToClassList("ds-node__button");

            TextField choiceTextField = DSElementUtility.CreateTextField(choiceData.Text, null, callback =>
            {
                choiceData.Text = callback.newValue;
            });

            //choiceTextField.style.flexDirection = FlexDirection.Column;

            choiceTextField.AddClasses(
                    "ds-node__text-field",
                    "ds-node__text-field__hidden",
                    "ds-node__choice-text-field"
             );

            choicePort.Add(choiceTextField);
            choicePort.Add(deleteChoiceButton);

            return choicePort;
        }
        #endregion
    }
}