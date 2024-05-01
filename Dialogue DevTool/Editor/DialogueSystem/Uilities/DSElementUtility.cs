using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace DS.Utilities
{
    using Elements;
    using UnityEditor;

    public static class DSElementUtility
    {
        public static Button CreateButton(string text, Action onClick = null)
        {
            Button button = new Button(onClick)
            {
                text = text
            };

            return button;
        }

        public static Foldout CreateFoldout(string title, bool collapsed = false)
        {
            Foldout foldout = new Foldout()
            {
                text = title,
                value = !collapsed
            };

            return foldout;
        }

        public static Port CreatePort(this DSNode node, string portName = "", Orientation orientation = Orientation.Horizontal,
            Direction direction = Direction.Output, Port.Capacity capacity = Port.Capacity.Single)
        {
            Port port = node.InstantiatePort(orientation, direction, capacity, typeof(bool));

            port.name = portName;

            return port;
        }

        public static TextField CreateTextField(string value = null, string label = null,
                EventCallback<ChangeEvent<string>> onValueChanged = null)
        {
            TextField textField = new TextField()
            {
                value = value,
                label = label
            };

            if (onValueChanged != null)
            {
                textField.RegisterValueChangedCallback(onValueChanged);
            }
            return textField;
        }

        public static TextField CreateTextArea(string value = null, string label = null,
            EventCallback<ChangeEvent<string>> onValueChanged = null)
        {
            TextField textarea = CreateTextField(value, label, onValueChanged);

            textarea.multiline = true;

            return textarea;
        }

        public static TextField CreateSpeakerArea(string value = null, string label = null,
            EventCallback<ChangeEvent<string>> onValueChanged = null)
        {
            TextField speakerField = new TextField()
            {
                value = value,
                label = label
            };

            if (onValueChanged != null)
                speakerField.RegisterValueChangedCallback(onValueChanged);

            return speakerField;
        }

        public static ObjectField CreateSprite(Sprite sprite, string label, EventCallback<ChangeEvent<Object>> onValueChanged = null)
        {
            ObjectField character = new ObjectField()
            {
                value = sprite,
                label = label,
                objectType = typeof(Sprite),
                allowSceneObjects = false
            };

            if (onValueChanged != null)
            {
                character.RegisterValueChangedCallback(onValueChanged);
            }

            return character;
        }

        public static TextField CreateSpriteName(string value = null, string label = null,
            EventCallback<ChangeEvent<string>> onValueChanged = null)
        {
            TextField spriteNameField = new TextField()
            {
                value = value, 
                label = label,
            }; 

            if (onValueChanged != null)
            {
                spriteNameField.RegisterValueChangedCallback(onValueChanged);
            }

            return spriteNameField;
        }
    }
}