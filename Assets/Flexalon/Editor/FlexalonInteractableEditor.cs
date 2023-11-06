#if UNITY_PHYSICS

using UnityEditor;
using UnityEngine;

namespace Flexalon.Editor
{
    [CustomEditor(typeof(FlexalonInteractable)), CanEditMultipleObjects]
    public class FlexalonInteractableEditor : UnityEditor.Editor
    {
        private SerializedProperty _clickable;
        private SerializedProperty _maxClickTime;
        private SerializedProperty _draggable;
        private SerializedProperty _interpolationSpeed;
        private SerializedProperty _restriction;
        private SerializedProperty _planeNormal;
        private SerializedProperty _localSpaceRestriction;
        private SerializedProperty _lineDirection;
        private SerializedProperty _holdOffset;
        private SerializedProperty _localSpaceOffset;
        private SerializedProperty _rotateOnDrag;
        private SerializedProperty _holdRotation;
        private SerializedProperty _localSpaceRotation;
        private SerializedProperty _hideCursor;
        private SerializedProperty _collider;
        private SerializedProperty _bounds;
        private SerializedProperty _layerMask;
        private SerializedProperty _clicked;
        private SerializedProperty _hoverStart;
        private SerializedProperty _hoverEnd;
        private SerializedProperty _selectStart;
        private SerializedProperty _selectEnd;
        private SerializedProperty _dragStart;
        private SerializedProperty _dragEnd;

        private static bool _showEvents = false;

        void OnEnable()
        {
            _clickable = serializedObject.FindProperty("_clickable");
            _maxClickTime = serializedObject.FindProperty("_maxClickTime");
            _draggable = serializedObject.FindProperty("_draggable");
            _interpolationSpeed = serializedObject.FindProperty("_interpolationSpeed");
            _restriction = serializedObject.FindProperty("_restriction");
            _planeNormal = serializedObject.FindProperty("_planeNormal");
            _localSpaceRestriction = serializedObject.FindProperty("_localSpaceRestriction");
            _lineDirection = serializedObject.FindProperty("_lineDirection");
            _holdOffset = serializedObject.FindProperty("_holdOffset");
            _localSpaceOffset = serializedObject.FindProperty("_localSpaceOffset");
            _rotateOnDrag = serializedObject.FindProperty("_rotateOnDrag");
            _holdRotation = serializedObject.FindProperty("_holdRotation");
            _localSpaceRotation = serializedObject.FindProperty("_localSpaceRotation");
            _hideCursor = serializedObject.FindProperty("_hideCursor");
            _collider = serializedObject.FindProperty("_collider");
            _bounds = serializedObject.FindProperty("_bounds");
            _layerMask = serializedObject.FindProperty("_layerMask");
            _clicked = serializedObject.FindProperty("_clicked");
            _hoverStart = serializedObject.FindProperty("_hoverStart");
            _hoverEnd = serializedObject.FindProperty("_hoverEnd");
            _selectStart = serializedObject.FindProperty("_selectStart");
            _selectEnd = serializedObject.FindProperty("_selectEnd");
            _dragStart = serializedObject.FindProperty("_dragStart");
            _dragEnd = serializedObject.FindProperty("_dragEnd");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(_clickable);

            if (_clickable.boolValue)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_maxClickTime"), new GUIContent("  Max Click Time"));
            }

            EditorGUILayout.PropertyField(_draggable);

            if (_draggable.boolValue)
            {
                EditorGUILayout.PropertyField(_interpolationSpeed);

                var restriction = _restriction;
                EditorGUILayout.PropertyField(restriction);
                if (restriction.enumValueIndex == (int)FlexalonInteractable.RestrictionType.Plane)
                {
                    EditorGUILayout.PropertyField(_planeNormal, new GUIContent("  Normal"));
                    EditorGUILayout.PropertyField(_localSpaceRestriction, new GUIContent("  Local Space"));
                }
                else if (restriction.enumValueIndex == (int)FlexalonInteractable.RestrictionType.Line)
                {
                    EditorGUILayout.PropertyField(_lineDirection, new GUIContent("  Direction"));
                    EditorGUILayout.PropertyField(_localSpaceRestriction, new GUIContent("  Local Space"));
                }

                EditorGUILayout.PropertyField(_holdOffset);
                EditorGUILayout.PropertyField(_localSpaceOffset, new GUIContent("  Local Space"));

                var rotateOnGrab = _rotateOnDrag;
                EditorGUILayout.PropertyField(rotateOnGrab);
                if (rotateOnGrab.boolValue)
                {
                    EditorGUILayout.PropertyField(_holdRotation, new GUIContent("  Rotation"));
                    EditorGUILayout.PropertyField(_localSpaceRotation, new GUIContent("  Local Space"));
                }

                EditorGUILayout.PropertyField(_hideCursor);

                EditorGUILayout.PropertyField(_collider);

                EditorGUILayout.PropertyField(_bounds);

                EditorGUILayout.PropertyField(_layerMask);
            }

            _showEvents = EditorGUILayout.Foldout(_showEvents, "Events");
            if (_showEvents)
            {
                if (_clickable.boolValue)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("_clicked"));
                }

                EditorGUILayout.PropertyField(serializedObject.FindProperty("_hoverStart"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_hoverEnd"));

                EditorGUILayout.PropertyField(serializedObject.FindProperty("_selectStart"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_selectEnd"));

                if (_draggable.boolValue)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("_dragStart"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("_dragEnd"));
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}

#endif