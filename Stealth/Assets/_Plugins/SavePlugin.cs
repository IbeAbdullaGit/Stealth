using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;
using System.IO;
public class SavePlugin : MonoBehaviour
{
    [DllImport("Plugin")]
    private static extern int GetID();

    [DllImport("Plugin")]
    private static extern void SetID(int id);

      [DllImport("Plugin")]
      private static extern void SetPosition(float x, float y, float z);

       [DllImport("Plugin")]
       private static extern void SaveToFile(string s);
       [DllImport("Plugin")]
        private static extern void SaveIdToFile(int id);

        [DllImport("Plugin")]
        private static extern void StartWriting(string fileName);

         [DllImport("Plugin")]
        private static extern void LoadFromFile(string fileName);

        [DllImport("Plugin")]
        private static extern void EndWriting();

        //[DllImport("Plugin.dll", EntryPoint = "?LoadFromFile@GameObject@@QEAA?AUVector3@@PEBD@Z")]
       

    PlayerAction inputAction;
    string m_Path;
    private string fn;

    EditorManager editor;

    void Start() {
        //m_Path = Application.persistentDataPath;
         m_Path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments).Replace("\\", "/");

        //Directory.CreateDirectory(m_Path+"/Celbreak");
        //m_Path = "Saves";
        fn = m_Path +"/My Games/celbreak_save_data.json";
        //fn = Path.Combine(Application.persistentDataPath, "save_data.json");
        //fn = "save_data.json";
        /* if (!Directory.Exists(fn))
        {
            Directory.CreateDirectory(fn);
        } */
        Debug.Log(fn);
        //if (!File.Exists(fn))
           // File.Create(fn);
        

    }

    public void SaveItems()
    {
        Debug.Log("Start Saving");
        //if (editor.editorMode)
        {
            StartWriting(fn);
            //we are only saving the last level
            //var player = GameObject.FindGameObjectWithTag("Player");

            //Debug.Log(SceneManager.GetActiveScene().name);
            
            SaveIdToFile(SceneManager.GetActiveScene().buildIndex);
            EndWriting();

            Debug.Log("Saved");

        }
    }
    public void LoadFile()
    {
        Debug.Log("Start loading");
        //check if data path exists
        if (File.Exists(fn))
        {
            StartWriting(fn);
            //we are only saving the last level
            //var player = GameObject.FindGameObjectWithTag("Player");
           //do we have anything saved to start with?
            {
                LoadFromFile(fn);
                 int id = GetID();
                if (id !=0)
                //access game manaer
                {
                    GetComponent<SwitchScene>().ChangeScene(id);
                    //Debug.Log(id);
                    Debug.Log("Loaded");
                }
                else{
                    Debug.Log("Can't load right now");
                }
                
            }
            EndWriting();
           
        }
          else{
            Debug.Log("Can't load");
        }  
        
    }

}
