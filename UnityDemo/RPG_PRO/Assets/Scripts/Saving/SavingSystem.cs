using UnityEngine;
using System.IO;
using System.Text;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;

namespace  RPG.Saving
{
    public class SavingSystem : MonoBehaviour
    {
        public void Save(string file)
        {
            // string path = GetSavingFilePath(file);
            // //print(path);

            // using( FileStream fs = File.Open(path, FileMode.Create))
            // {
            //     //第一种方案
            //     //byte[] buffer = DoSerializeVector(GetPlayerTransform().position);   //Encoding.UTF8.GetBytes("hello rpg core~!");
            //     //fs.Write(buffer, 0, buffer.Length);

            //     //第二种方案
            //     //SerializeableVector3 slVector3 = new SerializeableVector3( GetPlayerTransform().position );
            //     //BinaryFormatter binaryer = new BinaryFormatter();
            //     //binaryer.Serialize(fs, slVector3);


            //     //第三种方案
            //     BinaryFormatter binaryer = new BinaryFormatter();
            //     binaryer.Serialize(fs, CaptureState());
            // }


            //第四种方案   主要目的是跨场景保存数据
            Dictionary<string, object> states = LoadFile(file);
            CaptureState(states);
            SaveFile(file, states);
        }

        public void Load(string file)
        {
            // string path = GetSavingFilePath(file);
            // //print(path);

            // using (FileStream fs = File.Open(path, FileMode.Open))
            // {
            //     // 第一种方案
            //     // byte[] buffer = new byte[fs.Length];
            //     // fs.Read(buffer, 0, buffer.Length);
            //     // Vector3 positon =  DeSerializeVector(buffer);  //string text = Encoding.UTF8.GetString(buffer);

            //     // 第二种方案
            //     // BinaryFormatter binaryer = new BinaryFormatter();  //BinaryFormatter: 二进制格式
            //     // SerializeableVector3 slVector = (SerializeableVector3) binaryer.Deserialize(fs);
            //     // Transform playerTransForm = GetPlayerTransform();
            //     // playerTransForm.position = slVector.GetVector3(); 

            //     // 第三种方案
            //     BinaryFormatter binaryer = new BinaryFormatter();
            //     RestoreState( binaryer.Deserialize(fs) );

            // }

            //第四种方案   主要目的是跨场景保存数据
            RestoreState(LoadFile(file));
        }

        private Dictionary<string, object> LoadFile(string file)
        {
            string path = GetSavingFilePath(file);
            if(!File.Exists(path))
                return new Dictionary<string, object>();

            using (FileStream fs = File.Open(path, FileMode.Open))
            {
               BinaryFormatter binaryer = new BinaryFormatter();
               return (Dictionary<string, object>)binaryer.Deserialize(fs);
            }
        }

        private void SaveFile(string file, object state)
        {
            string path = GetSavingFilePath(file);

            using( FileStream fs = File.Open(path, FileMode.Create))
            {
                BinaryFormatter binaryer = new BinaryFormatter();
                binaryer.Serialize(fs, state);
            }
        }




        private void RestoreState(Dictionary<string,object> states)
        {
           foreach (SaveableEntity item in FindObjectsOfType<SaveableEntity>())
           {
               string id = item.GetUniqueIdentifier();
               if( states.ContainsKey(id) )
                item.RestoreState( states[id] );
           }
        }

        private void CaptureState( Dictionary<string, object> states )
        {
            foreach (SaveableEntity item in FindObjectsOfType<SaveableEntity>())
            {
                string uniqueIdentifier = item.GetUniqueIdentifier();
                states[uniqueIdentifier] = item.CaptureState();
            }
            states["lastSceneIndex"] = SceneManager.GetActiveScene().buildIndex;
           // print("scene saved index:" + SceneManager.GetActiveScene().buildIndex);
        }



        private string GetSavingFilePath(string file)
        {
            return Path.Combine(Application.persistentDataPath, file + ".sav");
        }

        private Transform GetPlayerTransform()
        {
            return GameObject.FindGameObjectWithTag("Player").transform;
        }

        /// <summary>
        /// load last scene 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public IEnumerator LoadLastScene(string file)
        {
            Dictionary<string, object> states = LoadFile(file);
            if(states.ContainsKey("lastSceneIndex"))
            {
                int lastSceneIndex = (int)states["lastSceneIndex"];
                if(lastSceneIndex != SceneManager.GetActiveScene().buildIndex)
                    yield return SceneManager.LoadSceneAsync(lastSceneIndex);
            }
            RestoreState(states);
        }














        /// <summary>
        /// ONLY FOR STUDY
        /// </summary>
        private byte[] DoSerializeVector(Vector3 vec3)
        {
            byte[] bytes = new byte[ 3 * 4 ];//要存x,y,z  一个字符占32位  就是  4个字节   8位是一个字节
            BitConverter.GetBytes(vec3.x).CopyTo(bytes, 0);//BitConverter  要用system命名空间
            BitConverter.GetBytes(vec3.y).CopyTo(bytes, 4);
            BitConverter.GetBytes(vec3.z).CopyTo(bytes, 8);
            return bytes;
        }
        /// <summary>
        /// ONLY FOR STUDY
        /// </summary>
        private Vector3 DeSerializeVector(byte[] bytes)
        {
            float x = BitConverter.ToSingle(bytes, 0);//Single ： 单精度浮点数
            float y = BitConverter.ToSingle(bytes, 4);
            float z = BitConverter.ToSingle(bytes, 8);
            return new Vector3(x,y,z);
        }



    }
    
}