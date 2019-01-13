using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace Insight
{
    public class ButtonCallback : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public bool buttonup = false;
        public bool buttondown = false;
        public bool buttonleft = false;
        public bool buttonright = false;
        public CamInsight camin = null;
        public void Start()
        {
            camin = FindObjectOfType<CamInsight>();
        }
        public void OnPointerDown(PointerEventData pointerEventData)
        {
            if (name == "ButtonUp")
            {
                buttonup = true;
            }
            if (name == "ButtonDown")
            {
                buttondown = true;
            }
            if (name == "ButtonLeft")
            {
                buttonleft = true;
            }
            if (name == "ButtonRight")
            {
                buttonright = true;
            }            
        }
        public void OnPointerUp(PointerEventData pointerEventData)
        {
            if (name == "ButtonUp")
            {
                buttonup = false;
                camin.moviendo = false;
            }
            if (name == "ButtonDown")
            {
                buttondown = false;
                camin.moviendo = false;
            }
            if (name == "ButtonLeft")
            {
                buttonleft = false;
                camin.moviendo = false;
            }
            if (name == "ButtonRight")
            {
                buttonright = false;
                camin.moviendo = false;
            }
        }
        public void Update()
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                if(camin == null)
                {
                    camin = FindObjectOfType<CamInsight>();
                }
                if (camin != null)
                {
                    if (camin.estaactivo)
                    {
                        if (buttonup)
                        {
                            FindObjectOfType<CamInsight>().OnUp();
                        }
                        if (buttondown)
                        {
                            FindObjectOfType<CamInsight>().OnDown();
                        }
                        if (buttonleft)
                        {
                            FindObjectOfType<CamInsight>().OnLeft();
                        }
                        if (buttonright)
                        {
                            FindObjectOfType<CamInsight>().OnRight();
                        }
                    }
                }
            }
        }
    }
}
