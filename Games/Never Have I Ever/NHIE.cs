/*
 * Never Have I Ever
 * Made by Child of the Beast
 * Version 1.4
 */
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;
using VRC.Udon.Common.Interfaces;

namespace UdonVR.Childofthebeast.NHIE
{
    public class NHIE : UdonSharpBehaviour
    {

        //Questions from
        //https://lifehacks.io/never-have-i-ever-questions/
        //https://herway.net/entertainment/650-never-have-i-ever-questions/
        public string[] QuestionsSFW = new string[50] { "shoplifted", "fainted", "hitchhiked",
        "been arrested", "gone surfing", "been electrocuted", "gotten stitches", "gone hunting",
        "gone vegan", "bungee jumped", "ridden an animal", "broken a bone", "shot a gun", "dined and dashed",
        "chipped a tooth", "gone scuba diving", "ruined someone elses vacation", "jumped from a roof", "been caught cheating on a test",
        "had a paranormal experience", "caught sneaking into a movie", "danced in an elevator", "had a treehouse",
        "worn glasses with fake lenses", "been on a fad diet", "been to a fashion show",
        "stolen something from a restaurant.", "had a bad allergic reaction", "woken up and couldnt move", "Been outside my home country",
        "been trapped in an elevator", "texted for four hours straight", "taken part in a talent show",
        "walked for more than eight hours straight", "tried to cut my own hair", "thought I was going to drown",
        "worked in fast-food", "fallen in love at first sight", "sung karaoke", "been on TV or the radio",
        "been awake for two days", "thrown up on a roller coaster", "accidentally sent someone to the hospital",
        "dyed my hair a crazy color", "had to run to save my life", "made money by performing on the street",
        "pressured someone into getting a tattoo or piercing", "had a physical fight with my best friend",
        "thrown something into a TV or computer screen", "Eaten food out the the garbage"};
        public string[] QuestionsNSFW = new string[50] { "Given a lap dance", "Received a lap dance",
        "Took pictures in my underwear", "taken a sexy selfie", "Tried skinny dipping",
        "Had a one night stand", "Kiss and told", "Had sex in public", "Kissed my best friend",
        "Accidentally sent an inappropriate text message to my mom that was intended for my girlfriend/boyfriend",
        "Slept nude", "sunbathed partially or totally nude", "Made out with a stranger",
        "Left my house without underwear", "Had a friend with benefits", "Had sex in the sea", "Had sex in a sleeping bag",
        "Lied about myself to get laid", "Made out with someone in a hot tub", "Had sex with someone 10 years older than me",
        "Worn someone elses underwear", "Shaved my partners pubic hair", "Eaten food off of someones naked body",
        "Played strip poker", "Been a friend with benefits", "Had sex with someone 5 years younger than me",
        "Flashed someone", "Been caught having sex", "Watched a friend having sex", "done body shots",
        "Asked someone to send nudes", "Had a threesome", "Had sex with someone the same day as meeting them",
        "Been locked outside naked", "Worn clothes to hide a hickey", "Slept with someone from my workplace",
        "Been approached by a hooker", "Sexted the wrong person", "Had sex in a pool", "Fantasized about someone in this room",
        "Questioned my sexuality", "Trapped someone", "Been Trapped", "had to fake an orgasm", "Had sex in a bathroom stall",
        "Flirted with a teacher", "Kissed someone without knowing their name", "Had phone sex", "Been to a nude beach",
        "Slipped my number to a waiter"};
        [UdonSynced] Vector3 SyncedVars = new Vector3(0f, 0f, 0f);
        private Vector3 _localVars;
        private float _Current;
        private float _GameType;
        private float _CurrentQuestion;

        private Text _QuestionBoard;
        private int _QuestionCountSFW;
        private int _QuestionCountNSFW;
        private GameObject _Circleobj;
        private GameObject _Classicobj;
        private GameObject _SelectMenu;
        private GameObject _GameMenu;

        [Tooltip("This tells the game how many frames to wait until trying to update the menu with a new question.")]
        public int UpdateRate = 10;
        private int _Frame;
        [Tooltip("WARNING: This will spam console with debugging every time UpdateRate is ran.")]
        public bool _Debug = false;
        private void SyncData(bool _a)
        {
            if (_a == true)
            {
                SyncedVars.x = _Current;
                SyncedVars.y = _GameType;
                SyncedVars.z = _CurrentQuestion;
                _localVars = SyncedVars;
            }
            else
            {
                if (SyncedVars == _localVars) return;
                _Current = SyncedVars.x;
                _GameType = SyncedVars.y;
                _CurrentQuestion = SyncedVars.z;
                _localVars = SyncedVars;
            }
        }

