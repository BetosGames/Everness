using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GUIChat : GUI
{
    public static GUIChat INSTANCE;
    public KeyCode openChat;
    public KeyCode openChatWithCommand;
    public KeyCode closeChat;
    public KeyCode sendChat;
    public KeyCode sendHistoryUp;
    public KeyCode sendHistoryDown;

    public GameObject ChatItemGO;
    public GameObject chatHolder;
    public TMP_InputField chatBox;
    public ScrollRect chatScrollBox;

    private float originalScrollSensitivity;
    private List<string> sendHistory = new List<string>();
    private int sendHistoryIndex;

    private bool forCommand;

    private bool forceCaretAtEnd;
    private bool firstTimeOpening = true;

    private void Start()
    {
        INSTANCE = this;
        RegisterGUI();
        originalScrollSensitivity = chatScrollBox.scrollSensitivity;    
    }

    IEnumerator FirstTimeOpening()
    {
        firstTimeOpening = false;
        chatBox.ActivateInputField();
        forceCaretAtEnd = true;
        yield return new WaitUntil(() => chatBox.caretPosition == chatBox.text.Length);
        forceCaretAtEnd = false;
    }

    public override void OnOpen()
    {
        Player.getLocalPlayer().disableControl = true;

        chatBox.text = forCommand ? "/" : "";
        sendHistoryIndex = -1;

        LayoutRebuilder.ForceRebuildLayoutImmediate(chatBox.GetComponent<RectTransform>());
        chatBox.ActivateInputField();
        chatBox.caretPosition = chatBox.text.Length;

        if (firstTimeOpening) StartCoroutine(FirstTimeOpening());
        
    }

    public override void OnClose()
    {
        Player.getLocalPlayer().disableControl = false;
    }

    public override void Always()
    {
        if (forceCaretAtEnd)
        {
            chatBox.caretPosition = chatBox.text.Length;
        }

        if (Input.GetKeyDown(openChatWithCommand))
        {
            forCommand = true;
            TryOpenGUI();
        }

        if (Input.GetKeyDown(openChat))
        {
            forCommand = false;
            TryOpenGUI();
        }

        if (Input.GetKeyDown(closeChat))
        {
            TryCloseGUI();
        }

        if (Input.GetKeyDown(sendChat) && IsOpen())
        {
            if (chatBox.text != "")
            {
                if (chatBox.text[0] == '/')
                {
                    string[] rawArgs = chatBox.text.TrimStart('/').Split(' ');

                    Command targetCommand = Registry.INSTANCE.GetCommandFromID(rawArgs[0]);

                    if(targetCommand != null)
                    {
                        string commandResult = targetCommand.Run(Player.getLocalPlayer(), rawArgs.Skip(1).ToArray());

                        switch (commandResult)
                        {
                            case Command.NO_PERMISSION:
                                WriteLine($"<red>Permission level not high enough to run this!");
                                break;
                            case Command.INVALID_SYNTAX:
                                WriteLine($"<red>Invalid Syntax!");
                                break;
                            default:
                                if(commandResult != "") WriteLine(commandResult);
                                break;
                        }
                    }
                    else
                    {
                        WriteLine($"<red>Command does not exist!");
                    }
                      
                }
                else
                {
                    WriteLine($"<yellow>[{Player.getLocalPlayer()}]<reset> {chatBox.text}");
                }

                if (sendHistory.Count == 0 || !sendHistory[sendHistory.Count - 1].Equals(chatBox.text)) sendHistory.Add(chatBox.text);
                TryCloseGUI();

            }
        }

        if (IsOpen())
        {
            chatBox.ActivateInputField();
            Player.getLocalPlayer().disableControl = true;
            //chatScrollBox.GetComponent<RectTransform>().offsetMax = new Vector2(chatScrollBox.GetComponent<RectTransform>().offsetMax.x, 0);
            chatScrollBox.scrollSensitivity = originalScrollSensitivity;
            chatBox.gameObject.SetActive(true);

            if (Input.GetKeyDown(sendHistoryUp))
            {

                if(sendHistoryIndex < sendHistory.Count - 1)
                {
                    sendHistoryIndex += 1;
                    chatBox.text = sendHistory[sendHistory.Count - sendHistoryIndex - 1];
                }

                chatBox.caretPosition = chatBox.text.Length;
            }

            if (Input.GetKeyDown(sendHistoryDown))
            {
                if(sendHistory.Count > 0)
                {
                    if (sendHistoryIndex > 0)
                    {
                        sendHistoryIndex -= 1;
                        chatBox.text = sendHistory[sendHistory.Count - sendHistoryIndex - 1];
                    }
                    else
                    {
                        chatBox.text = "";
                        sendHistoryIndex = -1;
                    }
                }

                chatBox.caretPosition = chatBox.text.Length;
            }
        }
        else
        {
            chatScrollBox.verticalNormalizedPosition = 0;
            //chatScrollBox.GetComponent<RectTransform>().offsetMax = new Vector2(chatScrollBox.GetComponent<RectTransform>().offsetMax.x, -340);
            chatScrollBox.scrollSensitivity = 0;

            chatBox.gameObject.SetActive(false);
        }
    }

    //Writes a line considering color code syntax. Returns if syntax is correct.
    public bool WriteLine(string text)
    {
        if(!text.Contains("<") && !text.Contains(">"))
        {
            WriteLineRaw(string.Join("", text));
            return true;
        }

        Dictionary<string, Color> colorCodes = new Dictionary<string, Color>();

        colorCodes.Add("reset", Color.white);
        colorCodes.Add("white", Color.white);
        colorCodes.Add("red", Color.red);
        colorCodes.Add("orange", new Color32(255, 104, 0, 255));
        colorCodes.Add("yellow", new Color32(255, 206, 0, 255));
        colorCodes.Add("green", Color.green);
        colorCodes.Add("blue", new Color32(0, 90, 255, 255));
        colorCodes.Add("purple", new Color32(155, 0, 255, 255));



        string finalText = $"<color={Extra.ColorHexFromUnityColor(colorCodes["reset"])}>";
        bool readingColorCode = false;
        string inputedColorCode = "";


        for (int i = 0; i < text.Length; i++)
        {
            if (text[i].Equals('<'))
            {
                if (readingColorCode)
                {
                    return false;
                }
                else
                {
                    readingColorCode = true;
                    inputedColorCode = "";
                    continue;
                }
            }

            if (text[i].Equals('>'))
            {
                if (readingColorCode)
                {
                    if (inputedColorCode.Equals(""))
                    {
                        return false;
                    }
                    else
                    {
                        if (colorCodes.ContainsKey(inputedColorCode))
                        {
                            readingColorCode = false;
                            finalText += $"<color={Extra.ColorHexFromUnityColor(colorCodes[inputedColorCode])}>";
                            continue;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    return false;
                }
            }

            if (readingColorCode)
            {
                if(i == text.Length - 1)
                {
                    return false;
                }
                else
                {
                    inputedColorCode += text[i];
                }
            }
            else
            {
                finalText += text[i];
            }
        }

        WriteLineRaw(finalText);
        return true;
    }

    //Writes a line considering nothing but the string provided
    public void WriteLineRaw(string text)
    {
        ChatItem newChatItem = Instantiate(ChatItemGO, chatHolder.transform).GetComponent<ChatItem>();
        newChatItem.SetText(text);
    }

    public void SendCommand(string command)
    {
        string[] args = command.Split(' ');
    }
}
