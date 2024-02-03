using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.EditorTools;
using Unity.VisualScripting;

[EditorTool("Enable Draw Mode", typeof(PathEdit))]
public class Pathing : EditorTool
{
    GUIContent m_Icon;
    public override GUIContent toolbarIcon => m_Icon;

    private void OnEnable()
    {
        Texture2D ico = EditorGUIUtility.Load("Assets/Materials/Textures/Icon.png") as Texture2D;
        m_Icon = new GUIContent(){
            image = ico,
            tooltip = "Path Drawing Mode"
        };
    }

    public override void OnToolGUI(EditorWindow window)
    {
        Event e = Event.current;
        int controlID = GUIUtility.GetControlID(FocusType.Passive);
       
       if(e.type == EventType.MouseDown && e.button == 0){
            Vector2 mousePos = Event.current.mousePosition;
			mousePos.y = Camera.current.pixelHeight - mousePos.y;
			Ray worldRay = Camera.current.ScreenPointToRay(mousePos);
            RaycastHit hitInfo;
 
            if(Physics.Raycast(worldRay, out hitInfo, Mathf.Infinity)){
                Selection.activeObject.GetComponent<PathEdit>().myPath.Add(hitInfo.point);
                Selection.activeObject.GetComponent<CrawlerLogic>().myPath.Add(hitInfo.point);
            }
            
        }
        else if(e.type == EventType.Layout){
            HandleUtility.AddDefaultControl(controlID);
        }
    }
}
