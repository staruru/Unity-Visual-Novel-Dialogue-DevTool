using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


namespace DS.Elements
{
    public class DSGroup : Group
    {
        public string ID { get; set; }
        public string oldTitle;

        private Color _defualtBorderColor;
        private float _defaultBorderWidth;

        public DSGroup(string groupTitle, Vector2 position)
        {
            ID = Guid.NewGuid().ToString();
            title = groupTitle;
            oldTitle = groupTitle;

            SetPosition(new Rect(position, Vector2.zero));

            _defualtBorderColor = contentContainer.style.borderBottomColor.value;
            _defaultBorderWidth = contentContainer.style.borderBottomWidth.value;
        }

        public void SetErrorStyle(Color color)
        {
            contentContainer.style.borderBottomColor = color;
            contentContainer.style.borderBottomWidth = 2f;
        }

        public void ResetStyle()
        {
            contentContainer.style.borderBottomColor = _defualtBorderColor; ;
            contentContainer.style.borderBottomWidth = _defaultBorderWidth;
        }
    }
}