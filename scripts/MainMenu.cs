using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace SG {
    public class MainMenu : MonoBehaviour
    {
        public int previousScene = 0;

        //[SerializeField] 
        public static int colorPlayer1; // this must be shared across all the instances between scenes
        //[SerializeField]
        public static int colorPlayer2;

        private void Start()
        {
            //Debug.Log(getColorP1());
            //Debug.Log(getColorP2());
        }

        public int getColorP1() {
            return colorPlayer1;
        }

        public int getColorP2()
        {
            return colorPlayer2;
        }

        public void setColorP1(int toSet) {
            colorPlayer1 = toSet;
        }

        public void setColorP2(int toSet)
        {
            colorPlayer2 = toSet;
        }

        public void selectColor() {
            previousScene = 0;
            SceneManager.LoadScene("Color Menu");
        }

        public void selectStage() {
            previousScene = 1;
            SceneManager.LoadScene("Stage Menu");
        }

        public void vesselStage() {
            previousScene = 2;
            //SceneManager.LoadScene("Vessel Stage");
            SceneManager.LoadScene(4);
        }

        public void pondStage()
        {
            previousScene = 2;
            //SceneManager.LoadScene("Pond Stage");
            SceneManager.LoadScene(3);
        }

        public void quitGame() {
            Application.Quit();
        }

        public void goBack() {



            //string stageName = "Main Menu";

            //switch (previousScene) {
            //    case 1:
            //        stageName = "Color Menu";
            //        break;
            //    case 2:
            //stageName = "Stage Menu";
            //break;
            //default:
            //stageName = "Main Menu";
            //break;
            //}
            //SceneManager.LoadScene(stageName);

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }

        public void goToFirst() {
            SceneManager.LoadScene(0);
        }
    }
}

