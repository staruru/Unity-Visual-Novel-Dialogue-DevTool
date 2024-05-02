using Codice.CM.SEIDInfo;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using System;

namespace DS.Elements
{
    using Data.Save;
    using Enumerations;
    using Utilities;
    using Windows;

    public class DSSingleChoiceNode : DSNode
    {
        public override void Initialize(string nodeName, string characterName, DSGraphView dSGraphView, Vector2 position)
        {
            base.Initialize(nodeName, characterName, dSGraphView, position);

            DialogueType = DSDialogueType.SingleChoice;

            DSChoiceSaveData choiceSaveData = new DSChoiceSaveData()
            {
                Text = "Next Dialogue"
            };

            Choices.Add(choiceSaveData);
        }

        public override void Draw()
        {
            base.Draw();

            /* OUTPUT CONTINAER */

            foreach (DSChoiceSaveData choice in Choices)
            {
                Port choicePort = this.CreatePort(choice.Text);

                choicePort.userData = choice;
                choicePort.portName = this.name;

                outputContainer.Add(choicePort);
            }

            RefreshExpandedState();
        }

    }
}