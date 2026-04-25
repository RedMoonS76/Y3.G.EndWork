using UnityEditor;
using UnityEngine;

namespace Cainos.PixelArtPlatformer_Dungeon
{
    [CustomEditor(typeof(AddShadow))]
    public class AddShadowEditor : Editor
    {
        private AddShadow instance;

        private void OnEnable()
        {
            instance = target as AddShadow;
        }

        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Add Shadow")) instance.Add();
        }
    }
}
