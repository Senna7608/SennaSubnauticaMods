using Common.Helpers;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
#pragma warning disable CS1591

namespace Common
{
    public class CommandRoot
    {
        public GameObject gameObject;

        private List<ConsoleCommand> commands = new List<ConsoleCommand>();

        public CommandRoot(string name, bool indestructible = false)
        {
            gameObject = new GameObject(name);

            if (indestructible)
            {
                gameObject.AddComponent<Indestructible>();
            }

            SceneManager.sceneLoaded += new UnityAction<Scene, LoadSceneMode>(OnSceneLoaded);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            bool startScreen = scene.name == "StartScreen" ? true : false;
            bool mainScreen = scene.name == "Main" ? true : false;

            foreach (ConsoleCommand command in commands)
            {
                if (startScreen)
                {
                    if (command.AvailableInStartScreen)
                    {
                        command.RegisterCommand();                        
                    }
                }

                if (mainScreen)
                {
                    if (command.AvailableInGame)
                    {
                        command.RegisterCommand();                        
                    }
                }
            }
        }

        public void AddCommand<T>() where T : ConsoleCommand
        {
            ConsoleCommand command = gameObject.AddComponent<T>();

            if (command != null)
            {
                commands.Add(command);
            }
            else
            {
                throw new Exception("Requested console command cannot add!");
            }            
        }        
    }
}
