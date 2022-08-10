using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Editor
{
    [CustomPropertyDrawer(typeof(FloatRangeSliderAttribute))]

    public class FloatRangeSliderDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            base.OnGUI(position, property, label);
            var originalIndentLevel = EditorGUI.indentLevel;
            EditorGUI.BeginProperty(position, label, property);
            
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            EditorGUI.indentLevel = 0;

            SerializedProperty minProperty = property.FindPropertyRelative("_min");
            SerializedProperty maxProperty = property.FindPropertyRelative("_max");
            var minValue = minProperty.floatValue;
            var maxValue = maxProperty.floatValue;

            var fieldWidth = position.width / 4f - 4f;
            float sliderWidth = position.width / 2f;

            position.width = fieldWidth;
            minValue = EditorGUI.FloatField(position, minValue);
            position.x += fieldWidth + 4f;
            position.width = sliderWidth;

            var limit = attribute as FloatRangeSliderAttribute;
            EditorGUI.MinMaxSlider(position, ref minValue, ref maxValue, limit.Min, limit.Max);
            position.x += sliderWidth + 4f;
            position.width = fieldWidth;
            maxValue = EditorGUI.FloatField(position, maxValue);
            if (minValue < limit.Min)
            {
                minValue = limit.Min;
            }

            if (maxValue > limit.Max)
            {
                maxValue = limit.Max;
            }
            else if (maxValue < minValue)
            {
                maxValue = minValue;
            }

            minProperty.floatValue = minValue;
            maxProperty.floatValue = maxValue;
            EditorGUI.EndProperty();
            EditorGUI.indentLevel = originalIndentLevel;
        }   
    }
}