        private void Start()
        {
            _Circleobj = transform.Find("Round").gameObject;
            _Classicobj = transform.Find("Classic").gameObject;
            _SelectMenu = transform.Find("Selecting_Canvas").gameObject;
            _GameMenu = transform.Find("Playing_Canvas").gameObject;
            _QuestionBoard = _GameMenu.transform.Find("Panel/QuestionText").GetComponent<Text>();
            _QuestionCountSFW = QuestionsSFW.Length;
            _QuestionCountNSFW = QuestionsNSFW.Length;
            _SelectMenu.transform.Find("Panel/SFW_Button/Count").GetComponent<Text>().text = _QuestionCountSFW.ToString() + " Questions";
            _SelectMenu.transform.Find("Panel/NSFW_Button/Count").GetComponent<Text>().text = _QuestionCountNSFW.ToString() + " Questions";
            _SelectMenu.transform.Find("Panel/Both_Button/Count").GetComponent<Text>().text = (_QuestionCountSFW + _QuestionCountNSFW).ToString() + " Questions";
        }
        private void Update()
        {
            if (_Frame >= UpdateRate)
            {
                if (_localVars != SyncedVars) SyncData(false);
                if (_Debug)
                {
                    Debug.Log("[UdonVR] SyncVars: " + SyncedVars.ToString());
                    Debug.Log("[UdonVR] LocalVars: " + SyncedVars.ToString());
                }

                if (_CurrentQuestion == 1)
                {
                    _QuestionBoard.text = QuestionsSFW[(int)_Current];
                }
                else
                {
                    _QuestionBoard.text = QuestionsNSFW[(int)_Current];
                }
                _Frame = 0;
            }
            else
            {
                _Frame++;
            }
        }
        /*
         * Setup
         */
        public void SetupSWF()
        {
            SendCustomNetworkEvent(NetworkEventTarget.Owner, "NetworkSetupSWF");
            NewQuestion();
        }
        public void SetupNSWF()
        {
            SendCustomNetworkEvent(NetworkEventTarget.Owner, "NetworkSetupNSWF");
            NewQuestion();
        }
        public void SetupBoth()
        {
            SendCustomNetworkEvent(NetworkEventTarget.Owner, "NetworkSetupBoth");
            NewQuestion();
        }
        public void NetworkSetupSWF()
        {
            _GameType = 1;
        }
        public void NetworkSetupNSWF()
        {
            _GameType = 2;
        }
        public void NetworkSetupBoth()
        {
            _GameType = 3;
        }

        public void Setup()
        {
            SendCustomNetworkEvent(NetworkEventTarget.All, "NetworkSetup");
        }
        public void NetworkSetup()
        {
            _SelectMenu.SetActive(false);
            _GameMenu.SetActive(true);
        }
        public void UnSetup()
        {
            SendCustomNetworkEvent(NetworkEventTarget.All, "NetworkUnSetup");
        }
        public void NetworkUnSetup()
        {
            _SelectMenu.SetActive(true);
            _GameMenu.SetActive(false);
        }
        /*
        *
        */
        public void NewQuestion()
        {
            SendCustomNetworkEvent(NetworkEventTarget.All, "NetworkSetup");
            SendCustomNetworkEvent(NetworkEventTarget.Owner, "NetworkNewQuestion");
        }
        public void NetworkNewQuestion()
        {
            switch (_GameType)
            {
                case 1:
                    PullSFW();
                    break;
                case 2:
                    PullNSFW();
                    break;
                case 3:
                    if (Random.Range(0, 2) == 0)
                    {
                        PullSFW();
                    }
                    else
                    {
                        PullNSFW();
                    }
                    break;
            }
            SyncData(true);
        }

        private void PullSFW()
        {
            _CurrentQuestion = 1;
            _Current = Random.Range(0, _QuestionCountSFW);
        }
        private void PullNSFW()
        {
            _CurrentQuestion = 2;
            _Current = Random.Range(0, _QuestionCountNSFW);
        }

        public void CircleTrigger()
        {

            if (Networking.LocalPlayer.isMaster)
            {
                SendCustomNetworkEvent(NetworkEventTarget.All, "Circle");
            }
        }
        public void ClassicTrigger()
        {
            if (Networking.LocalPlayer.isMaster)
            {
                SendCustomNetworkEvent(NetworkEventTarget.All, "Classic");
            }
        }
        public void Circle()
        {
            _Circleobj.SetActive(true);
            _Classicobj.SetActive(false);
        }
        public void Classic()
        {
            _Circleobj.SetActive(false);
            _Classicobj.SetActive(true);
        }
    }
}
