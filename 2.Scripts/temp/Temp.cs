using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

namespace _2.Scripts.temp
{
    public class Temp
    {
       // State
       public enum State
       {
           Idle,
           Aim,
           Run
       }

       private State myState = State.Idle;

       public State MyState
       {
           get
           {
               return myState;
           }
           set
           {
               myState = value;
               OnStateChangedHandler?.Invoke(myState);
           }
       }

       // PlayerInput
       public delegate void OnStateChanged(State state);
       public event OnStateChanged OnStateChangedHandler = null;

       private void Update()
       {
           if (Input.GetKeyDown(KeyCode.A))
           {
               MyState = State.Aim;
           }
       }
           

       // Behaviour

       private interface IBehaviour
       {
           public void Execute();
       }
       
       private Dictionary<string, IBehaviour> dic = new Dictionary<string, IBehaviour>();
       
       // PlayerController
       internal void Initialize()
       {
           foreach (var state in Enum.GetNames(typeof(State)))
           {
               dic[state] = GetModule<IBehaviour>(state);
           }
            
           // Event 
           OnStateChangedHandler += UpdateState;
       }

       private void UpdateState(State state)
       {
           dic[state.ToString()].Execute();
       }

       internal static T GetModule<T>(string moduleName)
       {
           var className = typeof(T) + "Behaviour";
           var type = Type.GetType(className);

           
           var instance = Activator.CreateInstance(type);
           var module = (T)instance;

           return module;
       }
       
       // 다른 폴더링
       public class AimBehaviour : IBehaviour
       {
           public void Execute()
           {
               // todo.. 화면을 줌 떙기는 기능
           }
       }

       public class RunBehaviour : IBehaviour
       {
           public void Execute()
           {
               
           }
       }
    }
}