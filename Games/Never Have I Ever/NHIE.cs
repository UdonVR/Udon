using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;
using VRC.Udon.Common.Interfaces;

public class NHIE : UdonSharpBehaviour
{

    //Questions from
    //https://lifehacks.io/never-have-i-ever-questions/
    //https://herway.net/entertainment/650-never-have-i-ever-questions/
    public string[] QuestionsSFW = new string[] { "shoplifted", "fainted", "hitchhiked",
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
    public string[] QuestionsNSFW = new string[] { "Given a lap dance", "Received a lap dance",
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

    [UdonSynced] int Current;
    [UdonSynced] string GameType;
    [UdonSynced] string CurrentQuestion;

    private Text QuestionBoard;
    private int QuestionCountSFW;
    private int QuestionCountNSFW;
    private GameObject Circleobj;
    private GameObject Classicobj;
    private GameObject SelectMenu;
    private GameObject GameMenu;

    private void Start()
    {
        Circleobj = transform.Find("Round").gameObject;
        Classicobj = transform.Find("Classic").gameObject;
        SelectMenu = transform.Find("Selecting_Canvas").gameObject;
        GameMenu = transform.Find("Playing_Canvas").gameObject;
        QuestionBoard = GameMenu.transform.Find("QuestionText").GetComponent<Text>();
        QuestionCountSFW = QuestionsSFW.Length;
        QuestionCountNSFW = QuestionsNSFW.Length;
        SelectMenu.transform.GetChild(0).Find("SFW_Button").Find("Count").GetComponent<Text>().text = QuestionCountSFW.ToString() + " Questions";
        SelectMenu.transform.GetChild(0).Find("NSFW_Button").Find("Count").GetComponent<Text>().text = QuestionCountNSFW.ToString() + " Questions";
        SelectMenu.transform.GetChild(0).Find("Both_Button").Find("Count").GetComponent<Text>().text = (QuestionCountSFW + QuestionCountNSFW).ToString() + " Questions";
    }
    private void Update()
    {
        if (CurrentQuestion == "SFW")
        {
            QuestionBoard.text = QuestionsSFW[Current];
        }
        else
        {
            QuestionBoard.text = QuestionsNSFW[Current];
        }
    }

    public void SetupSWF()
    {
        //Networking.SetOwner(Networking.LocalPlayer, gameObject);
        GameType = "SFW";
        NewQuestion();
    }
    public void SetupNSWF()
    {
        //Networking.SetOwner(Networking.LocalPlayer, gameObject);
        GameType = "NSFW";
        NewQuestion();
    }
    public void SetupBoth()
    {
        //Networking.SetOwner(Networking.LocalPlayer, gameObject);
        GameType = "BOTH";
        NewQuestion();
    }
    public void Setup()
    {
        SendCustomNetworkEvent(NetworkEventTarget.All, "NetworkSetup");
    }
    public void NetworkSetup()
    {
        SelectMenu.SetActive(false);
        GameMenu.SetActive(true);
    }
    public void UnSetup()
    {
        SendCustomNetworkEvent(NetworkEventTarget.All, "NetworkUnSetup");
    }
    public void NetworkUnSetup()
    {
        SelectMenu.SetActive(true);
        GameMenu.SetActive(false);
    }

    public void NewQuestion()
    {
        //Networking.SetOwner(Networking.LocalPlayer, gameObject);
        SendCustomNetworkEvent(NetworkEventTarget.All, "NetworkSetup");
        SendCustomNetworkEvent(NetworkEventTarget.Owner, "NetworkNewQuestion");
    }
    public void NetworkNewQuestion()
    {
        switch (GameType)
        {
            case "SFW":
                PullSFW();
                break;
            case "NSFW":
                PullNSFW();
                break;
            case "BOTH":
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
    }

        private void PullSFW()
    {
        CurrentQuestion = "SFW";
        Current = Random.Range(0, QuestionCountSFW);
    }
    private void PullNSFW()
    {
        CurrentQuestion = "NSFW";
        Current = Random.Range(0, QuestionCountNSFW);
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
        Circleobj.SetActive(true);
        Classicobj.SetActive(false);
    }
    public void Classic()
    {
        Circleobj.SetActive(false);
        Classicobj.SetActive(true);
    }
}
