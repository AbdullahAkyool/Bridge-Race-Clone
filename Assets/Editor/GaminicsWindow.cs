using UnityEngine;
using UnityEditor;


public class GaminicsWindow : EditorWindow
{

    [MenuItem("Gaminics/Gaminics Panel")]
    public static void OpenGaminicsPanel(){

        GaminicsWindow gaminicsWindow = GetWindow<GaminicsWindow>();
        Texture icon = AssetDatabase.LoadAssetAtPath<Texture> ("Assets/Editor/Textures/GaminicsLogo_Plane.png");     
        GUIContent titleContent = new GUIContent ("Gaminics", icon);
        gaminicsWindow.titleContent = titleContent;
        
    }

    [SerializeField] private Texture _gaminicsLogo;
    private void OnGUI() {

        GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
        
        
        GUILayout.BeginHorizontal();
        GUIStyle gaminicsLogoStyle = new GUIStyle();
        gaminicsLogoStyle.alignment = TextAnchor.UpperCenter;
        gaminicsLogoStyle.fixedWidth = 256f;
        gaminicsLogoStyle.fixedHeight = 128f;
        gaminicsLogoStyle.imagePosition = ImagePosition.ImageAbove;
        
        GUIContent gaminicsLogoContent = new GUIContent("Gaminics® 2022 All Rights Reserved.",_gaminicsLogo);
        GUILayout.Label(gaminicsLogoContent,gaminicsLogoStyle);
        GUILayout.EndHorizontal();

        GUIContent githubButtonContent = new GUIContent("Github",AssetDatabase.LoadAssetAtPath<Texture> ("Assets/Editor/Textures/Github.png"));
        GUIContent driveButtonContent = new GUIContent("Drive",AssetDatabase.LoadAssetAtPath<Texture> ("Assets/Editor/Textures/Drive.png"));
        GUIContent webButtonContent = new GUIContent("gaminics.com",AssetDatabase.LoadAssetAtPath<Texture> ("Assets/Editor/Textures/Web.png"));
        GUIContent milanoteButtonContent = new GUIContent("Milanote",AssetDatabase.LoadAssetAtPath<Texture> ("Assets/Editor/Textures/Milanote.png"));

        
        GUILayout.BeginVertical();
        
        GUILayout.BeginHorizontal();

        if(GUILayout.Button(githubButtonContent,GUILayout.Height(60),GUILayout.Width(160))){
            System.Diagnostics.Process.Start("https://github.com/gaminics/GaminicsUtils");
        }
        
        if(GUILayout.Button(driveButtonContent,GUILayout.Height(60),GUILayout.Width(160))){
            System.Diagnostics.Process.Start("https://drive.google.com/drive/folders/17NXAD6mUdcx_1Yli9uotAYLGvvSl-rl9");
        }

        GUILayout.EndHorizontal();


        GUILayout.BeginHorizontal();
        
        if(GUILayout.Button(webButtonContent,GUILayout.Height(60),GUILayout.Width(160))){
            System.Diagnostics.Process.Start("https://www.gaminics.com/");
        }
        
        if(GUILayout.Button(milanoteButtonContent,GUILayout.Height(60),GUILayout.Width(160))){
            System.Diagnostics.Process.Start("https://app.milanote.com/1MavGr1jn7w8c6/gaminics-inc?p=FMcLDsXQss4");
        }

        GUILayout.EndHorizontal();

        GUILayout.EndVertical();

        GUILayout.EndArea();

    }
}
